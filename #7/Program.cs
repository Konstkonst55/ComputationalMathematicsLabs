using Shared;

namespace _7
{
    internal class Program
    {
        static Func<double, double> function = x => 1 / x;

        const double xMin = 0.5, xStep = 3, xTarget = 2.56;
        const int xCount = 5;

        static List<(double x, double y)> dataPoints = ListUtils.FillDataPoints(function, xMin, xStep, xCount);

        static CubicSplineInterpolation csInterpolator = new(dataPoints);

        [STAThread]
        static void Main()
        {
            csInterpolator.Compute(xTarget);

            PlotterForms.Program.ShowGraph(GraphGenerator.GenerateData(csInterpolator, function, xMin));
        }
    }
}