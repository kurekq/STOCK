using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock
{
    public class Currencies
    {
        public List<Currency> currencies;
        public void FillFromDatabase()
        {
            Database db = new Database();
            currencies = db.GetQueryResult(typeof(Currency)).ConvertAll(x => (Currency)x);
            foreach (Currency c in currencies)
            {
                c.CurrencyListenings = db.GetQueryResult(typeof(CurrencyListinings), $"SYMBOL = '{c.Symbol}'").ConvertAll(x => (CurrencyListinings)x);

                CurrencyListinings before = null;
                foreach (CurrencyListinings curr in c.CurrencyListenings.OrderBy(a => a.ListeningDate))
                {
                    curr.Before = before;
                    before = curr;
                }
            }
        }
    }
}
