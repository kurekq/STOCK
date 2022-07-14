using System;
using System.Collections.Generic;
using System.Text;

namespace Stock
{
    public class Dividends
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

        public string GetSQLInsert()
        {
            return SqlBuilder.GetInsertTableQuery(this);
        }

    }
}
