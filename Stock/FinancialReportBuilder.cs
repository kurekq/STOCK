using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Stock
{
    public class FinancialReportBuilder
    {
        private Stock stock;
        private List<FinancialReport> Reports;
        public FinancialReportBuilder(Stock stock)
        {
            this.stock = stock;
            this.Reports = new List<FinancialReport>();
        }
        public List<FinancialReport> Build()
        {
            BiznesRadar br = new BiznesRadar();
            FinancialReport report = new FinancialReport();
            
            string HtmlCode = br.GetFinancialIncomeReportHtml(stock.BiznesRadarName);
            string financialReportHtml = HtmlCode;
            string reportTable = HtmlParser.GetHtml(HtmlCode, "<table class=\"report-table\"", @"</table>");
            string periodTr = HtmlParser.GetHtml(reportTable, "<tr>", "</tr>");

            while (true)
            {
                report = new FinancialReport();
                report.ISIN = stock.ISIN;
                string periodTh = HtmlParser.GetHtml(periodTr, "<th class=\"thq h\"", "</th>");
                if (string.IsNullOrEmpty(periodTh))
                {
                    break;
                }
                report.Period = HtmlParser.GetHtmlValue(periodTh, "\">", "<span>").Trim();
                periodTr = HtmlParser.RemoveFromHtml(periodTr, "<th class=\"thq h\"", "</th>");

                Reports.Add(report);
            }

            HtmlCode = financialReportHtml + br.GetFinancialBalanceReportHtml(stock.BiznesRadarName) + br.GetFinancialCashflowReportHtml(stock.BiznesRadarName)
                + br.GetProfitIndicatorsHtml(stock.BiznesRadarName) + br.GetFinancialIndicatorsHtml(stock.BiznesRadarName);

            foreach (FieldInfo fieldInfo in typeof(FinancialReport).GetFields().Where(x => x.Name != "Period"))
            {
                string tr = HtmlParser.GetHtml(HtmlCode, $"data-field=\"{fieldInfo.Name}\">", "</tr>");
                foreach (FinancialReport r in Reports.OrderBy(x => x.Period))
                {
                    string td = HtmlParser.GetHtml(tr, "<span class=\"value\">", "</span>");
                    if (string.IsNullOrEmpty(td))
                    {
                        break;
                    }
                    if (fieldInfo.FieldType.Name.ToLower() == "datetime")
                    {
                        string date = HtmlParser.GetHtmlValue(td, "<span>", "</span>");
                        if (!string.IsNullOrEmpty(date))
                        {
                            DateTime value = Convert.ToDateTime(date);
                            fieldInfo.SetValue(r, value);
                        }
                    }
                    else if (fieldInfo.FieldType.Name.ToLower() == "decimal")
                    {
                        string val = HtmlParser.GetHtmlValue(td, "<span>", "</span>").Replace(" ", string.Empty).Replace("%", string.Empty);
                        if (!string.IsNullOrEmpty(val))
                        {
                            decimal value = decimal.Parse(val, CultureInfo.InvariantCulture);
                            fieldInfo.SetValue(r, value);
                        }
                    }
                    else
                    {
                        Int64 value = Int64.Parse(HtmlParser.GetHtmlValue(td, "<span>", "</span>").Replace(" ", string.Empty), CultureInfo.InvariantCulture);
                        fieldInfo.SetValue(r, value);
                    }

                    tr = HtmlParser.RemoveFromHtml(tr, "<span class=\"value\">", "</span>");
                }
            }
            return new List<FinancialReport>(Reports);
        }
        public List<FinancialReport> Get()
        {
            if (Reports.Count == 0)
            {
                Build();
            }
            return new List<FinancialReport>(Reports);
        }
    }
}
