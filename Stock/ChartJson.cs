using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock
{
    public class ChartJson
    {
        private List<IChart> ChartsData;
        public ChartJson(List<IChart> chartsData)
        {
            this.ChartsData = chartsData;
        }
        public string GetJson()
        {
            string json = "[" + string.Join(",", 
                ChartsData.Select(c => $"[{c.GetUnixTimeSeconds()}," +
                  $"\"{c.GetOpenPrice().ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}\"," +
                  $"\"{c.GetMaxPrice().ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}\"," +
                  $"\"{c.GetMinPrice().ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}\"," +
                  $"\"{c.GetClosePrice().ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}\"]")) + "]";
            return json;
        }
    }
}
