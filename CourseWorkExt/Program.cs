using CourseWorkExt;

internal class Program
{
    private static void Main(string[] args)
    {
        bool useRk4 = true;
        Func<double, double, double, (double, double)> function = (x, y, dy) => (dy, (Math.Exp(x) + y + dy) / 3.0);

        var shootingSolver = new ShootingMethodSolver(function, useRk4);

        shootingSolver.PrintProblemInfo();
        shootingSolver.PrintMethodInfo(useRk4);

        double k0 = shootingSolver.FindInitialSlope(-10.0, 10.0, 1e-12, useRk4 ? 1e-4 : 1e-6);
        Console.WriteLine($"Найденное y'(0) (k0) = {k0:F14}\n");

        Console.WriteLine($"Подбор оптимального шага ({(useRk4 ? "Рунге-Кутта 4-го порядка" : "Рунге-Кутта 2-го порядка")})");

        double optimalH = shootingSolver.AdjustStep(1e-12, 0.1, 1.0, k0);
        Console.WriteLine($"Оптимальный шаг: {optimalH:E}\n");

        var result = shootingSolver.Compute(0, 1, optimalH, 1.0, k0);
        shootingSolver.PrintFinalResults(result, useRk4);
    }
}