using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock
{
    public class Stock : ICloneable, IPosition
    {
        [DatabaseField]
        public string ISIN;

        [DatabaseField]
        public DateTime DebutDate;

        [DatabaseField]
        public Int64 SharesAmount;

        [DatabaseField]
        public string Sector;

        [DatabaseField]
        public string Branch;

        [DatabaseField]
        public string BiznesRadarName;

        [DatabaseField]
        public string Ticker;

        [DatabaseField]
        public string FullName;

        [DatabaseField]
        public string MarketName;

        public DateTime? FinancialDebutDate
        {
            get
            {
                if (FinancialReports.Count > 0)
                {
                    return FinancialReports.Min(x => x.PrimaryReport);
                }
                else
                {
                    return null;
                }
                
            }
        }
        public List<ArchiveListinings> ArchiveListenings = new List<ArchiveListinings>();
        public List<FinancialReport> FinancialReports = new List<FinancialReport>();
        public List<Dividends> Dividends = new List<Dividends>(); 
        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(ISIN);
            }
        }

        public object Clone()
        {
            return (Stock)this.MemberwiseClone();
        }
        public string GetSQLInsert()
        {
            return SqlBuilder.GetInsertTableQuery(this);
        }
        public List<string> GetAllSqlsInsert()
        {
            List<string> sqls = new List<string>();
            sqls.Add(this.GetSQLInsert());

            foreach (FinancialReport fr in FinancialReports)
            {
                sqls.Add(fr.GetSQLInsert());
            }

            foreach (ArchiveListinings al in ArchiveListenings)
            {
                sqls.Add(al.GetSQLInsert());
            }

            sqls.AddRange(GetDividendsInserts());
            return sqls;
        }
        public List<string> GetDividendsInserts()
        {
            List<string> sqls = new List<string>();
            foreach (Dividends div in Dividends)
            {
                sqls.Add(div.GetSQLInsert());
            }
            return sqls;
        }
        public void AfterFulfillingFromDatabase()
        {
            FulfillShareAmount();
            FulfillPriceToProfit();
        }

        private void FulfillShareAmount()
        {
            foreach (ArchiveListinings arch in ArchiveListenings.Where(a => a.ListeningDate >= this.FinancialDebutDate).OrderByDescending(a => a.ListeningDate))
            {
                FinancialReport reportForListening = FinancialReports.Where(f => f.PrimaryReport >= arch.ListeningDate).OrderBy(f => f.PrimaryReport).FirstOrDefault();

                if (reportForListening == null)
                {
                    reportForListening = FinancialReports.OrderByDescending(f => f.PrimaryReport).FirstOrDefault();
                }
                    
                if (reportForListening != null)
                {
                    arch.ShareAmount = reportForListening.ShareAmount;
                }
            }
        }
        private void FulfillPriceToProfit()
        {
            foreach (ArchiveListinings arch in ArchiveListenings.Where(a => a.ListeningDate >= this.FinancialDebutDate).OrderBy(a => a.ListeningDate))
            {
                arch.PriceToProfit = new PriceToProfitCalculation(this.FinancialReports, arch).Calculate();
            }
        }

        public string GetSymbol()
        {
            return this.ISIN;
        }

        public string GetFullName()
        {
            return this.FullName;
        }

        public decimal GetPrice(DateTime dt)
        {
            return ArchiveListenings.Where(lis => lis.ListeningDate <= dt).OrderByDescending(lis => lis.ListeningDate).FirstOrDefault().ClosePrice;
        }

        public Currency GetCurrency()
        {
            return null;
        }
    }
}
