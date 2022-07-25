using System;
using System.Collections.Generic;
using System.Text;

namespace Stock
{
    public class Dividends : IInterest
    {
        [DatabaseField]
        public string ISIN;

        [DatabaseField]
        public int ForYear;

        [DatabaseField]
        public decimal PricePerShare;

        [DatabaseField]
        public decimal CashPayout;

        [DatabaseField]
        public decimal CashReserveCapital;

        [DatabaseField]
        public DateTime WZADate;

        [DatabaseField]
        public DateTime LastListiningWithLawToDiv;

        [DatabaseField]
        public DateTime PayoutDate;

        public decimal GetInterestPerUnit()
        {
            return this.PricePerShare;
        }

        public DateTime GetPayoutDate()
        {
            return this.PayoutDate;
        }

        public string GetSQLInsert()
        {
            return SqlBuilder.GetInsertTableQuery(this);
        }

    }
}
