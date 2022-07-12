using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock
{
    public class Stocks
    {
        public List<Stock> stocks;
        public void FillFromDatabase()
        {
            Database db = new Database();
            stocks = db.GetQueryResult(typeof(Stock), "TICKER = 'TIM'").ConvertAll(x => (Stock)x);
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
                s.AfterFulfillingFromDatabase();
            }
        }
    }
}
