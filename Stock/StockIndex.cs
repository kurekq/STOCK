using System;
using System.Collections.Generic;
using System.Text;

namespace Stock
{
    public class StockIndex
    {
        public string Symbol;
        public string FullName;
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
    }
}
