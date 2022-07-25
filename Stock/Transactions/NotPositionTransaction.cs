using System;
using System.Collections.Generic;
using System.Text;

namespace Stock
{
    public class NotPositionTransaction : ITransaction
    {
        protected DateTime Date;
        protected decimal TransactionValue;
        public NotPositionTransaction(DateTime date, decimal transactionValue)
        {
            this.Date = date;
            this.TransactionValue = transactionValue;
        }
        public DateTime GetDateTime()
        {
            return this.Date;
        }
        public decimal GetValue()
        {
            return this.TransactionValue;
        }
        public virtual string GetDescript()
        {
            return "";
        }

        public List<ITransaction> Divide(decimal value)
        {
            if (this.TransactionValue <= value)
            {
                throw new Exception("Its not possible to divide Transaction - amount conflict");
            }

            List<ITransaction> DividedTransactions = new List<ITransaction>();
            DividedTransactions.Add(new NotPositionTransaction(this.Date, this.TransactionValue - value));
            DividedTransactions.Add(new NotPositionTransaction(this.Date, value));
            return DividedTransactions;
        }
    }
}
