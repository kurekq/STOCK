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
        private BiznesRadar br = new BiznesRadar();
        public FinancialReportBuilder(Stock stock)
        {
            this.stock = stock;
            this.Reports = new List<FinancialReport>();
        }
        private string GetMinPeriod(string methodName)
        {
            string HtmlCode = "";
            if (methodName == "GetFinancialIncomeReportHtml")
            {
                HtmlCode = br.GetFinancialIncomeReportHtml(stock.BiznesRadarName);
            }
            else if (methodName == "GetFinancialBalanceReportHtml")
            {
                HtmlCode = br.GetFinancialBalanceReportHtml(stock.BiznesRadarName);
            }
            else if (methodName == "GetFinancialCashflowReportHtml")
            {
                HtmlCode = br.GetFinancialCashflowReportHtml(stock.BiznesRadarName);
            }
            else if (methodName == "GetProfitIndicatorsHtml")
            {
                HtmlCode = br.GetProfitIndicatorsHtml(stock.BiznesRadarName);
            }
            else if (methodName == "GetFinancialIndicatorsHtml")
            {
                HtmlCode = br.GetFinancialIndicatorsHtml(stock.BiznesRadarName);
            }

            string reportTable = HtmlParser.GetHtml(HtmlCode, "<table class=\"report-table\"", @"</table>");
            string periodTr = HtmlParser.GetHtml(reportTable, "<tr>", "</tr>");

            string periodTh = HtmlParser.GetHtml(periodTr, "<th class=\"thq h", "</th>");
            string period = HtmlParser.GetHtmlValue(periodTh, "\">", "<span>").Trim();
            return period;
        }

        private string GetMinPeriod()
        {

            int counter = 1;
            string firstMaxPeriod = "";
            while (counter <= 5)
            {
                string methodName = "";
                if (counter == 1)
                {
                    methodName = "GetFinancialIncomeReportHtml";
                }
                else if (counter == 2)
                {
                    methodName = "GetFinancialBalanceReportHtml";
                }
                else if (counter == 3)
                {
                    methodName = "GetFinancialCashflowReportHtml";
                }
                else if (counter == 4)
                {
                    methodName = "GetProfitIndicatorsHtml";
                }
                else if (counter == 5)
                {
                    methodName = "GetFinancialIndicatorsHtml";
                }

                string period = GetMinPeriod(methodName);
                if (string.IsNullOrEmpty(firstMaxPeriod) || string.Compare(firstMaxPeriod, period) < 0)
                {
                    firstMaxPeriod = period;
                }
                counter++;
            }

            return firstMaxPeriod;
        }

        public List<FinancialReport> Build()
        {
            FinancialReport report = new FinancialReport();
            
            string HtmlCode = br.GetFinancialIncomeReportHtml(stock.BiznesRadarName);
            string financialReportHtml = HtmlCode;
            string reportTable = HtmlParser.GetHtml(HtmlCode, "<table class=\"report-table\"", @"</table>");
            string periodTr = HtmlParser.GetHtml(reportTable, "<tr>", "</tr>");

            string minPeriod = GetMinPeriod();
            while (true)
            {
                report = new FinancialReport();
                report.ISIN = stock.ISIN;
                string periodTh = HtmlParser.GetHtml(periodTr, "<th class=\"thq h", "</th>");
                if (string.IsNullOrEmpty(periodTh))
                {
                    break;
                }
                string period = HtmlParser.GetHtmlValue(periodTh, "\">", "<span>").Trim();
                if (string.Compare(period, minPeriod) >= 0)
                {
                    report.Period = period;
                    Reports.Add(report);
                }

                periodTr = HtmlParser.RemoveFromHtml(periodTr, "<th class=\"thq h", "</th>");
            }

            HtmlCode = financialReportHtml + br.GetFinancialBalanceReportHtml(stock.BiznesRadarName) + br.GetFinancialCashflowReportHtml(stock.BiznesRadarName)
                + br.GetProfitIndicatorsHtml(stock.BiznesRadarName) + br.GetFinancialIndicatorsHtml(stock.BiznesRadarName);

            foreach (FieldInfo fieldInfo in typeof(FinancialReport).GetFields().Where(x => x.Name != "Period"))
            {
                string tr = HtmlParser.GetHtml(HtmlCode, $"data-field=\"{fieldInfo.Name}\">", "</tr>", GET_HTML_ORDER.LAST);
                foreach (FinancialReport r in Reports.OrderByDescending(x => x.Period))
                {
                    string td = HtmlParser.GetHtml(tr, "<span class=\"value\">", "</span>", GET_HTML_ORDER.LAST);
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

                    tr = HtmlParser.RemoveFromHtml(tr, "<span class=\"value\">", "</span>", GET_HTML_ORDER.LAST);
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
