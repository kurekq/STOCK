using System;
using System.Collections.Generic;
using System.Text;

namespace Stock
{
    public interface IPosition
    {
        string GetSymbol();
        string GetFullName();
        decimal GetPrice(DateTime dt);
        decimal GetOpenPrice(DateTime dt);
        decimal GetClosePrice(DateTime dt);
        decimal GetMinPrice(DateTime dt);
        decimal GetMaxWPrice(DateTime dt);
        Currency GetCurrency();
        List<IInterest> GetInterest(DateTime Payout, DateTime LastListiningWithLawToDiv);
        bool HasListinings(DateTime dt);
        DateTime GetNearestListiningDateTime(DateTime dt);
        double GetVolatility(DateTime from, DateTime to);
    }
}
