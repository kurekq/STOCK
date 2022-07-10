using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Stock
{
    public class SqlBuilder
    {
        public static string GetSelectQuery(Type objType)
        {
            string sql = "";

            sql = $"SELECT ";
            foreach (FieldInfo fi in objType.GetFields().Where(x => x.GetCustomAttribute(typeof(DatabaseField)) != null).OrderBy(x => x.Name))
            {
                  
                if (!fi.FieldType.Name.ToLower().Contains("list"))
                {
                    sql += fi.Name.Substring(0, Math.Min(fi.Name.Length, 31)) + ",";
                }
            }
            
            sql += $" FROM {objType.Name}";
            sql = sql.Replace(", FROM", " FROM");

            return sql;
        }
        public static string GetInsertTableQuery(object obj)
        {
            string sql = "";

            sql = $"INSERT INTO {obj.GetType().Name} (";
            foreach (FieldInfo fi in obj.GetType().GetFields().Where(x => x.GetCustomAttribute(typeof(DatabaseField)) != null))
            {
                if (!fi.FieldType.Name.ToLower().Contains("list"))
                {
                    sql += fi.Name.Substring(0, Math.Min(fi.Name.Length, 31)) + ",";
                }                  
            }
            sql += $") {Environment.NewLine} values (";

            foreach (FieldInfo fi in obj.GetType().GetFields().Where(x => x.FieldType.Name.ToLower() != "isin" && x.GetCustomAttribute(typeof(DatabaseField)) != null))
            {
                if (!fi.FieldType.Name.ToLower().Contains("list"))
                {
                    if (fi.FieldType.Name.ToLower() == "string")
                    {
                        sql += $"'{fi.GetValue(obj).ToString()}',";
                    }
                    else if (fi.FieldType.Name.ToLower() == "datetime")
                    {
                        DateTime dt = (DateTime)fi.GetValue(obj);
                        if (dt.Year == 1)
                        {
                            sql += "null,";
                        }
                        else
                        {
                            sql += $"'{dt.ToString("yyyy-MM-dd")}',";
                        }
                    }
                    else if (fi.FieldType.Name.ToLower() == "decimal")
                    {
                        sql += $"{fi.GetValue(obj).ToString().Replace(",", ".")},";
                    }
                    else
                    {

                        sql += $"{fi.GetValue(obj).ToString()},";
                    }
                }
            }
            sql += ");";
            sql = sql.Replace(",)", ")");
            return sql;
        }
    }
}
