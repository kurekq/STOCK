using System;
using System.Collections.Generic;
using System.Text;

namespace Stock
{
    public class PortfolioEvaluation
    {
        public DateTime Date;
        public decimal Unit;
        public decimal Price;
        public PortfolioEvaluation Before;
        public decimal? Change
        {
            get
            {
                if (Before?.Price > 0)
                {
                    return Math.Round(this.Price / this.Before.Price * 100 - 100, 2);
                }
                return null;
            }
        }

        public PortfolioEvaluation(DateTime date, decimal unit, decimal price, PortfolioEvaluation before)
        {
            if (before?.Date >= date)
            {
                throw new Exception("Wrong parameters - PortfolioEvaluation constructor.");
            }
            this.Date = date;
            this.Unit = unit;
            this.Price = price;
            this.Before = before;
        }
    }
}
