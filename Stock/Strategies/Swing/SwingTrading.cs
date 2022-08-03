using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock.Strategies
{
    public class SwingTrading
    {
        public StockToSwing GetStockToSwing(Stock s)
        {
            StockToSwing stockToSwing = new StockToSwing(s);

            if (stockToSwing.IsOkToSwing)
            {
                decimal maxPrice = stockToSwing.Stock.ArchiveListinings.Max(a => a.MaxPrice);
                decimal minPrice = stockToSwing.Stock.ArchiveListinings.Min(a => a.MinPrice);
                List<decimal> prices = GetPotentialPrices(minPrice, maxPrice);
                int maxBreaks = 0;
                decimal perfectPrice = 0;
                foreach (decimal p in prices)
                {
                    int priceBreaks = GetPriceBreak(s.ArchiveListinings, p);
                    if (maxBreaks < priceBreaks)
                    {
                        maxBreaks = priceBreaks;
                        perfectPrice = p;
                    }
                }
                stockToSwing.SwingPrice = perfectPrice;
            }

            return stockToSwing;
        }

        private List<decimal> GetPotentialPrices(decimal minPrice, decimal maxPrice)
        {
            List<decimal> prices = new List<decimal>();
            decimal intervalChange = Math.Round((maxPrice - minPrice) / 20, 2);

            decimal tempPrice = minPrice;
            while (tempPrice + 2 * intervalChange < maxPrice)
            {
                tempPrice += intervalChange;
                prices.Add(tempPrice);
            }
            return prices;          
        }

        private int GetPriceBreak(List<ArchiveListinings> archiveListinings, decimal price)
        {
            return archiveListinings.Where(a => (a.OpenPrice > price && a.Before?.OpenPrice < price) || (a.OpenPrice < price && a.Before?.OpenPrice > price)).Count();
        }
    }
}
