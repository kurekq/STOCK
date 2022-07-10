using System;
using System.Collections.Generic;
using System.Text;

namespace Stock
{
    public class ArchiveListinings : Listinings
    {
        [DatabaseField]
        public string ISIN;

        [DatabaseField]
        public decimal TradingVolume;

        public Int64 ShareAmount;
        public decimal OpenCap
        {
            get
            {
                return ShareAmount * OpenPrice;
            }
        }
        public decimal MaxCap
        {
            get
            {
                return ShareAmount * MaxPrice;
            }
        }
        public decimal MinCap
        {
            get
            {
                return ShareAmount * MinPrice;
            }
        }
        public decimal CloseCap
        {
            get
            {
                return ShareAmount * ClosePrice;
            }
        }
    }
}
