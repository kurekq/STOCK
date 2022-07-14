using System;
using System.Collections.Generic;
using System.Text;

namespace Stock
{
    public interface IListinings
    {
        DateTime GetDateTime();
        decimal GetPrice();
    }
}
