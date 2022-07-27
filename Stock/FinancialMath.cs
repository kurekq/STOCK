using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock
{
    public class FinancialMath
    {
        public static double StandardDeviation(IEnumerable<decimal> sequence)
        {
            double result = 0;

            if (sequence.Any())
            {
                double average = Decimal.ToDouble(sequence.Average());
                double sum = sequence.Sum(d => Math.Pow(Decimal.ToDouble(d) - average, 2));
                result = Math.Sqrt(sum / sequence.Count());
            }
            return Math.Round(result, 4);
        }
    }
}
