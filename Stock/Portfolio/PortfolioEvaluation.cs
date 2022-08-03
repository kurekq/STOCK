using System;
using System.Collections.Generic;
using System.Text;

namespace Stock
{
    public class PortfolioEvaluation : IChart
    {
        public DateTime Date;
        public decimal Unit;
        public decimal Price;

        public decimal OpenPrice;
        public decimal ClosePrice;
        public decimal MinPrice;
        public decimal MaxPrice;

        public PortfolioEvaluation Before;
        public decimal? Change
        {
            get
            {
                if (Before?.Price > 0)
                {
                    return Math.Round(this.Price / this.Before.Price * 100 - 100, 2);
                }
                return null;
            }
        }

        public PortfolioEvaluation(DateTime date, decimal unit, decimal price, PortfolioEvaluation before)
        {
            if (before?.Date >= date)
            {
                throw new Exception("Wrong parameters - PortfolioEvaluation constructor.");
            }
            this.Date = date;
            this.Unit = unit;
            this.Price = price;
            this.Before = before;

            if (before != null)
            {
                this.OpenPrice = before.Price;
            }
            else
            {
                this.OpenPrice = price;
            }
            this.ClosePrice = price;
            this.MinPrice = Math.Min(OpenPrice, ClosePrice);
            this.MaxPrice = Math.Max(OpenPrice, ClosePrice);
        }

        public long GetUnixTimeSeconds()
        {
            return new DateTimeOffset(Date, TimeSpan.Zero).ToUnixTimeSeconds();
        }

        public decimal GetOpenPrice()
        {
            return this.OpenPrice;
        }

        public decimal GetClosePrice()
        {
            return this.ClosePrice;
        }

        public decimal GetMinPrice()
        {
            return this.MinPrice;
        }

        public decimal GetMaxPrice()
        {
            return this.MaxPrice;
        }
    }
}
