using System;
using System.Collections.Generic;
using System.Text;

namespace Stock
{
    public class Stocks
    {
        public List<Stock> stocks;
        public void FillFromDatabase()
        {
            Database db = new Database();
            stocks = db.GetQueryResult(typeof(Stock)).ConvertAll(x => (Stock)x);
            foreach (Stock s in stocks)
            {
                s.ArchiveListenings = db.GetQueryResult(typeof(ArchiveListinings), $"ISIN = '{s.ISIN}'").ConvertAll(x => (ArchiveListinings)x);
                s.FinancialReport = db.GetQueryResult(typeof(FinancialReport), $"ISIN = '{s.ISIN}'").ConvertAll(x => (FinancialReport)x);
                s.AfterFulfillingFromDatabase();
            }
        }
    }
}
