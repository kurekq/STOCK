using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace Stock
{
    class Program
    {
        static void Main(string[] args)
        {
            Stocks stocks = new Stocks();
            stocks.FillFromDatabase();

            Stock s = stocks.stocks.Where(x => x.Ticker == "NESTMEDIC").First();

            //StockIndexes stocks = new StockIndexes();
            //stocks.FillFromDatabase();

            //Currencies currencies = new Currencies();
            //currencies.FillFromDatabase();

            return;

            Database db = new Database();
            Currency curr = new Currency();
            List<string> brName = new List<string>();
            brName.Add("CHF-FRANK-SZWAJCARSKI");
            brName.Add("EURO");
            brName.Add("GBP-FUNT-SZTERLING");
            brName.Add("JPY-JEN");
            brName.Add("USD-DOLAR");

            foreach (string br in brName)
            {
                curr.Symbol = br;
                curr.FullName = br;

                ListeningBuilder builder = new ListeningBuilder(curr);
                curr.CurrencyListenings = builder.GetCurrencyListinings();
                db.RunNonQuery(curr.GetAllSqlsInsert());
            }
        }
        private static List<Stock> GetStocks(Market market)
        {
            Database db = new Database();
            StockNamesBuilder snBuilder = new StockNamesBuilder();
            int counter = 1;
            List<Stock> stocks = new List<Stock>();
            List<string> stockNames = new List<string>();
            string marketName;
            if (market == Market.GPW)
            {
                marketName = "GPW";
                int cnt = 1;
                foreach (string s in snBuilder.Get(market).Where(x => x != "IFR"))
                {
                    if (cnt > 1420)
                    {
                        stockNames.Add(s);
                    }
                    cnt++;
                }
                
            }
            else
            {
                marketName = "NC";
                int cnt = 1;
                foreach (string s in snBuilder.Get(market))
                {
                    if (cnt > 193)
                    {
                        stockNames.Add(s);
                    }
                    cnt++;
                }
            }


            foreach (string stockName in stockNames)
            {
                StockBuilder sbuilder = new StockBuilder(stockName);
                sbuilder.Build();
                Stock stock = sbuilder.Get();
                
                stock.MarketName = marketName;
                string sqlTest = stock.GetSQLInsert();

                stocks.Add(stock);

                FinancialReportBuilder financialReportBuilder = new FinancialReportBuilder(stock);
                stock.FinancialReport = financialReportBuilder.Get();

                ListeningBuilder archiveListeningsBuilder = new ListeningBuilder(stock);
                stock.ArchiveListenings = archiveListeningsBuilder.GetArchiveListinings();

                db.RunNonQuery(stock.GetAllSqlsInsert());

                Console.WriteLine($"{counter}. {stockName}: " + DateTime.Now);
                counter++;
            }
            return stocks;
        }
    }

    public class CookieAwareWebClient : WebClient
    {
        private CookieContainer cookie = new CookieContainer();

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (request is HttpWebRequest)
            {
                (request as HttpWebRequest).CookieContainer = cookie;
            }
            return request;
        }
    }
}
