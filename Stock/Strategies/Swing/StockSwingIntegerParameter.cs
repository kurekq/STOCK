using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Strategies.Swing
{
    class StockSwingIntegerParameter : StockSwingParameter
    {
        public StockSwingIntegerParameter(int Level) : base(Level)
        {
              
        }
        public override decimal? GetValue(Random r)
        {
            decimal? baseValue = base.GetValue(r);
            if (baseValue == null)
            {
                return null;
            }
            return Math.Round((decimal)baseValue, 0);
        }
    }
}
