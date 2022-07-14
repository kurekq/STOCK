using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock
{
    public class StockIndex : IPosition
    {
        public string Symbol;
        public string FullName;
        public string Currency;
        public List<IndexListinings> IndexListenings;

        public string GetSQLInsert()
        {
            return SqlBuilder.GetInsertTableQuery(this);
        }
        public List<string> GetAllSqlsInsert()
        {
            List<string> sqls = new List<string>();
            sqls.Add(this.GetSQLInsert());

            foreach (IndexListinings al in IndexListenings)
            {
                sqls.Add(al.GetSQLInsert());
            }
            return sqls;
        }

        public string GetSymbol()
        {
            return Symbol;
        }

        public string GetFullName()
        {
            return FullName;
        }

        public decimal GetPrice(DateTime dt)
        {
            return IndexListenings.Where(lis => lis.ListeningDate <= dt).OrderByDescending(lis => lis.ListeningDate).FirstOrDefault().ClosePrice;
        }

        public Currency GetCurrency()
        {
            if (string.IsNullOrEmpty(this.Currency))
            {
                return null;
            }
            else
            {
                return new Currency() { Symbol = this.Currency };
            }            
        }
    }
}
