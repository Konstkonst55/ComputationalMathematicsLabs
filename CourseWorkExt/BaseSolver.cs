namespace CourseWorkExt
{
    public abstract class BaseSolver
    {
        protected const double Epsilon = 1e-12;
        protected const double InitialStep = 0.1;
        protected const double A = 0.0;
        protected const double B = 1.0;
        protected const double Ya = 1.0;
        protected static readonly double Yb = Math.E;

        public abstract (double Y, double DY) Compute(double a, double b, double h, double y0, double dy0, bool useWrite = true);

        public void PrintProblemInfo()
        {
            Console.WriteLine("Краевая задача");
            Console.WriteLine("Уравнение: y'' = (exp(x) + y + y') / 3");
            Console.WriteLine($"Аналитическое решение: y(x) = exp(x), y'(x) = exp(x)");
            Console.WriteLine($"Граничные условия: y({A}) = {Ya}, y({B}) = {Yb:F14}");
            Console.WriteLine($"y'({A}) (аналитическое) = {Math.Exp(A):F14}");
            Console.WriteLine($"y'({B}) (аналитическое) = {Math.Exp(B):F14}");
            Console.WriteLine($"Требуемая точность для y(B): |y(B) - Yb| < {Epsilon:E}\n");
        }

        public void PrintMethodInfo(bool useRk4)
        {
            var methodName = useRk4 ? "Рунге-Кутта 4-го порядка" : "Рунге-Кутта 2-го порядка";
            double integrationStep = useRk4 ? 1e-4 : 1e-6;

            Console.WriteLine($"Метод стрельбы ({methodName}) для поиска y'(0)");
            Console.WriteLine($"Используемый шаг интегрирования в ShotFunction при поиске y'(0): {integrationStep:E}");
        }

        public void PrintFinalResults((double Y, double DY) result, bool useRk4)
        {
            var methodName = useRk4 ? "Рунге-Кутта 4-го порядка" : "Рунге-Кутта 2-го порядка";

            Console.WriteLine($"Итоговый прогон ({methodName})");

            Console.WriteLine($"\nРезультат в точке {B}: y = {result.Y:F14}, y' = {result.DY:F14}");
            Console.WriteLine($"Ожидаемое значение y({B}): {Yb:F14}");
            Console.WriteLine($"Разница |y({B}) - Yb|: {Math.Abs(result.Y - Yb):E}");

            double expectedDYb = Math.Exp(B);

            Console.WriteLine($"Ожидаемое значение y'({B}): {expectedDYb:F14}");
            Console.WriteLine($"Разница |y'({B}) - exp({B})|: {Math.Abs(result.DY - expectedDYb):E}\n");
        }
    }
}
