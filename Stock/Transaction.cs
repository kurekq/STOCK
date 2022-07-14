using System;
using System.Collections.Generic;
using System.Text;

namespace Stock
{
    public class Transaction
    {
        public TransactionType Type;
        public DateTime Date;

        private decimal _price;
        public decimal Price
        {
            get
            {
                if (Position != null)
                {
                    return Position.GetPrice(Date);
                }
                else
                {
                    return _price;
                }
            }
            set
            {
                _price = value;
            }
        }
        public IPosition Position;
        public decimal Amount;
    }
}
