using System;
using System.Collections.Generic;
using System.Text;

namespace Stock
{
    public class PositionTransaction : ITransaction
    {
        protected DateTime Date;
        protected decimal _price;
        protected decimal Price
        {
            get
            {
                if (_price == default)
                {
                    _price = Position.GetPrice(Date);
                }
                return _price;
            }
            set
            {
                _price = value;
            }
        }
        public IPosition Position
        {
            get;
            protected set;
        }
        public decimal Amount
        {
            get;
            protected set;
        }
        public PositionTransaction(DateTime date, IPosition position, decimal amount)
        {          
            this.Position = position;
            this.Date = position.GetNearestListiningDateTime(date);
            this.Amount = amount;
        }
        public DateTime GetDateTime()
        {
            return Date;
        }
        public decimal GetValue()
        {
            return Amount * Price;
        }

        public virtual string GetDescript()
        {
            return "";
        }

        public virtual List<ITransaction> Divide(decimal amount)
        {
            if (this.Amount <= amount)
            {
                throw new Exception("Its not possible to divide Transaction - amount conflict");
            }

            List<ITransaction> DividedTransactions = new List<ITransaction>();
            DividedTransactions.Add(new PositionTransaction(this.Date, this.Position, this.Amount - amount));
            DividedTransactions.Add(new PositionTransaction(this.Date, this.Position, amount));
            return DividedTransactions;
        }
    }
}
