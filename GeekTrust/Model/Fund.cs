using GeekTrust.Interfaces;
using System;
using System.Collections.Generic;

namespace GeekTrust.Model
{
    public class Fund : IFund
    {
        public string Name { get; set; }
        public List<string> Stocks { get; set; }

        public Fund( string _Name, List<string> _Stocks ) 
        {
            Name = _Name;
            Stocks = _Stocks;
        }

        public void AddStock( string _StockName )
        {
            if( string.IsNullOrWhiteSpace( _StockName ) )
            {
                Console.WriteLine( "No stock provided." );
                return;
            }

            if( !Stocks.Contains( _StockName ) )
            {
                Stocks.Add( _StockName );
            }
            else
            {
                Console.WriteLine( $"{_StockName} is already in {Name}'s stock list." );
            }
        }
    }
}
