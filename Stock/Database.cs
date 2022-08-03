using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Stock
{
    public class Database
    {
        private const string CONNECTIONSTRING = @"database=localhost:C:\Users\p.kuriata\Desktop\GPW.fdb;user=SYSDBA;password=masterkey;Dialect=3;Charset=UTF8;";
        public void RunNonQuery(string sql)
        {
            RunNonQuery(new List<string>() { sql });
        }
        public void RunNonQuery(List<string> sqls)
        {
            using (var connection = new FbConnection(CONNECTIONSTRING))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    foreach(string sql in sqls)
                    {
                        using (var command = new FbCommand(sql, connection, transaction))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
            }
        }
        public List<object> GetQueryResult(Type objType, string whereClosure = "")
        {
            using (var connection = new FbConnection(CONNECTIONSTRING))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    string sql = SqlBuilder.GetSelectQuery(objType);
                    if (!string.IsNullOrEmpty(whereClosure))
                    {
                        sql += " WHERE " + whereClosure;
                    }

                    using (FbCommand cmd = new FbCommand(sql, connection, transaction))
                    {
                        using (FbDataReader reader = cmd.ExecuteReader())
                        {
                            DatabaseObjectFiller filler = new DatabaseObjectFiller();

                            return filler.GetFilledObjects(objType, reader);
                        }
                    }
                }
            }
        }
    }
}
