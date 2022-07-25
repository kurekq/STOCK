using System;
using System.Collections.Generic;
using System.Text;

namespace Stock
{
    public interface IInterest
    {
        decimal GetInterestPerUnit();
        DateTime GetPayoutDate();
    }
}
