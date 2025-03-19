using Plotter;

namespace _6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<double, double> dataPoints = new Dictionary<double, double>
            {
                { 1.0, 1.0000 },
                { 2.0, 1.4142 },
                { 3.0, 1.7321 },
                { 4.0, 2.0000 }
            };

            Dictionary<double, double> nDataPoints = new Dictionary<double, double>
            {
                { 1.0, 1.0000 },
                { 1.5, 1.2247 },
                { 2.0, 1.4142 },
                { 2.5, 1.5811 }
            };

            double xTarget = 2.56;
            double nxTarget = 1.69;

            LagrangeInterpolation lagrangeInterpolator = new LagrangeInterpolation(dataPoints);
            EitkenInterpolation eitkenInterpolator = new EitkenInterpolation(dataPoints);
            NewtonForwardInterpolation forwardInterpolation = new NewtonForwardInterpolation(nDataPoints);
            NewtonBackwardInterpolation backwardInterpolation = new NewtonBackwardInterpolation(nDataPoints);

            Console.WriteLine("Лагранж:\n");
            lagrangeInterpolator.SolveAndPrint(xTarget);

            Console.WriteLine("\nЭйткен:\n");
            eitkenInterpolator.SolveAndPrint(xTarget);

            Console.WriteLine($"\nПервая формула Ньютона:\n");
            Console.WriteLine("Таблица конечных разностей:");
            forwardInterpolation.PrintDifferenceTable();
            double resultForward = forwardInterpolation.Compute(nxTarget);
            Console.WriteLine($"\nРезультат первой формулы Ньютона: P({nxTarget}) = {resultForward:F6}");

            Console.WriteLine($"\nВторая формула Ньютона:\n");
            double resultBackward = backwardInterpolation.Compute(nxTarget);
            Console.WriteLine($"\nРезультат второй формулы Ньютона: P({nxTarget}) = {resultBackward:F6}");
        }
    }
}
