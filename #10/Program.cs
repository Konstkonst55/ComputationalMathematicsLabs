﻿using Shared;

namespace _10
{
    internal class Program
    {
        static Func<double, double> function = x => Math.Sin(x);
        static List<Func<double, double>> basis = new()
        {
            x => 1,
            x => x,
            Math.Sqrt
        };

        const double xMin = 0.0, xStep = 1;
        const int xCount = 4;

        static List<(double x, double y)> dataPoints = ListUtils.FillDataPoints(function, xMin, xStep, xCount);

        private static void Main(string[] args)
        {
            Printer.PrintListAsTable(dataPoints, "Исходные данные");

            Approximation approximation = new LeastSquaresApproximator(dataPoints, basis);

            var approximatedFunction = approximation.Compute();

            PlotterForms.Program.ShowGraph(GraphGenerator.GenerateApproximationData(function, approximatedFunction, xMin, xStep, xCount));
        }
    }
}
