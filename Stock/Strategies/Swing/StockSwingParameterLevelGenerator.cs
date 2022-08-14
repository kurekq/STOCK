using System;
using System.Collections.Generic;
using System.Linq;
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

        public int GetLvl(StockSwingParameter father, StockSwingParameter mother)
        {
            if (father == null && mother == null)
            {
                return 1 + random.Next(7);
            }
            else
            {
                int lvl1 = father.Level;
                int lvl2 = mother.Level;

                decimal[] divs = new decimal[7];
                divs[0] = 1m / (Math.Abs(lvl1 - 1) + Math.Abs(lvl2 - 1) + 1);
                divs[1] = 1m / (Math.Abs(lvl1 - 2) + Math.Abs(lvl2 - 2) + 1);
                divs[2] = 1m / (Math.Abs(lvl1 - 3) + Math.Abs(lvl2 - 3) + 1);
                divs[3] = 1m / (Math.Abs(lvl1 - 4) + Math.Abs(lvl2 - 4) + 1);
                divs[4] = 1m / (Math.Abs(lvl1 - 5) + Math.Abs(lvl2 - 5) + 1);
                divs[5] = 1m / (Math.Abs(lvl1 - 6) + Math.Abs(lvl2 - 6) + 1);
                divs[6] = 1m / (Math.Abs(lvl1 - 7) + Math.Abs(lvl2 - 7) + 1);

                decimal overalDiffs = divs.Sum();

                decimal randomInOveralDiff = overalDiffs * Convert.ToDecimal(random.NextDouble());

                int level = 0;
                decimal tempDiffsSum = 0;
                for (int i = 0; i < 7; i++)
                {
                    tempDiffsSum += divs[i];
                    if (tempDiffsSum > randomInOveralDiff)
                    {
                        level = ++i;
                        break;
                    }
                }
                
                return level;
            }
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
