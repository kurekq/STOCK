using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock
{
    public class Portfolio
    {       
        private List<Transaction> Transactions = new List<Transaction>();
        public List<PortfolioComponent> Components
        {
            get
            {
                return GetComponents(DateTime.Now);
            }    
        }
        public decimal Cash
        {
            get
            {
                return GetCash(DateTime.Now);
            }
        }

        public decimal GetCash(DateTime OnDate)
        {
            decimal cash = Components.Sum(c => c.Amount * c.Price) -
                Transactions.Where(t => t.Date <= OnDate && t.Type == TransactionType.COMMISSION || t.Type == TransactionType.TAX).Sum(t => t.Amount * t.Price) +
                Transactions.Where(t => t.Date <= OnDate && t.Type == TransactionType.DIVIDEND_PAYOUT).Sum(t => t.Amount * t.Price);
            return cash;
        }

        public List<PortfolioComponent> GetComponents(DateTime OnDate)
        {
            List<PortfolioComponent> components = new List<PortfolioComponent>();
            foreach (IPosition position in Transactions.Where(t => t.Date <= OnDate).Select(t => t.Position).Distinct())
            {
                PortfolioComponent component = new PortfolioComponent();
                decimal amount = Transactions.Where(t => t.Position == position && t.Type == TransactionType.BUY && t.Date <= OnDate).Sum(t => t.Amount) -
                    Transactions.Where(t => t.Position == position && t.Type == TransactionType.SELL && t.Date <= OnDate).Sum(t => t.Amount);
                if (amount != 0)
                {
                    component.Amount = amount;
                    component.Position = position;
                    component.Price = position.GetPrice(OnDate);
                    component.Currency = position.GetCurrency();
                    components.Add(component);
                }
            }
            return components;
        }
        public void AddTransaction(Transaction t)
        {
            Transactions.Add(t);
        }
    }
}
