using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock
{
    public class TransactionsPair
    {
        public List<BuyTransaction> BuyTransactions = new List<BuyTransaction>();
        public SellTransaction SellTransaction;
        public decimal Tax
        {
            get
            {
                return Math.Round((SellTransaction.GetValue() - BuyTransactions.Sum(b => b.GetValue())) * Config.CapitalGainsTaxValue, 2);
            }
        }
    }
}
