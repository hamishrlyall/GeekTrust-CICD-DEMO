using GeekTrust.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace GeekTrust.Tests
{
    public class FundManagerTests
    {
        private Mock<IAvailableFunds> MockAvailableFunds;
        private Mock<IPortfolio> MockPortfolio;
        private FundManager FundManager;
        private StringWriter StringWriter;
        private TextWriter OriginalConsoleOut;

        [SetUp]
        public void SetUp( )
        {
            MockAvailableFunds = new Mock<IAvailableFunds>( );
            MockPortfolio = new Mock<IPortfolio>( );
            FundManager = new FundManager( MockAvailableFunds.Object, MockPortfolio.Object );
            StringWriter = new StringWriter( );
            OriginalConsoleOut = Console.Out;
            Console.SetOut( StringWriter );
        }

        [TearDown]
        public void TearDown( )
        {
            Console.SetOut( OriginalConsoleOut );
        }

        [TestCase( FundManager.CURRENT_PORTFOLIO + " TEST_CASE_1 TEST_CASE_2")]
        [TestCase( FundManager.CURRENT_PORTFOLIO + " TEST_CASE_2 TEST_CASE_3" )]
        public void ProcessInputCommand_CurrentPortfolioValidInput_ShouldCallGetCurrentPortfolio( string _Input )
        {
            // Arrange
            var splitInput = _Input.Split( null ).ToList( );
            var command = splitInput[ 0 ];
            splitInput.Remove( command );

            MockPortfolio
                .Setup( c => c.GetCurrentPortfolio( splitInput, MockAvailableFunds.Object ) );

            // Act
            FundManager.ProcessInputCommand( _Input );

            // Assert
            MockPortfolio.Verify( v => v.GetCurrentPortfolio( splitInput, MockAvailableFunds.Object ), Times.Once );
            MockPortfolio.VerifyNoOtherCalls( );
            MockAvailableFunds.VerifyNoOtherCalls( );
        }

        [TestCase( FundManager.CURRENT_PORTFOLIO + "", FundManager.CURRENT_PORTFOLIO + " must include at least one argument."  )]
        public void ProcessInputCommand_CurrentPortfolioInvalidInput_ShouldWriteErrorMessage( string _Input, string _Result )
        {
            // Arrange
            var splitInput = _Input.Split( null ).ToList( );
            var command = splitInput[ 0 ];

            // Act
            FundManager.ProcessInputCommand( _Input );

            // Assert
            var output = StringWriter.ToString( ).Trim( );
            Assert.AreEqual( _Result, output );

            MockPortfolio.VerifyNoOtherCalls( );
            MockAvailableFunds.VerifyNoOtherCalls( );
        }

        [TestCase( FundManager.CALCULATE_OVERLAP + " TEST_CASE_1" )]
        public void ProcessInputCommand_CalculateOverlapValidInput_ShouldCallCalculateOverlap( string _Input )
        {
            // Arrange
            var splitInput = _Input.Split( null ).ToList( );
            var command = splitInput[ 0 ];
            splitInput.Remove( command );

            MockPortfolio
                .Setup( c => c.CalculateOverlap( splitInput.FirstOrDefault( ), MockAvailableFunds.Object ) );

            // Act
            FundManager.ProcessInputCommand( _Input );

            //Assert
            MockPortfolio.Verify( v => v.CalculateOverlap( splitInput.FirstOrDefault( ), MockAvailableFunds.Object ), Times.Once );
            MockPortfolio.VerifyNoOtherCalls( );
            MockAvailableFunds.VerifyNoOtherCalls( );
        }

        [TestCase( FundManager.CALCULATE_OVERLAP + " TEST_CASE_1 TEST_CASE_2", FundManager.CALCULATE_OVERLAP + " can only recieve one argument." )]
        public void ProcessInputCommand_CalculateOverlapInvalidInput_ShouldWriteErrorMessage( string _Input, string _Result )
        {
            // Arrange
            var splitInput = _Input.Split( null ).ToList( );
            var command = splitInput[ 0 ];
            splitInput.Remove( command );

            // Act
            FundManager.ProcessInputCommand( _Input );

            // Assert
            var output = StringWriter.ToString( ).Trim( );
            Assert.AreEqual( _Result, output );

            MockPortfolio.VerifyNoOtherCalls( );
            MockAvailableFunds.VerifyNoOtherCalls( );
        }


        [TestCase( "Hello_World", "Hello_World is not a valid input." )]
        [TestCase( FundManager.CURRENT_PORTFOLIO + "TEST_FUND_1", FundManager.CURRENT_PORTFOLIO + "TEST_FUND_1 is not a valid input." )]
        [TestCase( FundManager.CALCULATE_OVERLAP + "TEST_FUND_1", FundManager.CALCULATE_OVERLAP + "TEST_FUND_1 is not a valid input." )]
        [TestCase( FundManager.ADD_STOCK + "TEST_FUND_1", FundManager.ADD_STOCK + "TEST_FUND_1 is not a valid input." )]
        public void ProcessInputCommand_UnknownCommand_ShouldWriteErrorMessage( string _Input, string _Result )
        {
            // Act
            FundManager.ProcessInputCommand( _Input );

            //Assert
            var output = StringWriter.ToString( ).Trim( );
            Assert.AreEqual( _Result, output );
        }
    }
}
