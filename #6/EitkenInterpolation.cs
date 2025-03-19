using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6
{
    public class EitkenInterpolation
    {
        private Dictionary<double, double> _points;

        public EitkenInterpolation(Dictionary<double, double> dataPoints)
        {
            _points = new Dictionary<double, double>(dataPoints);
        }

        public double Compute(double x)
        {
            int n = _points.Count;
            List<double> xValues = new List<double>(_points.Keys);
            List<double> yValues = new List<double>(_points.Values);

            double[,] p = new double[n, n];

            for (int i = 0; i < n; i++)
            {
                p[i, 0] = yValues[i];
            }

            for (int j = 1; j < n; j++)
            {
                for (int i = 0; i < n - j; i++)
                {
                    p[i, j] = ((x - xValues[i + j]) * p[i, j - 1] - (x - xValues[i]) * p[i + 1, j - 1]) / (xValues[i] - xValues[i + j]);

                    Console.WriteLine($"P[{i},{j}] = (({x} - {xValues[i + j]}) * {p[i, j - 1]:F6} - ({x} - {xValues[i]}) * {p[i + 1, j - 1]:F6}) / ({xValues[i]} - {xValues[i + j]}) = {p[i, j]:F6}");
                }
            }

            return p[0, n - 1];
        }

        public void SolveAndPrint(double x)
        {
            double result = Compute(x);

            Console.WriteLine($"\nРезультат интерполяции Эйткена: P({x}) = {result:F6}");
        }
    }
}
