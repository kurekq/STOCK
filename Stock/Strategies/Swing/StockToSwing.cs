using Stock.Strategies.Swing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock.Strategies
{
    public class StockToSwing
    {
        public Stock Stock;
        public decimal SwingPrice;
        private int _swingPriceHistoricalBreaks;
        public int SwingPriceHistoricalBreaks
        {
            get
            {
                if (_swingPriceHistoricalBreaks == default)
                {
                    _swingPriceHistoricalBreaks = GetListiningsWithBreakPrice().Count();
                }
                return _swingPriceHistoricalBreaks;
            }
        }
        private List<DateTime> _breaks;
        public List<DateTime> BreakDates
        {
            get
            {
                if (_breaks == null)
                {
                    _breaks = GetListiningsWithBreakPrice().Select(a => a.ListeningDate).ToList();
                }
                return _breaks;
            }
        }
        public bool IsOkToSwing
        {
            get
            {
                return Stock.ArchiveListinings.Count > 200;
            }
        }
        public StockToSwing(Stock s)
        {
            this.Stock = s;
        }

        private List<ArchiveListinings> GetListiningsWithBreakPrice()
        {
            return Stock.ArchiveListinings.Where(a => (a.OpenPrice > SwingPrice && a.Before?.OpenPrice < SwingPrice) || (a.OpenPrice < SwingPrice && a.Before?.OpenPrice > SwingPrice)).ToList();
        }
        public List<SwingTransaction> GetSwingTransaction(decimal profitWaiting)
        {
            List<SwingTransaction> swingTrans = new List<SwingTransaction>();
            decimal priceToBuy = SwingPrice * (1 - profitWaiting / 100);
            decimal priceToSell = SwingPrice * (1 + profitWaiting / 100);

            DateTime lastTransactionDate = default;
            do
            {
                DateTime buyTransaction = Stock.ArchiveListinings.Where(a => a.ListeningDate > lastTransactionDate && a.OpenPrice < priceToBuy && a.Before?.OpenPrice > priceToBuy).OrderBy(a => a.ListeningDate).Select(a => a.ListeningDate).FirstOrDefault();

                if (buyTransaction != default)
                {
                    swingTrans.Add(new SwingTransaction()
                    {
                        TransactionDate = buyTransaction,
                        TransactionType = typeof(BuyTransaction)
                    });
                }
                else
                {
                    break;
                }
                lastTransactionDate = buyTransaction;
                DateTime sellTransaction = Stock.ArchiveListinings.Where(a => a.ListeningDate > lastTransactionDate && a.OpenPrice > priceToSell && a.Before?.OpenPrice < priceToSell).OrderBy(a => a.ListeningDate).Select(a => a.ListeningDate).FirstOrDefault();
                if (sellTransaction != default)
                {
                    swingTrans.Add(new SwingTransaction()
                    {
                        TransactionDate = sellTransaction,
                        TransactionType = typeof(SellTransaction)
                    });
                }
                else
                {
                    break;
                }
                lastTransactionDate = sellTransaction;
            } while (true);

            return swingTrans;
        }
    }
}
