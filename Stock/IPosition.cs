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
        Currency GetCurrency();
        List<IInterest> GetInterest(DateTime Payout, DateTime LastListiningWithLawToDiv);
        bool HasListinings(DateTime dt);
    }
}
