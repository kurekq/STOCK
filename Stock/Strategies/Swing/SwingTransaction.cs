using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Strategies.Swing
{
    public class SwingTransaction
    {
        public DateTime TransactionDate;
        private Type _transactionType;
        public Type TransactionType
        {
            get
            {
                return _transactionType;
            }
            set
            {
                if (value != typeof(SellTransaction) && value != typeof(BuyTransaction))
                {
                    throw new Exception("Problematic type in SwingTransaction");
                }
                _transactionType = value;
            }
        }
    }
}
