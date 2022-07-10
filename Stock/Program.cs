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
            Database db = new Database();

            //Stocks stocks = new Stocks();
            //stocks.FillFromDatabase();


            GetStocks(Market.GPW);
            GetStocks(Market.NEW_CONNECT);
            //StockIndexes stocks = new StockIndexes();
            //stocks.FillFromDatabase();

            //Currencies currencies = new Currencies();
            //currencies.FillFromDatabase();

            return;

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
        private static List<Stock> GetStocks(Market market, string stockName = "")
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
            }
            else
            {
                marketName = "NC";
            }

            if (!string.IsNullOrEmpty(stockName))
            {
                stockNames.Add(stockName);
            }
            else
            {           
                if (market == Market.GPW)
                {
                    int cnt = 1;
                    foreach (string s in snBuilder.Get(market).Where(x => x != "IFR"))
                    {
                        stockNames.Add(s);
                    }

                }
                else
                {
                    int cnt = 1;
                    foreach (string s in snBuilder.Get(market))
                    {
                        stockNames.Add(s);
                        cnt++;
                    }
                }
            }



            foreach (string sN in stockNames)
            {
                StockBuilder sbuilder = new StockBuilder(sN);
                sbuilder.Build();
                Stock stock = sbuilder.Get();
                
                stock.MarketName = marketName;
                string sqlTest = stock.GetSQLInsert();

                stocks.Add(stock);

                FinancialReportBuilder financialReportBuilder = new FinancialReportBuilder(stock);
                stock.FinancialReports = financialReportBuilder.Get();

                ListeningBuilder archiveListeningsBuilder = new ListeningBuilder(stock);
                stock.ArchiveListenings = archiveListeningsBuilder.GetArchiveListinings();

                db.RunNonQuery(stock.GetAllSqlsInsert());

                Console.WriteLine($"{counter}. {sN}: " + DateTime.Now);
                counter++;
            }
            return stocks;
        }
    }
}
