using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Strategies.Swing
{
    public class StockSwingParameterLevels
    {
        public StockSwingParameter[] parameters = new StockSwingParameter[7];

        public StockSwingParameterLevels(StockSwingParameter lvl1,
            StockSwingParameter lvl2,
            StockSwingParameter lvl3,
            StockSwingParameter lvl4,
            StockSwingParameter lvl5,
            StockSwingParameter lvl6,
            StockSwingParameter lvl7)
        {
            parameters[0] = lvl1;
            parameters[1] = lvl2;
            parameters[2] = lvl3;
            parameters[3] = lvl4;
            parameters[4] = lvl5;
            parameters[5] = lvl6;
            parameters[6] = lvl7;
        }
    }
}
