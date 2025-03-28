using Plotter;
using System.Drawing;

namespace _6
{
    internal class Program
    {
        static Func<double, double> function = Math.Sqrt;

        static Dictionary<double, double> dataPoints = FillDataPoints();

        const double xTarget = 2.56, step = 1.0, xMin = 1.0, xMax = 4.0;

        static LagrangeInterpolation lagrangeInterpolator;
        static EitkenInterpolation eitkenInterpolator;
        static NewtonForwardInterpolation forwardInterpolation;
        static NewtonBackwardInterpolation backwardInterpolation;

        static double resultLagrange;
        static double resultEitken;
        static double resultForward;
        static double resultBackward;

        static void Main()
        {
            PrintDictionaryAsTable(dataPoints, "Исходная функция:");

            lagrangeInterpolator = new LagrangeInterpolation(dataPoints);
            eitkenInterpolator = new EitkenInterpolation(dataPoints);
            forwardInterpolation = new NewtonForwardInterpolation(dataPoints);
            backwardInterpolation = new NewtonBackwardInterpolation(dataPoints);

            Console.WriteLine("Лагранж:\n");
            resultLagrange = lagrangeInterpolator.Compute(xTarget);
            Console.WriteLine($"P({xTarget}) = {resultLagrange:F6}");

            Console.WriteLine("\nЭйткен:\n");
            resultEitken = eitkenInterpolator.Compute(xTarget);
            Console.WriteLine($"P({xTarget}) = {resultEitken:F6}");

            Console.WriteLine("\nПервая формула Ньютона:\n");
            Console.WriteLine("Таблица конечных разностей:");
            forwardInterpolation.PrintDifferenceTable();
            resultForward = forwardInterpolation.Compute(xTarget);
            Console.WriteLine($"\nРезультат первой формулы Ньютона: P({xTarget}) = {resultForward:F6}");

            Console.WriteLine("\nВторая формула Ньютона:\n");
            resultBackward = backwardInterpolation.Compute(xTarget);
            Console.WriteLine($"\nРезультат второй формулы Ньютона: P({xTarget}) = {resultBackward:F6}");

            GenerateGraph();
        }

        static Dictionary<double, double> FillDataPoints() => Enumerable.Range(0, (int)((xMax - xMin) / step) + 1).Select(i => xMin + i * step).ToDictionary(x => x, x => function(x));

        static void GenerateGraph()
        {
            var plotter = new GraphPlotter(new Size(800, 600));
            plotter.SetTitle("Интерполяция: Лагранж, Эйткен, Ньютон");
            plotter.SetLabels("X", "Y");

            var xData = dataPoints.Keys.ToList();
            var yData = dataPoints.Values.ToList();
            var xValues = new List<double>();

            for (double x = 0; x <= 6.0; x += 0.2)
            {
                xValues.Add(x);
            }

            var yLagrange = xValues.Select(x => lagrangeInterpolator.Compute(x, false)).ToList();

            plotter.AddData(xValues, yLagrange, "Лагранж", scatter: true);
            plotter.AddData(xData, yData, "Исходные точки", scatter: true);
            plotter.AddPoint(xTarget, resultLagrange, "P(xTarget)");

            plotter.Save("C:\\Users\\Admin\\Desktop\\interpolation_graph.png");
        }

        static void PrintDictionaryAsTable(Dictionary<double, double> dict, string title = "")
        {
            if (title.Length > 0)
            {
                Console.WriteLine(title + "\n");
            }

            Console.WriteLine("{0,-15} {1,-15}", "X", "Y");  
            Console.WriteLine(new string('-', 30)); 
  
            foreach (var kvp in dict)
            {
                Console.WriteLine("{0,-15:F2} {1,-15:F6}", kvp.Key, kvp.Value);
            }

            Console.WriteLine();
        }
    }
}
