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


            Portfolio portfolio = new Portfolio(PortfolioType.TAXED);
            //portfolio.Transactions.AddTransaction(new BuyTransaction(new DateTime(2019, 1, 1), pzu, 500));
            portfolio.Transactions.AddTransaction(new BuyTransaction(new DateTime(2019, 1, 1), ambra, 100));
            portfolio.Transactions.AddTransaction(new BuyTransaction(new DateTime(2019, 1, 1), ferro, 100));
            
            portfolio.Transactions.AddTransaction(new BuyTransaction(new DateTime(2019, 2, 1), ambra, 100));
            portfolio.Transactions.AddTransaction(new BuyTransaction(new DateTime(2019, 2, 1), ferro, 100));

            portfolio.Transactions.AddTransaction(new BuyTransaction(new DateTime(2019, 3, 1), ambra, 100));
            portfolio.Transactions.AddTransaction(new BuyTransaction(new DateTime(2019, 3, 1), ferro, 100));

            portfolio.Transactions.AddTransaction(new BuyTransaction(new DateTime(2019, 4, 1), ambra, 100));
            portfolio.Transactions.AddTransaction(new BuyTransaction(new DateTime(2019, 4, 1), ferro, 100));

            portfolio.Transactions.AddTransaction(new BuyTransaction(new DateTime(2019, 5, 1), ambra, 100));
            portfolio.Transactions.AddTransaction(new BuyTransaction(new DateTime(2019, 5, 1), ferro, 100));

            portfolio.Transactions.AddTransaction(new BuyTransaction(new DateTime(2019, 6, 1), ambra, 100));
            portfolio.Transactions.AddTransaction(new BuyTransaction(new DateTime(2019, 6, 1), ferro, 100));

            portfolio.Transactions.AddTransaction(new BuyTransaction(new DateTime(2019, 7, 1), ambra, 100));
            portfolio.Transactions.AddTransaction(new BuyTransaction(new DateTime(2019, 7, 1), ferro, 100));

            portfolio.Transactions.AddTransaction(new BuyTransaction(new DateTime(2019, 8, 1), ambra, 100));
            portfolio.Transactions.AddTransaction(new BuyTransaction(new DateTime(2019, 8, 1), ferro, 100));

            portfolio.Transactions.AddTransaction(new BuyTransaction(new DateTime(2019, 9, 1), ambra, 100));
            portfolio.Transactions.AddTransaction(new BuyTransaction(new DateTime(2019, 9, 1), ferro, 100));

            portfolio.Transactions.AddTransaction(new BuyTransaction(new DateTime(2019, 10, 1), ambra, 100));
            portfolio.Transactions.AddTransaction(new BuyTransaction(new DateTime(2019, 10, 1), ferro, 100));

            portfolio.Transactions.AddTransaction(new BuyTransaction(new DateTime(2019, 11, 1), ambra, 100));
            portfolio.Transactions.AddTransaction(new BuyTransaction(new DateTime(2019, 11, 1), ferro, 100));

            portfolio.Transactions.AddTransaction(new BuyTransaction(new DateTime(2019, 12, 1), ambra, 100));
            portfolio.Transactions.AddTransaction(new BuyTransaction(new DateTime(2019, 12, 1), ferro, 100));

            //portfolio.Transactions.AddTransaction(new SellTransaction(new DateTime(2019, 12, 16), ambra, 750));
            //portfolio.Transactions.AddTransaction(new SellTransaction(new DateTime(2019, 12, 16), ferro, 750));

            portfolio.Transactions.AddTransaction(new SellTransaction(new DateTime(2020, 12, 16), ambra, 1200));
            portfolio.Transactions.AddTransaction(new SellTransaction(new DateTime(2020, 12, 16), ferro, 1200));

            portfolio.Transactions.CalculateTaxForAllYears();

            decimal overallBuy = portfolio.Transactions.Transactions.Where(t => t is BuyTransaction).Sum(t => t.GetValue());
            decimal overallSell = portfolio.Transactions.Transactions.Where(t => t is SellTransaction).Sum(t => t.GetValue());

            //Console.WriteLine(portfolio.Transactions.GetDescript());
            Console.WriteLine(" ");
            Console.WriteLine("wyniki: ");
            PortfolioResult cashResult = portfolio.GetCashResult();
            Console.WriteLine("Wartość portfela: " + cashResult.OveralValue);
            Console.WriteLine("     - pozycje: " + cashResult.ComponentsValue);
            Console.WriteLine("     - gotówka: " + cashResult.Cash);
            Console.WriteLine("     - suma wpłat: " + cashResult.OverallPayins);
            Console.WriteLine("");
            Console.WriteLine("     - dywidendy: " + cashResult.DividendsValue);
            Console.WriteLine("     - podatki: " + cashResult.TaxesValue);
            Console.WriteLine("     - prowizje: " + cashResult.CommisionsValue);

            decimal dd = cashResult.ProfitPerYear;

            decimal drow = cashResult.Drowdown;

            decimal profitPercent = cashResult.Profit / cashResult.OverallPayins;
            decimal d = 1;
            foreach(PortfolioEvaluation eval in cashResult.Evaluations)
            {
                if (eval.Change != null && eval.Change != 0)
                {
                    d *= eval.Change == null ? 1 : (decimal)((eval.Change + 100) / 100);
                    Console.WriteLine($"{eval.Date}, Change = {eval.Change}, d = { d }");
                }
                
            }

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
