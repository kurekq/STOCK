using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock
{
    public class Portfolio
    {
        public PortfolioType Type;
        public TransactionManager Transactions;
        public Portfolio (PortfolioType type)
        {
            this.Type = type;
            Transactions = new TransactionManager(Type);
        }
        public PortfolioResult GetCashResult()
        {
            return GetCashResult(DateTime.Now);
        }
        public PortfolioResult GetCashResult(DateTime OnDate)
        {
            return new PortfolioResult()
            {
                CommisionsValue = Transactions.GetCommisionesValue(OnDate),
                ComponentsValue = Transactions.GetComponentsValueToDate(OnDate),
                ComponentsValueWithoutTodaySell = Transactions.GetYesterdayComponentsWithTodayValue(OnDate),
                DividendsValue = Transactions.GetDividendsValue(OnDate),
                TaxesValue = Transactions.GetTaxesValue(OnDate),
                Payins = this.Transactions.GetPayins().Where(p => p.Date <= OnDate).ToList(),
                Cash = Transactions.Cash,
                Evaluations = GetPortfolioEvaluation()
            };
        }

        public List<PortfolioEvaluation> GetPortfolioEvaluation()
        {
            return GetPortfolioEvaluation(Transactions.FirstTransactionDate, Transactions.LastTransactionDate);
        }
        public List<PortfolioEvaluation> GetPortfolioEvaluation(DateTime from, DateTime to)
        {
            List<PortfolioEvaluation> evals = new List<PortfolioEvaluation>();
            if (from > to)
            {
                throw new Exception("Wrong parameter while GetPortfolioEvaluation");
            }

            PortfolioEvaluation before = null;
            DateTime temp = from;
            while (temp <= to)
            { 
                if (Transactions.AnyComponentsHasListinings(temp))
                {
                    PortfolioEvaluation eval = GetPortfolioEvaluation(temp, before);
                    evals.Add(eval);
                    before = eval;
                }

                temp = temp.AddDays(1);
            }

            return evals;
        }
        private PortfolioEvaluation GetPortfolioEvaluation(DateTime onDate, PortfolioEvaluation before)
        {
            decimal unit, price;
            decimal unitBefore = before != null ? before.Unit : 0;

            if (unitBefore > 0)
            {
                unit = before.Unit;
                decimal payin = Transactions.GetPayinAmount(onDate);
                decimal unitsToBuy = Math.Round(payin / before.Price, 2);
                unit += unitsToBuy;

                decimal componentsValue = Transactions.GetComponentsValueToDate(onDate) + Transactions.GetDividendsValueInDay(onDate) - Transactions.GetDividendsTaxValueInDay(onDate);
               
                if (componentsValue > 0)
                {
                    price = Math.Round(componentsValue / unit, 2);

                    decimal sellValue = Transactions.GetSellTransValueInDay(onDate);
                    
                    if (sellValue > 0)
                    {
                        decimal yesterdayComponentsTodayValue = Transactions.GetYesterdayComponentsWithTodayValue(onDate);
                        decimal priceForSell = price = Math.Round(yesterdayComponentsTodayValue / unit, 2);
                        decimal unitsSelled = Math.Round(sellValue / priceForSell, 2);
                        unit -= unitsSelled;
                    }             
                }
                else
                {
                    price = 0;
                    unit = 0;
                }
            }
            else
            {
                price = 100;
                unit = Math.Round(Transactions.GetComponentsValueToDate(onDate) / price, 2);
            }

            if (unit == 0)
            {
                price = 0;
            }
            
            PortfolioEvaluation ev = new PortfolioEvaluation(onDate, unit, price, before);

            return ev;
        }

    }
}
