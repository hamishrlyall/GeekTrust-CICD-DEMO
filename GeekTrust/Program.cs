using System;
using System.IO;
using GeekTrust.Model;

namespace GeekTrust
{
    class Program
    {
        public static string FilePath;
        static void Main(string[] _Args )
        {
            try
            {
                string[ ] inputData = File.ReadAllLines( _Args[ 0 ] );
                
                //Generate Data
                var availableFunds = new AvailableFunds( );

                // Create new Portfolio
                var portfolio = new Portfolio( );

                var fundManager = new FundManager( availableFunds, portfolio );

                foreach( var input in inputData )
                {
                    fundManager.ProcessInputCommand( input );
                }
            }
            catch( Exception ex )
            {
                Console.WriteLine( ex.Message );
                Console.ReadLine( );
            }
            finally
            {
                Console.ReadLine( );
            }
        }
    }
}
