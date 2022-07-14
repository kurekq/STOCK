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
    }
}
