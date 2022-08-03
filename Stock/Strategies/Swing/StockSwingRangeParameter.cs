using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Strategies.Swing
{
    class StockSwingRangeParameter : StockSwingParameter
    {
        public StockSwingRangeParameter(int Level) : base(Level)
        {

        }
        public override decimal? GetValue(Random r)
        {
            throw new Exception("This method has no sense for Range parameter");
        }
    }
}
