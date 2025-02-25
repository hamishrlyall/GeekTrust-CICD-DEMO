using GeekTrust.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace GeekTrust.Tests
{
    public class FundTests
    {
        private Fund Fund;
        private StringWriter StringWriter;
        private TextWriter OriginalConsoleOut;

        [SetUp]
        public void SetUp( )
        {
            Fund = new Fund( "TEST_FUND_1", new List<string>( ) { "Stock 1", "Stock 2", "Stock 3" } );
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

        [TestCase( "Stock 4" )]
        [TestCase( "Stock 5" )]
        [TestCase( "Stock 6" )]
        public void AddStock_ValidStockName_StockAddedToStocks( string _Input )
        {
            Fund.AddStock( _Input );

            Assert.That( Fund.Stocks.Contains( _Input ) );
        }

        [TestCase( " ", "No stock provided." )]
        [TestCase( "", "No stock provided." )]
        public void AddStock_InvalidStockName_WriteErrorMessage( string _Input, string _Result )
        {
            Fund.AddStock( _Input );

            Assert.That( !Fund.Stocks.Contains( _Input ) );
            var output = StringWriter.ToString( ).Trim( );
            Assert.AreEqual( _Result, output );
        }

        [TestCase( "Stock 1", "Stock 1 is already in TEST_FUND_1's stock list." )]
        [TestCase( "Stock 2", "Stock 2 is already in TEST_FUND_1's stock list." )]
        [TestCase( "Stock 3", "Stock 3 is already in TEST_FUND_1's stock list." )]
        public void AddStock_StockListAlreadyContainsStock_WriteErrorMessage( string _Input, string _Result )
        {
            Fund.AddStock( _Input );

            Assert.That( Fund.Stocks.Contains( _Input ) );
            var output = StringWriter.ToString( ).Trim( );
            Assert.AreEqual( _Result, output );
        }
    }
}
