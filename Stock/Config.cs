using System;
using System.Collections.Generic;
using System.Text;

namespace Stock
{
    public static class Config
    {
        public static decimal MinimumCommisionPLN = 5;
        public static decimal MinimumCommisionNotPLN = 19;
        public static decimal CommisionPercentagePLN = 0.29m;
        public static decimal CommisionPercentageNotPLN = 0.39m;
        public static decimal CapitalGainsTaxValue = 0.19m;
    }
}
