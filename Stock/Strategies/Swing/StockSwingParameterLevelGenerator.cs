using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Strategies.Swing
{
    public class StockSwingParameterLevelGenerator
    {
        private Random random;
        public StockSwingParameterLevelGenerator()
        {
            random = new Random();
        }

        private int GetLvl(StockSwingParameter father, StockSwingParameter mother)
        {
            if (father == null && mother == null)
            {
                return 1 + random.Next(7);
            }
            return 0;
        }
        public int GetVotalityParamLvl(StockSwingParameter father, StockSwingParameter mother)
        {
            return GetLvl(father, mother);
        }
        public int GetStockCapParamLvl(StockSwingParameter father, StockSwingParameter mother)
        {
            return GetLvl(father, mother);
        }
        public int GetStockCountParamLvl(StockSwingParameter father, StockSwingParameter mother)
        {
            return GetLvl(father, mother);
        }
        public int GetSwingDifferencesUpParamLvl(StockSwingParameter father, StockSwingParameter mother)
        {
            return GetLvl(father, mother);
        }
        public int GetSwingDifferencesDownParamLvl(StockSwingParameter father, StockSwingParameter mother)
        {
            return GetLvl(father, mother);
        }
        public int GetResignPercentageParamLvl(StockSwingParameter father, StockSwingParameter mother)
        {
            return GetLvl(father, mother);
        }
        public int GetResignMonthTimeParamLvl(StockSwingParameter father, StockSwingParameter mother)
        {
            return GetLvl(father, mother);
        }
    }
}
