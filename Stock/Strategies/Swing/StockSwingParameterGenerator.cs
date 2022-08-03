using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Strategies.Swing
{
    public class StockSwingParameterGenerator
    {
        //Poziomy zmienności: 0.0949, 0.1879, 0.2767, 0.5477, 0.8024, 1.2564
        public StockSwingParameterLevels GetVotalityParams()
        {
            return new StockSwingParameterLevels(
                new StockSwingParameter() { ValueFrom = null, ValueTo = 0.0949m },
                new StockSwingParameter() { ValueFrom = 0.095m, ValueTo = 0.1879m },
                new StockSwingParameter() { ValueFrom = 0.188m, ValueTo = 0.2767m },
                new StockSwingParameter() { ValueFrom = 0.2768m, ValueTo = 0.5477m },
                new StockSwingParameter() { ValueFrom = 0.5478m, ValueTo = 0.8024m },
                new StockSwingParameter() { ValueFrom = 0.8025m, ValueTo = 1.2564m },
                new StockSwingParameter() { ValueFrom = 1.2565m, ValueTo = null });
        }

        //Poziomy wielkości: 10mln, 50mln, 150mln, 400mln, 1000mln, 5000mln
        public StockSwingParameterLevels GetStockCapParams()
        {
            return new StockSwingParameterLevels(
                new StockSwingParameter() { ValueFrom = null, ValueTo = 50m },
                new StockSwingParameter() { ValueFrom = 50.0001m, ValueTo = 150m },
                new StockSwingParameter() { ValueFrom = 150.0001m, ValueTo = 250m },
                new StockSwingParameter() { ValueFrom = 250.0001m, ValueTo = 500m },
                new StockSwingParameter() { ValueFrom = 500.0001m, ValueTo = 1000m },
                new StockSwingParameter() { ValueFrom = 1000.0001m, ValueTo = 5000m },
                new StockSwingParameter() { ValueFrom = 5000.0001m, ValueTo = null });
        }

        //Ilość spółek: 2, 4, 7, 11, 16, 22
        public StockSwingParameterLevels GetStockCountParams()
        {
            return new StockSwingParameterLevels(
                new StockSwingParameter() { ValueFrom = null, ValueTo = 2m },
                new StockSwingParameter() { ValueFrom = 3m, ValueTo = 4m },
                new StockSwingParameter() { ValueFrom = 5m, ValueTo = 7m },
                new StockSwingParameter() { ValueFrom = 8m, ValueTo = 11m },
                new StockSwingParameter() { ValueFrom = 12m, ValueTo = 16m },
                new StockSwingParameter() { ValueFrom = 17m, ValueTo = 22m },
                new StockSwingParameter() { ValueFrom = 23m, ValueTo = null });
        }

        //% swingu góra: 2%, 4%, 7%, 12%, 18%, 25%
        public StockSwingParameterLevels GetSwingDifferencesUpParams()
        {
            return null;
        }

        //% swingu dół: 2%, 4%, 7%, 12%, 18%, 25%
        public StockSwingParameterLevels GetSwingDifferencesDownParams()
        {
            return null;
        }

        //rezygnacja z inwestycje, gdy spadek wynosi: nigdy, 10%, 20%, 30%, 40%, 50%
        public StockSwingParameterLevels GetResignPercentageParams()
        {
            return null;
        }

        //sprzedajemy spółkę, gdy czekamy dłużej niż: nigdy, 3 miesiące, 6 miesięcy, 12 miesięcy, 18 miesięcy, 30 miesięcy
        public StockSwingParameterLevels GetResignMonthTimeParams()
        {
            return null;
        }
    }
}
