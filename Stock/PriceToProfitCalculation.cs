using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock
{
    public class PriceToProfitCalculation
    {
        List<FinancialReport> FinancialReports;
        ArchiveListinings Listening;

        public PriceToProfitCalculation(List<FinancialReport> financialReports, ArchiveListinings listening)
        {
            this.Listening = listening;
            this.FinancialReports = new List<FinancialReport>(financialReports);
        }

        public decimal? Calculate()
        {
            decimal? priceToProfit = null;

            List<FinancialReport> fourReports = Get4FinancialReports();

            if (fourReports.Count != 4)
            {
                decimal profit = fourReports.Sum(f => f.IncomeNetProfit);
                priceToProfit = Listening.OpenCap / profit * 1000;
            }

            return priceToProfit;
        }

        private List<FinancialReport> Get4FinancialReports()
        {
            return FinancialReports?.Where(f => f.PrimaryReport <= Listening.ListeningDate)?.OrderByDescending(f => f.PrimaryReport)?.Take(4)?.ToList();
        }
    }
}
