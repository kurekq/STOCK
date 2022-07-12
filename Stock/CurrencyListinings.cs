using System;
using System.Collections.Generic;
using System.Text;

namespace Stock
{
    public class CurrencyListinings
    {
        [DatabaseField]
        public string Symbol;

        [DatabaseField]
        public DateTime ListeningDate;

        [DatabaseField]
        public decimal Price;

        public CurrencyListinings Before;
        public decimal? PriceChange
        {
            get
            {
                if (Before != null)
                {
                    return Math.Round(this.Price / Before.Price * 100 - 100, 4);
                }
                return null;
            }
        }

        public string GetSQLInsert()
        {
            return SqlBuilder.GetInsertTableQuery(this);
        }
    }
}
