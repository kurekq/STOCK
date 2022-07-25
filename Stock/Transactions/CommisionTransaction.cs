using System;
using System.Collections.Generic;
using System.Text;

namespace Stock
{
    public class CommisionTransaction : NotPositionTransaction, ICashMinusTransaction
    {
        public CommisionTransaction(DateTime date, decimal transactionValue) : base(date, transactionValue) { }
        public override string GetDescript()
        {
            return $"{this.Date.ToString("yyyy-MM-dd")} prowizja {this.GetValue()}.";
        }
    }
}
