using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock
{
    public class StockIndex : IPosition
    {
        public string Symbol;
        public string FullName;
        public string Currency;
        public List<IndexListinings> IndexListinings;

        public string GetSQLInsert()
        {
            return SqlBuilder.GetInsertTableQuery(this);
        }
        public List<string> GetAllSqlsInsert()
        {
            List<string> sqls = new List<string>();
            sqls.Add(this.GetSQLInsert());

            foreach (IndexListinings al in IndexListinings)
            {
                sqls.Add(al.GetSQLInsert());
            }
            return sqls;
        }

        public string GetSymbol()
        {
            return Symbol;
        }

        public string GetFullName()
        {
            return FullName;
        }

        public decimal GetPrice(DateTime dt)
        {
            return IndexListinings.Where(lis => lis.ListeningDate <= dt).OrderByDescending(lis => lis.ListeningDate).FirstOrDefault().ClosePrice;
        }

        public Currency GetCurrency()
        {
            if (string.IsNullOrEmpty(this.Currency))
            {
                return null;
            }
            else
            {
                return new Currency() { Symbol = this.Currency };
            }            
        }

        public List<IInterest> GetInterest(DateTime Payout, DateTime LastListiningWithLawToDiv)
        {
            return null;
        }

        public bool HasListinings(DateTime dt)
        {
            return this.IndexListinings.Any(al => al.ListeningDate == dt);
        }

        public decimal GetOpenPrice(DateTime dt)
        {
            return IndexListinings.Where(lis => lis.ListeningDate <= dt).OrderByDescending(lis => lis.ListeningDate).FirstOrDefault().OpenPrice;
        }

        public decimal GetClosePrice(DateTime dt)
        {
            return IndexListinings.Where(lis => lis.ListeningDate <= dt).OrderByDescending(lis => lis.ListeningDate).FirstOrDefault().ClosePrice;
        }

        public decimal GetMinPrice(DateTime dt)
        {
            return IndexListinings.Where(lis => lis.ListeningDate <= dt).OrderByDescending(lis => lis.ListeningDate).FirstOrDefault().MinPrice;
        }

        public decimal GetMaxWPrice(DateTime dt)
        {
            return IndexListinings.Where(lis => lis.ListeningDate <= dt).OrderByDescending(lis => lis.ListeningDate).FirstOrDefault().MaxPrice;
        }

        public DateTime GetNearestListiningDateTime(DateTime dt)
        {
            return IndexListinings.Where(lis => lis.ListeningDate <= dt).OrderByDescending(lis => lis.ListeningDate).Select(lis => lis.ListeningDate).First();
        }
        public double GetVolatility()
        {
            return GetVolatility(default, default);
        }
        public double GetVolatility(DateTime from, DateTime to)
        {
            List<decimal> prices = IndexListinings.Where(a => a.ListeningDate >= from && (a.ListeningDate <= to || to == default)).Select(a => (decimal)a.OpenPrice).ToList();
            return FinancialMath.CalculateVolatility(prices);
        }
    }
}
