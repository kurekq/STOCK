using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock
{
    public class PortfolioResult
    {
        public decimal Drowdown
        {
            get
            {
                decimal drowdown = 0;
                decimal potentialMaxPrice = 0;
                DateTime potentialFrom = default;
                decimal potencialMinPrice = 0;
                foreach (PortfolioEvaluation ev in Evaluations.Where(e => e.Change < 0).OrderBy(e => e.Date))
                {
                    potentialFrom = ev.Before.Date;
                    potentialMaxPrice = ev.Before.Price;
                    potencialMinPrice = Evaluations.Where(e => e.Date > potentialFrom).Min(e => e.Price);

                    drowdown = Math.Max((1 - potencialMinPrice / potentialMaxPrice) * 100, drowdown);                       
                }
                return drowdown;
            }

            
        }
        public decimal ProfitPerYear
        {
            get
            {
                if (OverallPayins == 0)
                {
                    return 0;
                }
                double profitDouble = 1 + Decimal.ToDouble(Profit / OverallPayins);
                double AverageListiningsPerYear = 250;
                
                return Convert.ToDecimal(Math.Round(Math.Pow(profitDouble, AverageListiningsPerYear / Evaluations.Count), 4));
            }
        }
        public decimal ProfitPercentage
        {
            get
            {
                return Math.Round(1 + (Profit / OverallPayins), 4);
            }

        }
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
        public double Volatility
        {
            get
            {
                List<decimal> prices = Evaluations.Select(a => (decimal)a.Price).ToList();
                return FinancialMath.CalculateVolatility(prices) / Decimal.ToDouble(prices.Average());
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
