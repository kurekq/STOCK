using Stock.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock
{
    public class TransactionManager
    {
        public List<ITransaction> Transactions;
        private PortfolioType PortfolioType;
        private List<PortfolioPayin> Payins;
        private List<int> CalculatedTaxForYears;

        public decimal Cash
        {
            get;
            private set;
        }
        public DateTime FirstTransactionDate
        {
            get;
            private set;
        }
        public DateTime LastTransactionDate
        {
            get;
            private set;
        }
        public TransactionManager(PortfolioType portfolioType)
        {
            Transactions = new List<ITransaction>();
            PortfolioType = portfolioType;
            Payins = new List<PortfolioPayin>();
            Cash = 0;
            CalculatedTaxForYears = new List<int>();
        }
        public string GetDescript()
        {
            return string.Join(Environment.NewLine, Transactions.OrderBy(t => t.GetDateTime()).Select(t => t.GetDescript()));
        }
        public List<PortfolioComponent> GetComponents()
        {
            return GetComponents(DateTime.Now);
        }
        private List<PortfolioComponent> Components = new List<PortfolioComponent>();
        public List<PortfolioComponent> GetComponents(DateTime OnDate)
        {
            if (!Components.Any(c => c.OnDate == OnDate))
            {
                FillComponents(OnDate);
            }

            return Components.Where(c => c.OnDate == OnDate).ToList();
        }
        private void FillComponents(DateTime OnDate)
        {
            List<PositionTransaction> positionTrans = Transactions.Where(t => t is PositionTransaction && t.GetDateTime() <= OnDate).ToList().ConvertAll(t => (PositionTransaction)t);

            foreach (IPosition position in positionTrans.Where(t => t.GetDateTime() <= OnDate).Select(t => t.Position).Distinct())
            {
                PortfolioComponent component = new PortfolioComponent();
                decimal amount =
                    positionTrans.Where(t => t.Position == position && t is BuyTransaction).Sum(t => t.Amount) -
                    positionTrans.Where(t => t.Position == position && t is SellTransaction).Sum(t => t.Amount);

                if (amount != 0)
                {
                    component.OnDate = OnDate;
                    component.Amount = amount;
                    component.Position = position;
                    component.Currency = position.GetCurrency();
                    Components.Add(component);
                }
            }
        }
        public bool AnyComponentsHasListinings(DateTime OnDate)
        {
            return GetComponents(OnDate).Any(c => c.Position.HasListinings(OnDate));
        }
        public decimal GetComponentsValueToDate(DateTime OnDate)
        {
            return GetComponents(OnDate).Sum(c => c.GetValue());
        }
        public decimal GetYesterdayComponentsWithTodayValue(DateTime OnDate)
        {
            return GetComponents(OnDate.AddDays(-1)).Sum(c => c.GetValue(OnDate));
        }
        public decimal GetDividendsValue(DateTime OnDate)
        {
            return Transactions.Where(t => t.GetDateTime() <= OnDate && t is DividendTransaction).Sum(t => t.GetValue());
        }
        public decimal GetDividendsValueInDay(DateTime OnDate)
        {
            return Transactions.Where(t => t.GetDateTime() == OnDate && t is DividendTransaction).Sum(t => t.GetValue());
        }
        public decimal GetDividendsTaxValueInDay(DateTime OnDate)
        {
            return Transactions.Where(t => t.GetDateTime() == OnDate && t is DividendTaxTransaction).Sum(t => t.GetValue());
        }
        public decimal GetTaxesValue(DateTime OnDate)
        {
            return Transactions.Where(t => t.GetDateTime() <= OnDate && t is TaxTransaction).Sum(t => t.GetValue());
        }
        public decimal GetSellTransValueInDay(DateTime OnDate)
        {
            return Transactions.Where(t => t.GetDateTime() == OnDate && t is SellTransaction).Sum(t => t.GetValue());
        }
        public decimal GetCommisionesValue(DateTime OnDate)
        {
            return Transactions.Where(t => t.GetDateTime() <= OnDate && t is CommisionTransaction).Sum(t => t.GetValue());
        }
        public void AddTransaction(ITransaction t)
        {
            AddAutomaticTransactions(t);
            AddTransactionWithoutAutomatization(t);
        }
        private void AddTransactionWithoutAutomatization(ITransaction t)
        {
            string validate = Validate(t);
            if (!string.IsNullOrEmpty(validate))
            {
                throw new Exception(validate);
            }

            if (this.FirstTransactionDate == default)
            {
                FirstTransactionDate = t.GetDateTime();
            }
            else
            {
                LastTransactionDate = t.GetDateTime();
            }

            Transactions.Add(t);
            FillPayinsAndCash(t);
        }
        private void FillPayinsAndCash(ITransaction t)
        {
            decimal transactionValue = t.GetValue();
            if (t is ICashPlusTransaction)
            {
                this.Cash += transactionValue;
            }
            else if (t is ICashMinusTransaction)
            {
                if (this.Cash >= transactionValue)
                {
                    this.Cash -= transactionValue;
                }
                else
                {                   
                    Payins.Add(new PortfolioPayin()
                    {
                        Amount = transactionValue - this.Cash,
                        Date = t.GetDateTime()
                    });
                    this.Cash = 0;
                }
            }
        }
        private string Validate(ITransaction t)
        {
            if (Transactions.Any(x => (x is BuyTransaction || x is SellTransaction) && x.GetDateTime() > t.GetDateTime()))
            {
                return $"Exists newer transaction ({t.GetDescript()})";
            }
            if (t is SellTransaction)
            {
                SellTransaction sellTrans = (SellTransaction)t;
                decimal amountInPortfolio = GetComponents().Where(c => c.Position.GetSymbol() == sellTrans.Position.GetSymbol()).Sum(c => c.Amount);
                if (amountInPortfolio < sellTrans.Amount)
                {
                    return $"Not possible to sell this position. Sell: {sellTrans.Amount}, you have: {amountInPortfolio}";
                }
            }
            return "";
        }
        public void AddAutomaticTransactions(ITransaction t)
        {
            PositionTransaction pt = t as PositionTransaction;
            if (pt != null && !(pt is DividendTransaction))
            {                
                AddCommisions(pt);
                if (Transactions.Any(tr => tr is PositionTransaction))
                {
                    DateTime LastTransaction = Transactions.Where(tr => tr is BuyTransaction || tr is SellTransaction).Max(x => x.GetDateTime());
                    if (LastTransaction != t.GetDateTime())
                    {
                        AddDividends(LastTransaction, t.GetDateTime().AddDays(-1));
                    }
                    //first transaction in year
                    if (PortfolioType == PortfolioType.TAXED && LastTransaction.Year != t.GetDateTime().Year)
                    {
                        CalculateTax(LastTransaction.Year);
                    }
                }
            }  
        }
        private void AddCommisions(PositionTransaction t)
        {
            decimal commisionAmount;
            if (t.Position.GetCurrency() == null)
            {
                commisionAmount = Math.Max(Math.Round(t.GetValue() * Config.CommisionPercentagePLN / 100, 2), Config.MinimumCommisionPLN);
            }
            else 
            {
                commisionAmount = Math.Max(Math.Round(t.GetValue() * Config.CommisionPercentageNotPLN / 100, 2), Config.MinimumCommisionNotPLN);
            }
            CommisionTransaction commisionTran = new CommisionTransaction(t.GetDateTime(), commisionAmount);
            AddTransactionWithoutAutomatization(commisionTran);
        }
        private void CalculateTax(int year)
        {
            TaxCalculator taxCalculator = new TaxCalculator(year, Transactions);
            decimal taxValue = taxCalculator.GetTaxValue();
            if (taxValue > 0)
            {
                TaxTransaction taxTrans = new TaxTransaction(new DateTime(year, 12, 31), taxValue);
                AddTransactionWithoutAutomatization(taxTrans);
            }
            CalculatedTaxForYears.Add(year);
        }
        public void CalculateTaxForAllYears()
        {
            int fromYear = FirstTransactionDate.Year;  
            int toYear = LastTransactionDate.Year;

            int tmpYear = fromYear;
            while (tmpYear <= toYear)
            {
                if (!CalculatedTaxForYears.Contains(tmpYear))
                {
                    CalculateTax(tmpYear);
                }
                tmpYear++;
            }
        }
        private void AddDividends(DateTime from, DateTime to)
        {
            if (from > to)
            {
                throw new Exception("Problem while automation dividends - from > to.");
            }
            
            foreach (PortfolioComponent component in GetComponents(from))
            {
                DateTime temp = from;
                while (temp <= to)
                {
                    foreach (IInterest dividend in component.Position.GetInterest(default, temp))
                    {
                        decimal dividendValue = dividend.GetInterestPerUnit();
                        DateTime payoutDate = dividend.GetPayoutDate();
                        DividendTransaction divTrans = new DividendTransaction(payoutDate, component.Position, component.Amount, dividendValue);
                        AddTransactionWithoutAutomatization(divTrans);
                        if (this.PortfolioType == PortfolioType.TAXED)
                        {
                            DividendTaxTransaction tax = new DividendTaxTransaction(payoutDate, Math.Round(dividendValue * component.Amount * Config.CapitalGainsTaxValue, 2));
                            AddTransactionWithoutAutomatization(tax);
                        }                    
                    }
                    temp = temp.AddDays(1);
                }
            }

        }
        public decimal GetPayinAmount(DateTime dt)
        {
            return Payins.Where(p => p.Date == dt).Sum(p => p.Amount);
        }
        private List<DateTime> PortfolioComponentChanged(DateTime from, DateTime to)
        {
            return Transactions.Where(t => (t is SellTransaction || t is BuyTransaction) && t.GetDateTime() >= from && t.GetDateTime() <= to).Select(t => t.GetDateTime()).OrderBy(t => t).ToList();
        }
        public List<PortfolioPayin> GetPayins()
        {
            return new List<PortfolioPayin>(Payins);
        }
    }
}
