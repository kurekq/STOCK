using System;
using System.Collections.Generic;
using System.Text;

namespace Stock
{
    public class PortfolioComponent
    {
        public DateTime OnDate;
        public IPosition Position;
        public decimal Amount;
        public Currency Currency;
        public decimal GetValue()
        {
            return Amount * Position.GetPrice(OnDate);
        }
        public decimal GetValue(DateTime onOtherDate)
        {
            return Amount * Position.GetPrice(onOtherDate);
        }
    }
}
