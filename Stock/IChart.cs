using System;
using System.Collections.Generic;
using System.Text;

namespace Stock
{
    public interface IChart
    {
        long GetUnixTimeSeconds();
        decimal GetOpenPrice();
        decimal GetClosePrice();
        decimal GetMinPrice();
        decimal GetMaxPrice();
    }
}
