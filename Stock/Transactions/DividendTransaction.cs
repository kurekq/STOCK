using System;
using System.Collections.Generic;
using System.Text;

namespace Stock
{
    class DividendTransaction : PositionTransaction, ICashPlusTransaction
    {
        public DividendTransaction(DateTime date, IPosition position, decimal amount, decimal price) : base(date, position, amount)
        {
            if (price == default)
            {
                throw new Exception("DividendTransaction without value");
            }
            this._price = price;
        }
        public override string GetDescript()
        {
            return $"{this.Date.ToString("yyyy-MM-dd")} dywidenda {this.GetValue()} ({this.Price} zł x {this.Amount}) od {this.Position.GetSymbol()} ({this.Position.GetFullName()}).";
        }
    }
}
