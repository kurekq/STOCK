using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Strategies.Swing
{
    public class StockSwingParametersPackageGenerator
    {
        private StockSwingParametersPackage Father;
        private StockSwingParametersPackage Mother;
        public StockSwingParametersPackageGenerator(StockSwingParametersPackage father, StockSwingParametersPackage mother)
        {
            this.Father = father;
            this.Mother = mother;
        }

        public StockSwingParametersPackage GetPackage()
        {
            StockSwingParametersPackage package = new StockSwingParametersPackage();
            StockSwingParameterLevelGenerator lvlGenerator = new StockSwingParameterLevelGenerator();
            StockSwingParameterGenerator paramsGenerator = new StockSwingParameterGenerator();

            package.ResignPercentage = paramsGenerator.GetResignPercentageParams().Parameters[lvlGenerator.GetResignPercentageParamLvl(Father?.ResignPercentage, Mother?.ResignPercentage) - 1];
            package.ResignTimeInMonth = paramsGenerator.GetResignMonthTimeParams().Parameters[lvlGenerator.GetResignMonthTimeParamLvl(Father?.ResignTimeInMonth, Mother?.ResignTimeInMonth) - 1];
            package.StockCap = paramsGenerator.GetStockCapParams().Parameters[lvlGenerator.GetStockCapParamLvl(Father?.StockCap, Mother?.StockCap) - 1];
            package.StockCount = paramsGenerator.GetStockCountParams().Parameters[lvlGenerator.GetStockCountParamLvl(Father?.StockCount, Mother?.StockCount) - 1];
            package.SwingDifferDown = paramsGenerator.GetSwingDifferencesDownParams().Parameters[lvlGenerator.GetSwingDifferencesDownParamLvl(Father?.SwingDifferDown, Mother?.SwingDifferDown) - 1];
            package.SwingDifferUp = paramsGenerator.GetSwingDifferencesUpParams().Parameters[lvlGenerator.GetSwingDifferencesUpParamLvl(Father?.SwingDifferUp, Mother?.SwingDifferUp) - 1];
            package.Votality = paramsGenerator.GetVotalityParams().Parameters[lvlGenerator.GetVotalityParamLvl(Father?.Votality, Mother?.Votality) - 1];
            return new StockSwingParametersPackage();
        }
    }
}
