using FirebirdSql.Data.FirebirdClient;
using Stock.Strategies;
using Stock.Strategies.Swing;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
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

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            //stocks.FillFromDatabase("ISIN in ('PLAMBRA00013','PLFERRO00016','PLPKO0000016','PLPZU0000011')");
            stocks.FillFromDatabase();
            stopwatch.Stop();

            DateTime startDate = new DateTime(2012, 1, 1);
            List<Stock> stocksBeforeStart = stocks.stocks.Where(s => s.ArchiveListinings.Any(a => a.ListeningDate < startDate)).OrderBy(s => s.GetVolatility(default, startDate)).ToList();

            Console.WriteLine("Elapsed Time is {0} ms", stopwatch.ElapsedMilliseconds);

            Stock ambra = stocks.stocks[0];
            Stock ferro = stocks.stocks[1];
            Stock pko = stocks.stocks[2];
            Stock pzu = stocks.stocks[3];


            double lvl1Zmiennosc = stocksBeforeStart[34].GetVolatility(default, startDate);
            double lvl2Zmiennosc = stocksBeforeStart[93].GetVolatility(default, startDate);
            double lvl3Zmiennosc = stocksBeforeStart[165].GetVolatility(default, startDate);
            double lvl4Zmiennosc = stocksBeforeStart[299].GetVolatility(default, startDate);
            double lvl5Zmiennosc = stocksBeforeStart[373].GetVolatility(default, startDate);
            double lvl6Zmiennosc = stocksBeforeStart[431].GetVolatility(default, startDate); 

            Portfolio portfolio = new Portfolio(PortfolioType.TAXED);
            List<SwingTradingPortfolio> tradingPortfiolios = new List<SwingTradingPortfolio>();

            foreach (Stock stock in stocks.stocks)
            {
                portfolio = new Portfolio(PortfolioType.TAXED);
                SwingTrading swing = new SwingTrading();
                StockToSwing stockSwing = swing.GetStockToSwing(stock);

                foreach (SwingTransaction sTrans in stockSwing.GetSwingTransaction(10).OrderBy(st => st.TransactionDate))
                {
                    if (sTrans.TransactionType == typeof(BuyTransaction))
                    {
                        portfolio.Transactions.AddTransaction(new BuyTransaction(sTrans.TransactionDate, stockSwing.Stock, 1000));
                    }
                    else if (sTrans.TransactionType == typeof(SellTransaction))
                    {
                        portfolio.Transactions.AddTransaction(new SellTransaction(sTrans.TransactionDate, stockSwing.Stock, 1000));
                    }
                }

                portfolio.Transactions.CalculateTaxForAllYears();


                tradingPortfiolios.Add(new SwingTradingPortfolio() { Portfolio = portfolio, StockSwing = stockSwing });


            }

            string stocksInteresting = "";
            int counter = 0;
            foreach(SwingTradingPortfolio tport in tradingPortfiolios.Where(tp => tp.StockSwing.Stock.ArchiveListinings.OrderByDescending(a => a.ListeningDate).Select(a => a.MaxCap).FirstOrDefault() > 250000000).OrderByDescending(tp => tp.Portfolio.Transactions.Transactions.Count))
            {
                if (counter > 20)
                {
                    break;
                }
                stocksInteresting += Environment.NewLine;
                stocksInteresting += "Stock: " + tport.StockSwing.Stock.FullName;
                stocksInteresting += Environment.NewLine;
                stocksInteresting += "SwingPrice: " + tport.StockSwing.SwingPrice;
                tport.Portfolio.WriteToConsol();
                stocksInteresting += tport.Portfolio.ToString();
                stocksInteresting += Environment.NewLine;
                counter++;
            }

            //string json = new ChartJson(cashResult.Evaluations.ToList().ConvertAll(e => (IChart)e)).GetJson();

            /*
            decimal profitPercent = cashResult.Profit / cashResult.OverallPayins;
            decimal d = 1;
            foreach(PortfolioEvaluation eval in cashResult.Evaluations)
            {
                if (eval.Change != null && eval.Change != 0)
                {
                    d *= eval.Change == null ? 1 : (decimal)((eval.Change + 100) / 100);
                    Console.WriteLine($"{eval.Date}, Change = {eval.Change}, d = { d }");
                }
                
            } */

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
                    stock.ArchiveListinings = archiveListeningsBuilder.GetArchiveListinings();

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
