using System;
using System.Collections.Generic;
using System.Text;

namespace Stock
{
    public class PortfolioComponent
    {
        public IPosition Position;
        public decimal Amount;
        public decimal Price;
        public Currency Currency;
    }
}
