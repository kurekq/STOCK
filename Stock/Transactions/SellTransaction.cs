using System;
using System.Collections.Generic;
using System.Text;

namespace Stock
{
    public class SellTransaction : PositionTransaction, ICashPlusTransaction
    {
        public SellTransaction(DateTime date, IPosition position, decimal amount) : base(date, position, amount) { }

        public override string GetDescript()
        {
            return $"{this.Date.ToString("yyyy-MM-dd")} sprzedaż {this.Position.GetSymbol()} ({this.Position.GetFullName()}) {this.Amount} sztuk po {this.Price}.";
        }

        public override List<ITransaction> Divide(decimal amount)
        {
            if (this.Amount <= amount)
            {
                throw new Exception("Its not possible to divide Transaction - amount conflict");
            }

            List<ITransaction> DividedTransactions = new List<ITransaction>();
            DividedTransactions.Add(new SellTransaction(this.Date, this.Position, this.Amount - amount));
            DividedTransactions.Add(new SellTransaction(this.Date, this.Position, amount));
            return DividedTransactions;
        }
    }
}
