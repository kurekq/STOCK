using System;
using System.Collections.Generic;
using System.Text;

namespace Stock
{
    public class TaxTransaction : NotPositionTransaction, ICashMinusTransaction
    {
        public TaxTransaction(DateTime date, decimal transactionValue) : base(date, transactionValue) { }

        public override string GetDescript()
        {
            return $"{this.Date.ToString("yyyy-MM-dd")} podatek {this.GetValue()}.";
        }
    }
}
