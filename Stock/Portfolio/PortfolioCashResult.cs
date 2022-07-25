using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock
{
    public class PortfolioCashResult
    {
        public decimal Profit
        {
            get
            {
                return OveralValue - OverallPayins;
            }
        }
        public decimal OveralValue
        {
            get
            {
                return ComponentsValue + DividendsValue - TaxesValue - CommisionsValue + Cash;
            }        
        }
        public decimal OverallPayins
        {
            get
            {
                return Payins.Sum(p => p.Amount);
            }

        }
        public decimal ComponentsValue;
        public decimal ComponentsValueWithoutTodaySell;
        public decimal DividendsValue;
        public decimal TaxesValue;
        public decimal CommisionsValue;
        public List<PortfolioPayin> Payins;
        public List<PortfolioEvaluation> Evaluations;
        public decimal Cash;
    }
}
