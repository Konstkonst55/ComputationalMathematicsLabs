using static SkiaSharp.HarfBuzz.SKShaper;

namespace _6
{
    public class LagrangeInterpolation
    {
        private Dictionary<double, double> _points;

        public LagrangeInterpolation(Dictionary<double, double> dataPoints)
        {
            _points = new Dictionary<double, double>(dataPoints);
        }

        public double Compute(double x, bool useWrite = true)
        {
            double result = 0;
            var xValues = _points.Keys.ToList();
            var yValues = _points.Values.ToList();
            int n = xValues.Count;

            for (int i = 0; i < n; i++)
            {
                double term = yValues[i];

                for (int j = 0; j < n; j++)
                {
                    if (i != j)
                    {
                        term *= (x - xValues[j]) / (xValues[i] - xValues[j]);
                        if (useWrite) Console.Write($"(x - {xValues[j]}) / ({xValues[i]} - {xValues[j]}) * ");
                    }
                }

                result += term;
                if (useWrite) Console.WriteLine($"{yValues[i]} -> {term}");
            }

            return result;
        }

        public double ComputeTruncationError(double x)
        {
            double maxDerivative = 15.0 / 16;
            double factorial = 24.0;
            double product = 1;

            foreach (var xi in _points.Keys)
            {
                product *= (x - xi);
            }

            return Math.Abs((maxDerivative / factorial) * product);
        }

        public void SolveAndPrint(double x)
        {
            double interpolatedValue = Compute(x);
            double truncationError = ComputeTruncationError(x);
            double roundError = 5e-5;
            double realError = truncationError + roundError;

            Console.WriteLine();
            Console.WriteLine($"Значение в x = {x}: {interpolatedValue:F4}");
            Console.WriteLine($"Погрешность усечения: {truncationError:F6}");
            Console.WriteLine($"Погрешность округления: {roundError:F6}");
            Console.WriteLine($"Реальная погрешность: {realError:F6}");
        }
    }
}
