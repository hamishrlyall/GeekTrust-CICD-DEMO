using GeekTrust.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeekTrust.Model
{
    public class Portfolio : IPortfolio
    {
        public HashSet<Fund> CurrentFunds;
        public Portfolio(  ) 
        {
        }

        public void GetCurrentPortfolio( List<string> _FundNames, IAvailableFunds _AvailableFunds )
        {
            var funds = new HashSet<Fund>( );

            foreach( var fundName in _FundNames )
            {
                var fundFound = _AvailableFunds.GetFundByName( fundName );
                if( fundFound != null )
                {
                    funds.Add( fundFound );
                }
            }

            // Only set the CurrentFunds if any funds were found.
            if( funds.Count > 0 )
            {
                CurrentFunds = funds;
            }
        }

        public void CalculateOverlap( string _FundName, IAvailableFunds _AvailableFunds )
        {
            var overlapFund = _AvailableFunds.GetFundByName( _FundName );

            if( overlapFund == null )
            {
                return;
            }

            if( CurrentFunds.Count( ) > 0 )
            {
                foreach( var fund in CurrentFunds )
                {
                   GetOverlapPercentage( overlapFund, fund );
                }
            }
            else
            {
                Console.WriteLine( "No funds in portfolio to compare." );
            }
        }

        private void GetOverlapPercentage( Fund _OverlapFund, Fund _ExistingFund )
        {
            double commonStockCount = _OverlapFund.Stocks.Intersect( _ExistingFund.Stocks ).Count( );

            double overlap = 2 * ( commonStockCount ) / ( _OverlapFund.Stocks.Count + _ExistingFund.Stocks.Count ) * 100;

            if( overlap > 0 )
            {
                Console.WriteLine( $"{_OverlapFund.Name} {_ExistingFund.Name} {overlap.ToString( $"F{2}" )}%" );
            }
        }
    }
}
