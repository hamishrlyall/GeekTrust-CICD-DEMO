using GeekTrust.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeekTrust.Interfaces
{
    public interface IAvailableFunds
    {
        Fund GetFundByName( string _Name );
    }
}
