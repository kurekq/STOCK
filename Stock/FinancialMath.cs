using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock
{
    public class FinancialMath
    {
        public static double CalculateVolatility(IEnumerable<decimal> sequence)
        {
            double result = 0;

            if (sequence.Any())
            {
                decimal average = sequence.Average();
                double variance = sequence.Sum(s => Math.Pow(Decimal.ToDouble(s - average), 2)) / sequence.Count();
                result = Math.Sqrt(variance) / Decimal.ToDouble(average);
            }
            return Math.Round(result, 4);
        }
    }
}
