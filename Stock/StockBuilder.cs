using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Stock
{
    public class StockBuilder
    {
        private Stock stock;
        public StockBuilder(string biznesRadarName)
        {
            this.stock = new Stock() { BiznesRadarName = biznesRadarName };
        }

        public void Build()
        {
            BiznesRadar biznesRadar = new BiznesRadar();
            string htmlCode = biznesRadar.GetMainPageHtml(stock.BiznesRadarName);

            string profileSummaryTable = HtmlParser.GetHtml(htmlCode, "<table class=\"profileSummary\"", "</table>");
            if (!string.IsNullOrEmpty(profileSummaryTable))
            {
                FillSummaryInfo(profileSummaryTable);
            }
            

            string fullNameContainer = HtmlParser.GetHtml(htmlCode, "<div id=\"fullname-container\">", "</div>");
            if (!string.IsNullOrEmpty(fullNameContainer))
            {
                stock.FullName = GetFullName(fullNameContainer);
            }        

            string tickerContainer = HtmlParser.GetHtml(htmlCode, "<div class=\"profile-h1-c\">", "</div>");
            if (!string.IsNullOrEmpty(tickerContainer))
            {
                stock.Ticker = GetTicker(tickerContainer);
                if (string.IsNullOrEmpty(stock.Ticker))
                {
                    stock.Ticker = stock.BiznesRadarName;
                }
            }

        }

        public Stock Get()
        {
            if (stock.IsEmpty)
            {
                Build();
            }
            return (Stock)stock.Clone();
        }

        private string GetTicker(string tickerContainer)
        {
            string text = HtmlParser.GetHtmlValue(tickerContainer, "<h1>", "</h1>");
            return HtmlParser.GetHtmlValue(text, "(", ")");
        }

        private string GetFullName(string fullNameContainer)
        {
            return HtmlParser.GetHtmlValue(fullNameContainer, "<h2>", "</h2>");
        }

        private void FillSummaryInfo(string profileSummaryTable)
        {
            stock.ISIN = HtmlParser.GetHtmlValue(profileSummaryTable, "<td>", "</td>");
            profileSummaryTable = HtmlParser.RemoveFromHtml(profileSummaryTable, "<tr>", "</tr>");

            string td = "";  HtmlParser.GetHtmlValue(profileSummaryTable, "<td>", "</td>");
            if (profileSummaryTable.Contains("Data debiutu:"))
            {
                td = HtmlParser.GetHtmlValue(profileSummaryTable, "<td>", "</td>");
                stock.DebutDate = DateTime.ParseExact(td, "dd.MM.yyyy", null);
                profileSummaryTable = HtmlParser.RemoveFromHtml(profileSummaryTable, "<tr>", "</tr>");
            }

            td = HtmlParser.GetHtmlValue(profileSummaryTable, "<td>", "</td>");
            stock.SharesAmount = Int64.Parse(HtmlParser.GetHtmlValue(td, "\">", "</a>").Replace(" ", string.Empty));
            profileSummaryTable = HtmlParser.RemoveFromHtml(profileSummaryTable, "<tr>", "</tr>");

            //kapitalizacja, tu nas nie interesuje
            profileSummaryTable = HtmlParser.RemoveFromHtml(profileSummaryTable, "<tr>", "</tr>");

            td = HtmlParser.GetHtmlValue(profileSummaryTable, "<td>", "</td>");
            stock.Sector = HtmlParser.GetHtmlValue(td, "\">", "</a>");
            profileSummaryTable = HtmlParser.RemoveFromHtml(profileSummaryTable, "<tr>", "</tr>");

            td = HtmlParser.GetHtmlValue(profileSummaryTable, "<td>", "</td>");
            stock.Branch = HtmlParser.GetHtmlValue(td, "\">", "</a>");
        }
    }
}