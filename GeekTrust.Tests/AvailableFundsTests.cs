using GeekTrust.Model;
using NUnit.Framework;
using System;
using System.IO;

namespace GeekTrust.Tests
{
    public class AvailableFundsTests
    {
        private AvailableFunds AvailableFunds;
        private StringWriter StringWriter;
        private TextWriter OriginalConsoleOut;
        [SetUp]
        public void SetUp( )
        {
            AvailableFunds = new AvailableFunds
            {
                Funds =
                {
                    new Fund( "TEST_FUND_1", null ),
                    new Fund( "TEST_FUND_2", null ),
                    new Fund( "TEST_FUND_3", null )
                }
            };
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

        [TestCase( "TEST_FUND_1" )]
        [TestCase( "TEST_FUND_2" )]
        [TestCase( "TEST_FUND_3" )]
        public void GetFundByName_ValidName_ReturnFund(string _Input)
        {
            //Act
            var fund = AvailableFunds.GetFundByName( _Input );

            //Assert
            Assert.IsNotNull( fund );
            Assert.That( fund.Name == _Input );
        }

        [TestCase( "TEST_FUND_4", "FUND_NOT_FOUND" )]
        [TestCase( "TEST_FUND_5", "FUND_NOT_FOUND" )]
        [TestCase( "TEST_FUND_6", "FUND_NOT_FOUND" )]
        public void GetFundByName_InvalidName_WriteErrorToConsoleAndReturnNull( string _Input, string _Result )
        {
            //Act
            var fund = AvailableFunds.GetFundByName( _Input );

            //Assert
            Assert.IsNull( fund );
            var output = StringWriter.ToString( ).Trim( );
            Assert.AreEqual( _Result, output );
        }
    }
}
