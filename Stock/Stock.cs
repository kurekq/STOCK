using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock
{
    public class Stock : ICloneable
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
                if (FinancialReport.Count > 0)
                {
                    return FinancialReport.Min(x => x.PrimaryReport);
                }
                else
                {
                    return null;
                }
                
            }
        }
        public List<ArchiveListinings> ArchiveListenings = new List<ArchiveListinings>();
        public List<FinancialReport> FinancialReport = new List<FinancialReport>();
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

            foreach (FinancialReport fr in FinancialReport)
            {
                sqls.Add(fr.GetSQLInsert());
            }

            foreach (ArchiveListinings al in ArchiveListenings)
            {
                sqls.Add(al.GetSQLInsert());
            }
            return sqls;
        }
        public void AfterFulfillingFromDatabase()
        {
            FulfillShareAmount();
        }

        private void FulfillShareAmount()
        {
            foreach (ArchiveListinings arch in ArchiveListenings.Where(a => a.ListeningDate >= this.FinancialDebutDate).OrderBy(a => a.ListeningDate))
            {
                FinancialReport reportForListening = FinancialReport.Where(f => f.PrimaryReport >= arch.ListeningDate).OrderBy(f => f.PrimaryReport).FirstOrDefault();

                if (reportForListening == null)
                {
                    reportForListening = FinancialReport.OrderByDescending(f => f.PrimaryReport).FirstOrDefault();
                }
                    
                if (reportForListening != null)
                {
                    arch.ShareAmount = reportForListening.ShareAmount;
                }
            }
        }
    }
}
