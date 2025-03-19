using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6
{
    public class NewtonBackwardInterpolation : NewtonInterpolation
    {
        public NewtonBackwardInterpolation(Dictionary<double, double> dataPoints) : base(dataPoints) { }

        public double Compute(double x, bool useWrite = true)
        {
            List<double> xValues = new List<double>(points.Keys);
            int n = xValues.Count;
            double h = xValues[1] - xValues[0];
            double q = (x - xValues[n - 1]) / h;

            if (useWrite) Console.WriteLine($"q = ({x} - {xValues[n - 1]}) / {h} = {q:F6}");

            double result = differenceTable[n - 1, 0];
            double term = 1;

            for (int i = 1; i < n; i++)
            {
                term *= (q + (i - 1)) / i;
                double addTerm = differenceTable[n - i - 1, i] * term;
                result += addTerm;

                if (useWrite) Console.WriteLine($"Шаг {i}: {differenceTable[n - i - 1, i]:F6} / {Factorial(i)} * {term:F6} = {addTerm:F6}");
            }

            return result;
        }
    }
}
