using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Transactions
{
    class DividendTaxTransaction : TaxTransaction
    {
        public DividendTaxTransaction(DateTime date, decimal transactionValue) : base(date, transactionValue) { }

        public override string GetDescript()
        {
            return $"{this.Date.ToString("yyyy-MM-dd")} podatek od dywidendy {this.GetValue()}.";
        }
    }
}
