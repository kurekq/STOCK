using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock
{
    public class Stocks
    {
        public List<Stock> stocks;
        public void FillFromDatabase(string whereClause = "")
        {
            Database db = new Database();
            //stocks = db.GetQueryResult(typeof(Stock), "TICKER = 'TIM'").ConvertAll(x => (Stock)x);
            stocks = db.GetQueryResult(typeof(Stock), whereClause).ConvertAll(x => (Stock)x);
            foreach (Stock s in stocks)
            {
                s.ArchiveListenings = db.GetQueryResult(typeof(ArchiveListinings), $"ISIN = '{s.ISIN}'").ConvertAll(x => (ArchiveListinings)x);

                ArchiveListinings before = null;
                foreach (ArchiveListinings arch in s.ArchiveListenings.OrderBy(a => a.ListeningDate))
                {
                    arch.Before = before;
                    before = arch;
                }

                s.FinancialReports = db.GetQueryResult(typeof(FinancialReport), $"ISIN = '{s.ISIN}'").ConvertAll(x => (FinancialReport)x);
                s.Dividends = db.GetQueryResult(typeof(Dividends), $"ISIN = '{s.ISIN}'").ConvertAll(x => (Dividends)x);

                s.AfterFulfillingFromDatabase();
            }
        }

        public void FillFromDatabaseOnlyStock()
        {
            Database db = new Database();
            //stocks = db.GetQueryResult(typeof(Stock), "TICKER = 'TIM'").ConvertAll(x => (Stock)x);
            stocks = db.GetQueryResult(typeof(Stock)).ConvertAll(x => (Stock)x);
        }
    }
}