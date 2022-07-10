using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;

namespace Stock
{
    public class ListeningBuilder
    {
        private Stock stock;
        private List<ArchiveListinings> archiveListenings;

        private StockIndex index;
        private List<IndexListinings> indexListenings;

        private Currency currency;
        private List<CurrencyListinings> currencyListenings;
        public ListeningBuilder(Stock stock)
        {
            this.stock = stock;
            archiveListenings = new List<ArchiveListinings>();
        }
        public ListeningBuilder(StockIndex index)
        {
            this.index = index;
            indexListenings = new List<IndexListinings>();
        }
        public ListeningBuilder(Currency currency)
        {
            this.currency = currency;
            currencyListenings = new List<CurrencyListinings>();
        }

        private CurrencyListinings GetCurrencyListeningFromTr(string tr)
        {
            CurrencyListinings currencyList = new CurrencyListinings();
            currencyList.Symbol = this.currency.Symbol;

            ArchiveListinings archList = GetArchiveListeningFromTr(tr);

            currencyList.ListeningDate = archList.ListeningDate;
            currencyList.Price = archList.OpenPrice;

            return currencyList;
        }
        private IndexListinings GetIndexListeningFromTr(string tr)
        {
            IndexListinings indexList = new IndexListinings();
            indexList.Symbol = this.index.Symbol;

            ArchiveListinings archList = GetArchiveListeningFromTr(tr);

            indexList.ListeningDate = archList.ListeningDate;
            indexList.OpenPrice = archList.OpenPrice;
            indexList.MaxPrice = archList.MaxPrice;
            indexList.MinPrice = archList.MinPrice;
            indexList.ClosePrice = archList.ClosePrice;

            return indexList;
        }
        private ArchiveListinings GetArchiveListeningFromTr(string tr)
        {
            ArchiveListinings archiveList = new ArchiveListinings();
            if (stock != null)
            {
                archiveList.ISIN = this.stock.ISIN;
            }
            
            string td = HtmlParser.GetHtmlValue(tr, "<td>", "</td>");
            archiveList.ListeningDate = DateTime.ParseExact(td, "dd.MM.yyyy", null);
            tr = HtmlParser.RemoveFromHtml(tr, "<td>", "</td>");

            archiveList.OpenPrice = decimal.Parse(HtmlParser.GetHtmlValue(tr, "<td>", "</td>"), CultureInfo.InvariantCulture);
            tr = HtmlParser.RemoveFromHtml(tr, "<td>", "</td>");

            string maxPriceStr = HtmlParser.GetHtmlValue(tr, "<td>", "</td>");
            if (!string.IsNullOrEmpty(maxPriceStr))
            {
                archiveList.MaxPrice = decimal.Parse(maxPriceStr, CultureInfo.InvariantCulture);
                tr = HtmlParser.RemoveFromHtml(tr, "<td>", "</td>");
            }

            string minPriceStr = HtmlParser.GetHtmlValue(tr, "<td>", "</td>");
            if (!string.IsNullOrEmpty(minPriceStr))
            {
                archiveList.MinPrice = decimal.Parse(minPriceStr, CultureInfo.InvariantCulture);
                tr = HtmlParser.RemoveFromHtml(tr, "<td>", "</td>");
            }

            string closePriceStr = HtmlParser.GetHtmlValue(tr, "<td>", "</td>");
            if (!string.IsNullOrEmpty(closePriceStr))
            {
                archiveList.ClosePrice = decimal.Parse(closePriceStr, CultureInfo.InvariantCulture);
                tr = HtmlParser.RemoveFromHtml(tr, "<td>", "</td>");
            }

            if (tr.Contains("<td>"))
            {
                tr = HtmlParser.RemoveFromHtml(tr, "<td>", "</td>");
            }
            
            string tradingVolumeString = HtmlParser.GetHtmlValue(tr, "<td>", "</td>");
            if (!string.IsNullOrEmpty(tradingVolumeString))
            {
                archiveList.TradingVolume = decimal.Parse(tradingVolumeString);
            }           

            return archiveList;
        }

        private void Build(Type t)
        {
            BiznesRadar biznesRadar = new BiznesRadar();
            string biznesRadarName = "";
            if (t == typeof(ArchiveListinings))
            {
                biznesRadarName = stock.BiznesRadarName;
            }
            else if (t == typeof(IndexListinings))
            {
                biznesRadarName = index.Symbol;
            }
            else if (t == typeof(CurrencyListinings))
            {
                biznesRadarName = currency.Symbol;
            }

            string htmlCode = biznesRadar.GetListeningHtml(biznesRadarName);
            string pageElement = HtmlParser.GetHtml(htmlCode, "<a class=\"pages_pos\"", "</a>", GET_HTML_ORDER.LAST);

            int numberOfPage = 1;
            if (pageElement != "")
            {
                numberOfPage = int.Parse(HtmlParser.GetHtmlValue(pageElement, "\">", "</a>"));
            }


            int page = 1;
            while (page <= numberOfPage)
            {
                htmlCode = biznesRadar.GetListeningHtml(biznesRadarName, page);

                string table = HtmlParser.GetHtml(htmlCode, "<table class=\"qTableFull\">", @"</table>");
                while (true)
                {
                    string tr = HtmlParser.GetHtml(table, "<tr>", "</tr>");
                    if (string.IsNullOrEmpty(tr))
                    {
                        break;
                    }

                    if (tr.Contains("<td>"))
                    {
                        if (t == typeof(ArchiveListinings))
                        {
                            archiveListenings.Add(GetArchiveListeningFromTr(tr));
                        }
                        else if (t == typeof(IndexListinings))
                        {
                            indexListenings.Add(GetIndexListeningFromTr(tr));
                        }
                        else if (t == typeof(CurrencyListinings))
                        {
                            currencyListenings.Add(GetCurrencyListeningFromTr(tr));
                        }
                    }

                    table = HtmlParser.RemoveFromHtml(table, "<tr>", "</tr>");
                }
                page++;
            }
        }
        public List<ArchiveListinings> GetArchiveListinings()
        {
            if (archiveListenings.Count == 0)
            {
                Build(typeof(ArchiveListinings));
            }
            return new List<ArchiveListinings>(archiveListenings);
        }
        public List<IndexListinings> GetIndexListinings()
        {
            if (indexListenings.Count == 0)
            {
                Build(typeof(IndexListinings));
            }
            return new List<IndexListinings>(indexListenings);
        }
        public List<CurrencyListinings> GetCurrencyListinings()
        {
            if (currencyListenings.Count == 0)
            {
                Build(typeof(CurrencyListinings));
            }
            return new List<CurrencyListinings>(currencyListenings);
        }
    }
}
