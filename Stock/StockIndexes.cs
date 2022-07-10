using System;
using System.Collections.Generic;
using System.Text;

namespace Stock
{
    public class StockIndexes
    {
        public List<StockIndex> stocks;
        public void FillFromDatabase()
        {
            Database db = new Database();
            stocks = db.GetQueryResult(typeof(StockIndex)).ConvertAll(x => (StockIndex)x);
            foreach (StockIndex s in stocks)
            {
                s.IndexListenings = db.GetQueryResult(typeof(IndexListinings), $"SYMBOL = '{s.Symbol}'").ConvertAll(x => (IndexListinings)x);
            }
        }
    }
}
