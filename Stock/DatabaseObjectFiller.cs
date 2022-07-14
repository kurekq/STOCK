using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Stock
{
    public class DatabaseObjectFiller
    {
        public List<object> GetFilledObjects(Type objType, FbDataReader reader)
        {
            List<object> objects = new List<object>();
            while (reader.Read())
            {
                Object obj = Activator.CreateInstance(objType);
                int counter = 0;
                foreach (FieldInfo fi in objType.GetFields().Where(x => x.GetCustomAttribute(typeof(DatabaseField)) != null).OrderBy(x => x.Name))
                {                   
                    if (!fi.FieldType.Name.ToLower().Contains("list"))
                    {
                        if (!reader.IsDBNull(counter))
                        {
                            if (fi.FieldType.Name.ToLower() == "string")
                            {
                                fi.SetValue(obj, reader.GetString(counter));
                            }
                            else if (fi.FieldType.Name.ToLower() == "datetime")
                            {

                                fi.SetValue(obj, reader.GetDateTime(counter));
                            }
                            else if (fi.FieldType.Name.ToLower() == "decimal")
                            {
                                fi.SetValue(obj, reader.GetDecimal(counter));
                            }
                            else if (fi.FieldType.Name.ToLower() == "int32")
                            {
                                fi.SetValue(obj, reader.GetInt32(counter));
                            }
                            else 
                            {
                                fi.SetValue(obj, reader.GetInt64(counter));
                            }
                        }
                        counter++;
                    }
                }
                objects.Add(obj);
            }
            return objects;
        }
    }
}
