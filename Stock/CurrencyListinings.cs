using System;
using System.Collections.Generic;
using System.Text;

namespace Stock
{
    public class CurrencyListinings
    {
        public string Symbol;
        public DateTime ListeningDate;
        public decimal Price;

        public string GetSQLInsert()
        {
            return SqlBuilder.GetInsertTableQuery(this);
        }
    }
}
