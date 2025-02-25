using GeekTrust.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeekTrust.Interfaces
{
    public interface IPortfolio
    {
        void GetCurrentPortfolio( List<string> _FundNames, IAvailableFunds _AvailableFunds );

        void CalculateOverlap( string _FundName, IAvailableFunds _AvailableFunds );
    }
}
