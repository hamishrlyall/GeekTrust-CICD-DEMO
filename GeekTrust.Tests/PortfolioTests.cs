using GeekTrust.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GeekTrust.Tests
{
    public class PortfolioTests
    {
        private Portfolio Portfolio;
        private AvailableFunds AvailableFunds;
        private StringWriter StringWriter;
        private TextWriter OriginalConsoleOut;


        [SetUp]
        public void SetUp( )
        {
            AvailableFunds = PrepTestData( );
            Portfolio = new Portfolio( );
            PopulateCurrentPortfolio( );
            StringWriter = new StringWriter( );
            OriginalConsoleOut = Console.Out;
            Console.SetOut( StringWriter );
        }

        [TearDown]
        public void TearDown( )
        {
            // Clean up
            Console.SetOut( OriginalConsoleOut );
        }

        [TestCase( "TEST_FUND_1 TEST_FUND_2 TEST_FUND_3" )]
        [TestCase( "TEST_FUND_1 TEST_FUND_4" )]
        public void GetCurrentPortfolio_NoItemsInPortfolio_ShouldPopulateCurrentPortfolio( string _Input )
        {
            // Arrange
            Portfolio.CurrentFunds.Clear( );
            var arguments = _Input.Split( null ).ToList( );

            // Act
            Portfolio.GetCurrentPortfolio( arguments, AvailableFunds );

            //Assert
            Assert.That( Portfolio.CurrentFunds.Count( ) == arguments.Count, "CurrentFunds should have same number of items as input arguments." );
            foreach( var argument in arguments )
            {
                Assert.That( Portfolio.CurrentFunds.Any( p => p.Name == argument ), $"CurrentFunds should have a fund with Name: '{argument}'." );
            }
        }

        [TestCase( "FAKE_FUND FAKE_FUND2 FAKE_FUND3", new string[ ] {
                    "FUND_NOT_FOUND",
                    "FUND_NOT_FOUND",
                    "FUND_NOT_FOUND"
                    } )]
        [TestCase( "1234 !$%#", new string[ ] {
                    "FUND_NOT_FOUND",
                    "FUND_NOT_FOUND"
                    } )]
        public void GetCurrentPortfolio_InvalidInput_ShouldWriteErrorMessage( string _Input, string[ ] _Results )
        {
            // Arrange
            Portfolio.CurrentFunds.Clear( );
            var arguments = _Input.Split( null ).ToList( );

            // Act
            Portfolio.GetCurrentPortfolio( arguments, AvailableFunds );

            // Assert
            Assert.That( Portfolio.CurrentFunds.Count( ) == 0, "CurrentFunds should have no items." );
            foreach( var argument in arguments )
            {
                Assert.That( !Portfolio.CurrentFunds.Any( p => p.Name == argument ), $"CurrentFunds should not have a fund with Name: '{argument}'." );
            }

            var output = StringWriter.ToString( ).Trim( );
            var outputLines = output.Split( new[ ] { Environment.NewLine }, StringSplitOptions.None );
            Assert.AreEqual( _Results.Length, outputLines.Length );

            for( int i = 0; i < _Results.Length; i++ )
            {
                Assert.AreEqual( _Results[ i ], outputLines[ i ], $"Line {i + 1} output is incorrect." );
            }
        }

        [TestCase( "TEST_FUND_4 TEST_FUND_5 TEST_FUND_6" )]
        [TestCase( "TEST_FUND_4 TEST_FUND_5" )]
        public void GetCurrentPortfolio_PortfolioExists_Overwritten( string _Input ) 
        {
            // Arrange
            var arguments = _Input.Split( null ).ToList( );

            // Act
            Portfolio.GetCurrentPortfolio( arguments, AvailableFunds );

            // Assert
            Assert.That( Portfolio.CurrentFunds.Count( ) == arguments.Count, "CurrentFunds should have same number of items as input arguments." );
            foreach( var argument in arguments )
            {
                Assert.That( Portfolio.CurrentFunds.Any( p => p.Name == argument ), $"CurrentFunds should have a fund with Name: '{arguments}'." );
            }
        }

        [TestCase( "FAKE_FUND FAKE_FUND2 FAKE_FUND3", 
                    new string[ ] {
                    "FUND_NOT_FOUND",
                    "FUND_NOT_FOUND",
                    "FUND_NOT_FOUND"
                    } )]
        [TestCase( "1234 !$%#",
                    new string[ ] {
                    "FUND_NOT_FOUND",
                    "FUND_NOT_FOUND",
                    } )]
        public void GetCurrentPortfolio_PortfolioExists_NotOverwritten( string _Input, string[ ] _Results )
        {
            // Arrange
            var arguments = _Input.Split( null ).ToList( );

            // Act
            Portfolio.GetCurrentPortfolio( arguments, AvailableFunds );

            // Assert
            Assert.That( Portfolio.CurrentFunds.Count( ) == 3, "CurrentFunds should have same number of items as it began with." );
            foreach( var argument in arguments )
            {
                Assert.That( !Portfolio.CurrentFunds.Any( p => p.Name == argument ), $"CurrentFunds should not have a fund with Name: '{argument}'." );
            }

            var output = StringWriter.ToString( ).Trim( );
            var outputLines = output.Split( new[ ] { Environment.NewLine }, StringSplitOptions.None );
            Assert.AreEqual( _Results.Length, outputLines.Length );

            for( int i = 0; i < _Results.Length; i++ )
            {
                Assert.AreEqual( _Results[ i ], outputLines[ i ], $"Line {i + 1} output is incorrect." );
            }
        }

        [TestCase( "TEST_FUND_4", 
                    new string[ ] {
                    "TEST_FUND_4 TEST_FUND_1 26.67%",
                    "TEST_FUND_4 TEST_FUND_2 13.33%",
                    "TEST_FUND_4 TEST_FUND_3 32.26%"
                    } ) ]
        [TestCase( "TEST_FUND_5", 
                    new string[ ] {
                    "TEST_FUND_5 TEST_FUND_1 66.67%",
                    "TEST_FUND_5 TEST_FUND_2 40.00%",
                    "TEST_FUND_5 TEST_FUND_3 32.26%" } ) ]
        public void CalculateOverlap_ValidInput_ShouldWriteOverlapResults( string _Input, string[ ] _Results )
        {
            // Act
            Portfolio.CalculateOverlap( _Input, AvailableFunds );

            // Assert
            var output = StringWriter.ToString( ).Trim();
            var outputLines = output.Split( new[ ] { Environment.NewLine }, StringSplitOptions.None );
            Assert.AreEqual( _Results.Length, outputLines.Length );

            for( int i = 0; i < _Results.Length; i++ )
            {
                Assert.AreEqual( _Results[ i ], outputLines[ i ], $"Line {i + 1} output is incorrect." );
            }
        }

        [TestCase( "FAKE_FUND", "FUND_NOT_FOUND" )]
        [TestCase( "1234", "FUND_NOT_FOUND" )]
        [TestCase( "!$%#", "FUND_NOT_FOUND" )]
        public void CalculateOverlap_InvalidInput_ShouldWriteErrorMessage( string _Input, string _Result )
        {
            // Act
            Portfolio.CalculateOverlap( _Input, AvailableFunds );

            //Assert
            var output = StringWriter.ToString( ).Trim( );
            Assert.AreEqual( _Result, output );
        }

        [TestCase( "TEST_FUND_4", "No funds in portfolio to compare." )]
        [TestCase( "TEST_FUND_5", "No funds in portfolio to compare." )]
        public void CalculateOverlap_EmptyPortfolio_ShouldWriteErrorMessage( string _Input, string _Result ) 
        {
            //Arrange
            Portfolio.CurrentFunds.Clear( );

            // Act
            Portfolio.CalculateOverlap( _Input, AvailableFunds );

            //Assert
            var output = StringWriter.ToString( ).Trim( );
            Assert.AreEqual( _Result, output );
        }

        [TestCase( "TEST_FUND_6" ) ]
        [TestCase( "TEST_FUND_7" )]
        public void CalculateOverlap_NoOverlappingFunds_ShouldWriteNoResult( string _Input )
        {
            // Act
            Portfolio.CalculateOverlap( _Input, AvailableFunds );

            // Assert
            var output = StringWriter.ToString( ).Trim( );
            Assert.IsEmpty( output );
        }


        [TestCase( "TEST_FUND_1" )]
        [TestCase( "TEST_FUND_2" )]
        public void CalculateOverlap_NoStocksInFunds_ShouldWriteNoResult( string _Input ) 
        {
            // Arrange
            Portfolio.CurrentFunds.Clear( );
            Portfolio.CurrentFunds.Add( new Fund( "TEST_FUND_8", new List<string>( ) ) );
            Portfolio.CurrentFunds.Add( new Fund( "TEST_FUND_9", new List<string>( ) ) );

            // Act
            Portfolio.CalculateOverlap( _Input, AvailableFunds );

            // Assert
            var output = StringWriter.ToString( ).Trim( );
            Assert.IsEmpty( output );
        }

        private void PopulateCurrentPortfolio( )
        {
            var root = PrepTestData( );
            Portfolio.CurrentFunds = new HashSet<Fund>();
            Portfolio.CurrentFunds.Add( root.Funds.ElementAt( 0 ) );
            Portfolio.CurrentFunds.Add( root.Funds.ElementAt( 1 ) );
            Portfolio.CurrentFunds.Add( root.Funds.ElementAt( 2 ) );
        }

        private AvailableFunds PrepTestData( )
        {
            return new AvailableFunds( )
            {
                Funds = new HashSet<Fund>( )
                {
                    new Fund( "TEST_FUND_1", new List<string>( )
                    {
                        "TEST STOCK 1",
                        "TEST STOCK 2",
                        "TEST STOCK 3",
                        "TEST STOCK 4",
                        "TEST STOCK 5",
                        "TEST STOCK 6",
                        "TEST STOCK 7",
                        "TEST STOCK 8",
                        "TEST STOCK 9",
                        "TEST STOCK 10"
                    } ),
                    new Fund( "TEST_FUND_2", new List<string>( ) {
                        "TEST STOCK 5",
                        "TEST STOCK 6",
                        "TEST STOCK 7",
                        "TEST STOCK 8",
                        "TEST STOCK 9",
                        "TEST STOCK 10",
                        "TEST STOCK 11",
                        "TEST STOCK 12",
                        "TEST STOCK 13",
                        "TEST STOCK 14"
                    } ),
                    new Fund( "TEST_FUND_3", new List<string>( )
                    {
                        "TEST STOCK 2",
                        "TEST STOCK 4",
                        "TEST STOCK 6",
                        "TEST STOCK 8",
                        "TEST STOCK 10",
                        "TEST STOCK 11",
                        "TEST STOCK 12",
                        "TEST STOCK 14",
                        "TEST STOCK 16",
                        "TEST STOCK 18",
                        "TEST STOCK 20",
                    } ),
                    new Fund( "TEST_FUND_4", new List<string>( )
                    {
                        "TEST STOCK 1",
                        "TEST STOCK 20",
                        "TEST STOCK 15",
                        "TEST STOCK 4",
                        "TEST STOCK 17",
                        "TEST STOCK 18",
                        "TEST STOCK 21",
                        "TEST STOCK 8",
                        "TEST STOCK 3",
                        "TEST STOCK 11",
                        "TEST STOCK 22",
                        "TEST STOCK 23",
                        "TEST STOCK 24",
                        "TEST STOCK 25",
                        "TEST STOCK 26",
                        "TEST STOCK 27",
                        "TEST STOCK 28",
                        "TEST STOCK 29",
                        "TEST STOCK 30",
                        "TEST STOCK 31"
                    } ),
                    new Fund( "TEST_FUND_5", new List<string>( )
                    {
                        "TEST STOCK 1",
                        "TEST STOCK 2",
                        "TEST STOCK 3",
                        "TEST STOCK 4",
                        "TEST STOCK 5",
                        "TEST STOCK 6",
                        "TEST STOCK 7",
                        "TEST STOCK 8",
                        "TEST STOCK 9",
                        "TEST STOCK 10",
                        "TEST STOCK 31",
                        "TEST STOCK 32",
                        "TEST STOCK 33",
                        "TEST STOCK 34",
                        "TEST STOCK 35",
                        "TEST STOCK 36",
                        "TEST STOCK 37",
                        "TEST STOCK 38",
                        "TEST STOCK 39",
                        "TEST STOCK 40",

                    } ),
                    new Fund ( "TEST_FUND_6", new List<string>( )
                    {
                        "TEST STOCK 99",
                        "TEST STOCK 98",
                        "TEST STOCK 97",
                    } ),
                    new Fund( "TEST_FUND_7", new List<string>( )
                    {
                        "TEST STOCK 96",
                        "TEST STOCK 95",
                        "TEST STOCK 94",
                    } ),
                    new Fund( "TEST_FUND_8", new List<string>( ) { } ),
                    new Fund( "TEST_FUND_9", new List<string>( ) { } )
                }
            };
        }
    }
}
