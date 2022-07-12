using System;
using System.Collections.Generic;
using System.Text;

namespace Stock
{
    public class Listinings
    {
        [DatabaseField]
        public DateTime ListeningDate;

        [DatabaseField]
        public decimal OpenPrice;

        [DatabaseField]
        public decimal MaxPrice;

        [DatabaseField]
        public decimal MinPrice;

        [DatabaseField]
        public decimal ClosePrice;

        public Listinings Before;

        public decimal? PriceChange
        {
            get
            {
                if (Before != null)
                {
                    return Math.Round(this.ClosePrice / Before.ClosePrice * 100 - 100, 4);
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
