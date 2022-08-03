using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Strategies.Swing
{
    public class StockSwingParameterLevels
    {
        public StockSwingParameter[] Parameters = new StockSwingParameter[7];

        public StockSwingParameterLevels(StockSwingParameter lvl1,
            StockSwingParameter lvl2,
            StockSwingParameter lvl3,
            StockSwingParameter lvl4,
            StockSwingParameter lvl5,
            StockSwingParameter lvl6,
            StockSwingParameter lvl7)
        {
            Parameters[0] = lvl1;
            Parameters[1] = lvl2;
            Parameters[2] = lvl3;
            Parameters[3] = lvl4;
            Parameters[4] = lvl5;
            Parameters[5] = lvl6;
            Parameters[6] = lvl7;
        }

        public decimal? GetValue(Random r, int lvl)
        {
            return Parameters[lvl - 1].GetValue(r);
        }
    }
}
