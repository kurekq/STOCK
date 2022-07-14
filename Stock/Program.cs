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

            Stocks stocks = new Stocks();

            stocks.FillFromDatabase("ISIN in ('PLAMBRA00013','PLFERRO00016','PLPKO0000016','PLPZU0000011')");

            Stock ambra = stocks.stocks[0];
            Stock ferro = stocks.stocks[1];
            Stock pko = stocks.stocks[2];
            Stock pzu = stocks.stocks[3];


            Portfolio portfolio = new Portfolio();
            portfolio.AddTransaction(
                new Transaction()
                {
                    Amount = 100,
                    Date = new DateTime(2018,1,1),
                    Position = ambra,
                    Type = TransactionType.BUY
                });

            portfolio.AddTransaction(
                new Transaction()
                {
                    Amount = 75,
                    Date = new DateTime(2018, 2, 1),
                    Position = ferro,
                    Type = TransactionType.BUY
                });

            portfolio.AddTransaction(
    new Transaction()
    {
        Amount = 150,
        Date = new DateTime(2018, 3, 1),
        Position = pko,
        Type = TransactionType.BUY
    });

            portfolio.AddTransaction(
new Transaction()
{
Amount = 125,
Date = new DateTime(2021, 3, 1),
Position = pko,
Type = TransactionType.SELL
});
            portfolio.AddTransaction(
new Transaction()
{
Amount = 117,
Date = new DateTime(2022, 3, 7),
Position = pzu,
Type = TransactionType.BUY
});

            portfolio.AddTransaction(
new Transaction()
{
   Amount = 1,
   Date = new DateTime(2022, 3, 7),
   Price = 20.17m,
   Type = TransactionType.COMMISSION
});

            portfolio.AddTransaction(
new Transaction()
{
Amount = 1,
Date = new DateTime(2021, 1, 1),
Price = 1114.99m,
Type = TransactionType.COMMISSION
});

            //GetStocks(Market.GPW);
            //GetStocks(Market.NEW_CONNECT);
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
        private static void DividendsStuff(Stocks stocks)
        {
            Database db = new Database();
            foreach (Stock s in stocks.stocks)
            {
                s.Dividends = new DividendsBuilder(s).Get();

                if (s.Dividends.Count > 0)
                {
                    db.RunNonQuery(s.GetDividendsInserts());
                    Console.WriteLine(s.BiznesRadarName + " dywidendy załadowane do bazy");
                }
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
                        if (cnt > 0)
                        {
                            stockNames.Add(s);
                        }
                        cnt++;
                    }
                }
                else
                {
                    int cnt = 1;
                    foreach (string s in snBuilder.Get(market))
                    {
                        if (cnt > 0)
                        {
                            stockNames.Add(s);
                        }                       
                        cnt++;
                    }
                }
            }

            foreach (string sN in stockNames)
            {
                StockBuilder sbuilder = new StockBuilder(sN);
                sbuilder.Build();
                Stock stock = sbuilder.Get();

                if (!stock.IsEmpty)
                {
                    stock.MarketName = marketName;
                    string sqlTest = stock.GetSQLInsert();

                    stocks.Add(stock);

                    FinancialReportBuilder financialReportBuilder = new FinancialReportBuilder(stock);
                    stock.FinancialReports = financialReportBuilder.Get();

                    ListeningBuilder archiveListeningsBuilder = new ListeningBuilder(stock);
                    stock.ArchiveListenings = archiveListeningsBuilder.GetArchiveListinings();

                    DividendsBuilder divBuilder = new DividendsBuilder(stock);
                    stock.Dividends = divBuilder.Get();

                    db.RunNonQuery(stock.GetAllSqlsInsert());

                    Console.WriteLine($"{counter}. {sN}: " + DateTime.Now);
                }
               
                counter++;
            }
            return stocks;
        }
    }
}
