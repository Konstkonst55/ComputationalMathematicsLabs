using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6
{
    public class NewtonInterpolation
    {
        protected Dictionary<double, double> _points;
        protected double[,] _differenceTable;

        public NewtonInterpolation(Dictionary<double, double> dataPoints)
        {
            _points = new Dictionary<double, double>(dataPoints);
            CalculateDifferenceTable();
        }

        private void CalculateDifferenceTable()
        {
            int n = _points.Count;
            _differenceTable = new double[n, n];
            List<double> yValues = new List<double>(_points.Values);

            for (int i = 0; i < n; i++)
            {
                _differenceTable[i, 0] = yValues[i];
            }

            for (int j = 1; j < n; j++)
            {
                for (int i = 0; i < n - j; i++)
                {
                    _differenceTable[i, j] = _differenceTable[i + 1, j - 1] - _differenceTable[i, j - 1];
                }
            }
        }

        protected static double Factorial(int n)
        {
            if (n == 0)
            {
                return 1;
            }

            double result = 1;

            for (int i = 2; i <= n; i++)
            {
                result *= i;
            }

            return result;
        }

        public void PrintDifferenceTable()
        {
            int n = _points.Count;
            List<double> xValues = new List<double>(_points.Keys);

            int columnWidth = Math.Max("x".Length, "y".Length);

            for (int i = 0; i < n; i++)
            {
                columnWidth = Math.Max(columnWidth, xValues[i].ToString("F6").Length);
            }

            for (int i = 0; i < n; i++)
            {
                columnWidth = Math.Max(columnWidth, _differenceTable[i, 0].ToString("F6").Length);
            }

            string header = "x".PadRight(columnWidth) + "\t" + "y".PadRight(columnWidth);

            for (int i = 1; i < n; i++)
            {
                header += $"\t{($"d{(i == 1 ? "" : i)}y").PadRight(columnWidth)}";
            }

            Console.WriteLine(header);

            for (int i = 0; i < n; i++)
            {
                Console.Write($"{xValues[i].ToString("F6").PadRight(columnWidth)}\t");

                for (int j = 0; j < n - i; j++)
                {
                    Console.Write($"{_differenceTable[i, j]:F6}".PadRight(columnWidth) + "\t");
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

    }
}
