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
                new StockSwingRangeParameter(1) { ValueFrom = null, ValueTo = 0.0949m },
                new StockSwingRangeParameter(2) { ValueFrom = 0.095m, ValueTo = 0.1879m },
                new StockSwingRangeParameter(3) { ValueFrom = 0.188m, ValueTo = 0.2767m },
                new StockSwingRangeParameter(4) { ValueFrom = 0.2768m, ValueTo = 0.5477m },
                new StockSwingRangeParameter(5) { ValueFrom = 0.5478m, ValueTo = 0.8024m },
                new StockSwingRangeParameter(6) { ValueFrom = 0.8025m, ValueTo = 1.2564m },
                new StockSwingRangeParameter(7) { ValueFrom = 1.2565m, ValueTo = null });
        }

        //Poziomy wielkości: 10mln, 50mln, 150mln, 400mln, 1000mln, 5000mln
        public StockSwingParameterLevels GetStockCapParams()
        {
            return new StockSwingParameterLevels(
                new StockSwingRangeParameter(1) { ValueFrom = null, ValueTo = 50m },
                new StockSwingRangeParameter(2) { ValueFrom = 50.0001m, ValueTo = 150m },
                new StockSwingRangeParameter(3) { ValueFrom = 150.0001m, ValueTo = 250m },
                new StockSwingRangeParameter(4) { ValueFrom = 250.0001m, ValueTo = 500m },
                new StockSwingRangeParameter(5) { ValueFrom = 500.0001m, ValueTo = 1000m },
                new StockSwingRangeParameter(6) { ValueFrom = 1000.0001m, ValueTo = 5000m },
                new StockSwingRangeParameter(7) { ValueFrom = 5000.0001m, ValueTo = null });
        }

        //Ilość spółek: 2, 4, 7, 11, 16, 22
        public StockSwingParameterLevels GetStockCountParams()
        {
            return new StockSwingParameterLevels(
                new StockSwingIntegerParameter(1) { ValueFrom = null, ValueTo = 2m },
                new StockSwingIntegerParameter(2) { ValueFrom = 3m, ValueTo = 4m },
                new StockSwingIntegerParameter(3) { ValueFrom = 5m, ValueTo = 7m },
                new StockSwingIntegerParameter(4) { ValueFrom = 8m, ValueTo = 11m },
                new StockSwingIntegerParameter(5) { ValueFrom = 12m, ValueTo = 16m },
                new StockSwingIntegerParameter(6) { ValueFrom = 17m, ValueTo = 22m },
                new StockSwingIntegerParameter(7) { ValueFrom = 23m, ValueTo = 100m });
        }

        //% swingu góra: 2%, 4%, 7%, 12%, 18%, 25%
        public StockSwingParameterLevels GetSwingDifferencesUpParams()
        {
            return new StockSwingParameterLevels(
                new StockSwingParameter(1) { ValueFrom = 2m, ValueTo = 2m },
                new StockSwingParameter(2) { ValueFrom = 2.01m, ValueTo = 4m },
                new StockSwingParameter(3) { ValueFrom = 4.01m, ValueTo = 7m },
                new StockSwingParameter(4) { ValueFrom = 7.01m, ValueTo = 12m },
                new StockSwingParameter(5) { ValueFrom = 12.01m, ValueTo = 18m },
                new StockSwingParameter(6) { ValueFrom = 18.01m, ValueTo = 25m },
                new StockSwingParameter(7) { ValueFrom = 25.01m, ValueTo = 50m });
        }

        //% swingu dół: 2%, 4%, 7%, 12%, 18%, 25%
        public StockSwingParameterLevels GetSwingDifferencesDownParams()
        {
            return GetSwingDifferencesUpParams();
        }

        //rezygnacja z inwestycje, gdy spadek wynosi: nigdy, 10%, 20%, 30%, 40%, 50%
        public StockSwingParameterLevels GetResignPercentageParams()
        {
            return new StockSwingParameterLevels(
                new StockSwingIntegerParameter(1) { ValueFrom = null, ValueTo = null },
                new StockSwingIntegerParameter(2) { ValueFrom = 10m, ValueTo = 10m },
                new StockSwingIntegerParameter(3) { ValueFrom = 20m, ValueTo = 20m },
                new StockSwingIntegerParameter(4) { ValueFrom = 30m, ValueTo = 30m },
                new StockSwingIntegerParameter(5) { ValueFrom = 40m, ValueTo = 40m },
                new StockSwingIntegerParameter(6) { ValueFrom = 50m, ValueTo = 50m },
                new StockSwingIntegerParameter(7) { ValueFrom = 75m, ValueTo = 75m });
        }

        //sprzedajemy spółkę, gdy czekamy dłużej niż: nigdy, 3 miesiące, 6 miesięcy, 12 miesięcy, 18 miesięcy, 30 miesięcy
        public StockSwingParameterLevels GetResignMonthTimeParams()
        {
            return new StockSwingParameterLevels(
                new StockSwingIntegerParameter(1) { ValueFrom = null, ValueTo = null },
                new StockSwingIntegerParameter(2) { ValueFrom = 3m, ValueTo = 3m },
                new StockSwingIntegerParameter(3) { ValueFrom = 6m, ValueTo = 6m },
                new StockSwingIntegerParameter(4) { ValueFrom = 12m, ValueTo = 12m },
                new StockSwingIntegerParameter(5) { ValueFrom = 18m, ValueTo = 18m },
                new StockSwingIntegerParameter(6) { ValueFrom = 30m, ValueTo = 30m },
                new StockSwingIntegerParameter(7) { ValueFrom = 50m, ValueTo = 50m });
        }
    }
}
