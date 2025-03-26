using Plotter;
using ScottPlot;
using System.Drawing;

namespace _6
{
    internal class Program
    {
        static Dictionary<double, double> dataPoints = new Dictionary<double, double>
        {
            { 1.0, 1.0000 },
            { 2.0, 1.4142 },
            { 3.0, 1.7321 },
            { 4.0, 2.0000 }
        };

        static double lxTarget = 2.56;
        static double exTarget = 1.75;
        static double nfxTarget = 1.15;
        static double nbxTarget = 1.69;

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
            lagrangeInterpolator = new LagrangeInterpolation(dataPoints);
            eitkenInterpolator = new EitkenInterpolation(dataPoints);
            forwardInterpolation = new NewtonForwardInterpolation(dataPoints);
            backwardInterpolation = new NewtonBackwardInterpolation(dataPoints);

            Console.WriteLine("Лагранж:\n");
            resultLagrange = lagrangeInterpolator.Compute(lxTarget);
            Console.WriteLine($"P({lxTarget}) = {resultLagrange:F6}");

            Console.WriteLine("\nЭйткен:\n");
            resultEitken = eitkenInterpolator.Compute(exTarget);
            Console.WriteLine($"P({exTarget}) = {resultEitken:F6}");

            Console.WriteLine("\nПервая формула Ньютона:\n");
            Console.WriteLine("Таблица конечных разностей:");
            forwardInterpolation.PrintDifferenceTable();
            resultForward = forwardInterpolation.Compute(nfxTarget);
            Console.WriteLine($"\nРезультат первой формулы Ньютона: P({nfxTarget}) = {resultForward:F6}");

            Console.WriteLine("\nВторая формула Ньютона:\n");
            resultBackward = backwardInterpolation.Compute(nbxTarget);
            Console.WriteLine($"\nРезультат второй формулы Ньютона: P({nbxTarget}) = {resultBackward:F6}");

            GenerateGraph();
        }

        static void GenerateGraph()
        {
            GraphPlotter plotter = new GraphPlotter(new Size(800, 600));
            plotter.SetTitle("Интерполяция: Лагранж, Эйткен, Ньютон");
            plotter.SetLabels("X", "Y");

            List<double> xData = [.. dataPoints.Keys];
            List<double> yData = [.. dataPoints.Values];

            plotter.AddData(xData, yData, "Исходные точки", scatter: true);

            List<double> xValues = new List<double>();

            for (double x = 0; x <= 6.0; x += 0.2)
            {
                xValues.Add(x);
            }

            List<double> yLagrange = xValues.Select(x => lagrangeInterpolator.Compute(x, false)).ToList();

            plotter.AddData(xValues, yLagrange, "Лагранж", scatter: true);

            plotter.AddPoint(lxTarget, resultLagrange, "P(lxTarget)", size: 15);
            plotter.AddPoint(exTarget, resultEitken, "P(exTarget)", size: 15);
            plotter.AddPoint(nfxTarget, resultForward, "P(nfxTarget) (Forward)", size: 15);
            plotter.AddPoint(nbxTarget, resultBackward, "P(nbxTarget) (Backward)", size: 15);

            plotter.Save("C:\\Users\\Admin\\Desktop\\interpolation_graph.png");
        }
    }
}
