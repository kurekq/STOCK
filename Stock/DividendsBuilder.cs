using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock
{
    public class DividendsBuilder
    {
        private Stock stock;
        private List<Dividends> Dividends;
        private BiznesRadar br = new BiznesRadar();
        public DividendsBuilder(Stock stock)
        {
            this.stock = stock;
            this.Dividends = new List<Dividends>();
        }

        public List<Dividends> Build()
        {
            List<Dividends> dividends = new List<Dividends>();

            string HtmlCode = br.GetDividendsHtml(stock.BiznesRadarName);
            string reportTable = HtmlParser.GetHtml(HtmlCode, "<div class=\"table-c\"", @"</table>");
            reportTable = HtmlParser.RemoveFromHtml(reportTable, "<tr>", "</tr>");

            
            while (true)
            {
                string tr = HtmlParser.GetHtml(reportTable, "<tr>", "</tr>");
                if (string.IsNullOrEmpty(tr))
                {
                    break;
                }

                string yearTd = HtmlParser.GetHtmlValue(tr, "<td>", "</td>");
                tr = HtmlParser.RemoveFromHtml(tr, "<td>", "</td>");
              
                tr = HtmlParser.RemoveFromHtml(tr, "<td>", "</td>");

                string pricePerShareTd = HtmlParser.GetHtmlValue(tr, "<td>", "</td>");
                tr = HtmlParser.RemoveFromHtml(tr, "<td>", "</td>");

                string cashPayoutTd = HtmlParser.GetHtmlValue(tr, "<td>", "</td>");
                tr = HtmlParser.RemoveFromHtml(tr, "<td>", "</td>");

                string cashReserveCapitalTd = HtmlParser.GetHtmlValue(tr, "<td>", "</td>");
                tr = HtmlParser.RemoveFromHtml(tr, "<td>", "</td>");

                tr = HtmlParser.RemoveFromHtml(tr, "<td>", "</td>");

                string statusTd = HtmlParser.GetHtmlValue(tr, "<div class=\"multiline\">", "</td>").Replace("\t", "").Replace("\n", "");
                List<string> statusesTd = new List<string>();
                while (true)
                {
                    string singleStatus = HtmlParser.GetHtmlValue(statusTd, "<div>", "</div>");
                    if (string.IsNullOrEmpty(singleStatus))
                    {
                        break;
                    }
                    statusTd = HtmlParser.RemoveFromHtml(statusTd, "<div>", "</div>");
                    singleStatus = String.Concat(singleStatus.Where(x => x == '.' || Char.IsDigit(x)));
                    statusesTd.Add(singleStatus);
                }

                string WZADateTd = HtmlParser.GetHtmlValue(tr, "<td class=\"date\">", "</td>").Replace("\t", "").Replace("\n", "");
                List<string> wzaDatesTd = new List<string>();
                if (WZADateTd.Contains("<div class=\"multiline\""))
                {
                    while (true)
                    {
                        string singleDate = HtmlParser.GetHtmlValue(WZADateTd, "<div class=\"multiline\">", "</div>");
                        if (string.IsNullOrEmpty(singleDate))
                        {
                            break;
                        }
                        WZADateTd = HtmlParser.RemoveFromHtml(WZADateTd, "<div class=\"multiline\">", "</div>");
                        wzaDatesTd.Add(singleDate);
                    }
                }
                else
                {
                    wzaDatesTd.Add(WZADateTd);
                }             
                tr = HtmlParser.RemoveFromHtml(tr, "<td class=\"date\">", "</td>");

                string LastListiningsTd = HtmlParser.GetHtmlValue(tr, "<td class=\"date\">", "</td>").Replace("\t", "").Replace("\n", "");
                List<string> LastListiningsListTd = new List<string>();
                if (LastListiningsTd.Contains("<div class=\"multiline\""))
                {
                    while (true)
                    {
                        string singleDate = HtmlParser.GetHtmlValue(LastListiningsTd, "<div class=\"multiline\">", "</div>");
                        if (string.IsNullOrEmpty(singleDate))
                        {
                            break;
                        }
                        LastListiningsTd = HtmlParser.RemoveFromHtml(LastListiningsTd, "<div class=\"multiline\">", "</div>");
                        LastListiningsListTd.Add(singleDate);
                    }
                }
                else
                {
                    LastListiningsListTd.Add(LastListiningsTd);
                }
                tr = HtmlParser.RemoveFromHtml(tr, "<td class=\"date\">", "</td>");

                string PayoutDateTd = HtmlParser.GetHtmlValue(tr, "<td class=\"date\">", "</td>").Replace("\t", "").Replace("\n", "");
                List<string> PayoutDateListTd = new List<string>();
                if (PayoutDateTd.Contains("<div class=\"multiline\">"))
                {
                    while (true)
                    {
                        string singleDate = HtmlParser.GetHtmlValue(PayoutDateTd, "<div class=\"multiline\">", "</div>");
                        if (string.IsNullOrEmpty(singleDate))
                        {
                            break;
                        }
                        PayoutDateTd = HtmlParser.RemoveFromHtml(PayoutDateTd, "<div class=\"multiline\">", "</div>");
                        PayoutDateListTd.Add(singleDate);
                    }
                }
                else
                {
                    PayoutDateListTd.Add(LastListiningsTd);
                }
                tr = HtmlParser.RemoveFromHtml(tr, "<td class=\"date\">", "</td>");

                reportTable = HtmlParser.RemoveFromHtml(reportTable, "<tr>", "</tr>");
            }

            //"";

            return dividends;
        }

        public List<Dividends> Get()
        {
            if (Dividends.Count == 0)
            {
                Build();
            }
            return new List<Dividends>(Dividends);
        }
    }
}
