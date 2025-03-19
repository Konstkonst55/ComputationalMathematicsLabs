using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6
{
    public class NewtonForwardInterpolation : NewtonInterpolation
    {
        public NewtonForwardInterpolation(Dictionary<double, double> dataPoints) : base(dataPoints) { }

        public double Compute(double x)
        {
            List<double> xValues = new List<double>(points.Keys);
            double h = xValues[1] - xValues[0];
            double q = (x - xValues[0]) / h;

            Console.WriteLine($"q = ({x} - {xValues[0]}) / {h} = {q:F6}");

            double result = differenceTable[0, 0];
            double term = 1;

            for (int i = 1; i < points.Count; i++)
            {
                term *= (q - (i - 1)) / i;
                double addTerm = differenceTable[0, i] * term;
                result += addTerm;

                Console.WriteLine($"Шаг {i}: {differenceTable[0, i]:F6} / {Factorial(i)} * {term:F6} = {addTerm:F6}");
            }

            return result;
        }
    }

}
