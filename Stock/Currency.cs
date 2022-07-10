using System;
using System.Collections.Generic;
using System.Text;

namespace Stock
{
    public class Currency
    {
        public string Symbol;
        public string FullName;
        public List<CurrencyListinings> CurrencyListenings;

        public string GetSQLInsert()
        {
            return SqlBuilder.GetInsertTableQuery(this);
        }
        public List<string> GetAllSqlsInsert()
        {
            List<string> sqls = new List<string>();
            sqls.Add(this.GetSQLInsert());

            foreach (CurrencyListinings al in CurrencyListenings)
            {
                sqls.Add(al.GetSQLInsert());
            }
            return sqls;
        }
    }
}
