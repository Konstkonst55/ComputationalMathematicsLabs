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

        static Dictionary<double, double> nDataPoints = new Dictionary<double, double>
        {
            { 1.0, 1.0000 },
            { 1.5, 1.2247 },
            { 2.0, 1.4142 },
            { 2.5, 1.5811 }
        };

        static double xTarget = 2.56;
        static double nxTarget = 1.69;

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
            resultLagrange = lagrangeInterpolator.Compute(xTarget);
            Console.WriteLine($"P({xTarget}) = {resultLagrange:F6}");

            Console.WriteLine("\nЭйткен:\n");
            resultEitken = eitkenInterpolator.Compute(xTarget);
            Console.WriteLine($"P({xTarget}) = {resultEitken:F6}");

            Console.WriteLine("\nПервая формула Ньютона:\n");
            Console.WriteLine("Таблица конечных разностей:");
            forwardInterpolation.PrintDifferenceTable();
            resultForward = forwardInterpolation.Compute(nxTarget);
            Console.WriteLine($"\nРезультат первой формулы Ньютона: P({nxTarget}) = {resultForward:F6}");

            Console.WriteLine("\nВторая формула Ньютона:\n");
            resultBackward = backwardInterpolation.Compute(nxTarget);
            Console.WriteLine($"\nРезультат второй формулы Ньютона: P({nxTarget}) = {resultBackward:F6}");

            GenerateGraph();
        }

        static void GenerateGraph()
        {
            GraphPlotter plotter = new GraphPlotter(new Size(800, 600));
            plotter.SetTitle("Интерполяция: Лагранж, Эйткен, Ньютон");
            plotter.SetLabels("X", "Y");

            List<double> xData = new List<double>(dataPoints.Keys);
            List<double> yData = new List<double>(dataPoints.Values);
            plotter.AddData(xData, yData, "Исходные точки", scatter: true);

            List<double> xInterpolated = new List<double>();
            List<double> yLagrange = new List<double>();

            for (double x = 1.0; x <= 4.0; x += 0.1)
            {
                xInterpolated.Add(x);
                yLagrange.Add(lagrangeInterpolator.Compute(x, false));
            }

            plotter.AddData(xInterpolated, yLagrange, "Лагранж");

            List<double> yEitken = new List<double>();

            foreach (double x in xInterpolated)
            {
                yEitken.Add(eitkenInterpolator.Compute(x, false));
            }

            plotter.AddData(xInterpolated, yEitken, "Эйткен");

            List<double> nxInterpolated = new List<double>();
            List<double> yNewtonForward = new List<double>();

            for (double x = 1.0; x <= 2.5; x += 0.1)
            {
                nxInterpolated.Add(x);
                yNewtonForward.Add(forwardInterpolation.Compute(x, false));
            }

            plotter.AddData(nxInterpolated, yNewtonForward, "Ньютон (первая формула)");

            List<double> yNewtonBackward = new List<double>();

            foreach (double x in nxInterpolated)
            {
                yNewtonBackward.Add(backwardInterpolation.Compute(x, false));
            }

            plotter.AddData(nxInterpolated, yNewtonBackward, "Ньютон (вторая формула)");

            plotter.AddPoint(xTarget, resultLagrange, "P(xTarget)", size: 15);
            plotter.AddPoint(nxTarget, resultForward, "P(nxTarget) (Forward)", size: 15);
            plotter.AddPoint(nxTarget, resultBackward, "P(nxTarget) (Backward)", size: 15);

            plotter.Save("C:/Users/Acer/Desktop/interpolation_graph.png");
        }
    }
}
