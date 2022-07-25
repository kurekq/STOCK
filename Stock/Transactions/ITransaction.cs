using System;
using System.Collections.Generic;
using System.Text;

namespace Stock
{
    public interface ITransaction
    {
        decimal GetValue();
        DateTime GetDateTime();
        string GetDescript();
        List<ITransaction> Divide(decimal amount);
    }
}
