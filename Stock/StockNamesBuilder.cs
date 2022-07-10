using System;
using System.Collections.Generic;
using System.Text;

namespace Stock
{
    public class StockNamesBuilder
    {
        public List<string> Get (Market market)
        {
            List<string> stocks = new List<string>();

            BiznesRadar br = new BiznesRadar();
            string HtmlCode = "";
            if (market == Market.GPW)
            {
                HtmlCode = br.GetAllGPWStocksHtml();
            }
            else if (market == Market.NEW_CONNECT)
            {
                HtmlCode = br.GetAllNewConnectStocksHtml();
            }

            HtmlCode = HtmlParser.GetHtml(HtmlCode, "table class=\"qTableFull\">", "</table>");
            string stocksTable = HtmlParser.GetHtml(HtmlCode, "table class=\"qTableFull\">", "</table>");

            while(true)
            {
                string tr = HtmlParser.GetHtml(stocksTable, "<tr id=\"", "</tr>");
                if (string.IsNullOrEmpty(tr))
                {
                    break;
                }
                else
                {
                    string href = HtmlParser.GetHtmlValue(tr, "href=\"", "\" ");
                    string stockName = href.Replace("/notowania/", string.Empty);
                    stocks.Add(stockName);
                    stocksTable = HtmlParser.RemoveFromHtml(stocksTable, "<tr id=\"", "</tr>");
                }
            }           
            return stocks;

        }
    }
}
