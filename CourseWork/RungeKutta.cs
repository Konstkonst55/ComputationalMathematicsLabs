namespace CourseWork
{
    public static class RungeKuttaSolver
    {
        private const double Tolerance = 1e-12;
        private const double InitialStepSize = 0.1;
        private const double StartX = 0.0;
        private const double EndX = 1.0;
        private const double InitialY = 1.0;
        private const double InitialYPrime = 1.0;

        public static void RunSolver(string methodName, bool useFourthOrder)
        {
            PrintSubheader($"Адаптация шага для {methodName}");
            double optimalStep = FindOptimalStep(Tolerance, InitialStepSize, useFourthOrder);

            PrintSubheader($"Решение методом {methodName}");
            SolveODE(StartX, EndX, optimalStep, InitialY, InitialYPrime, useFourthOrder);
        }

        static (double y, double v) SolveODE(double xStart, double xEnd, double step, double y0, double v0, bool useFourthOrder, bool verbose = false)
        {
            int stepCount = (int)Math.Round((xEnd - xStart) / step);
            double x = xStart, y = y0, v = v0;

            if (verbose) PrintState(0, x, y, v);

            for (int i = 1; i <= stepCount; i++)
            {
                (y, v) = useFourthOrder ? RungeKutta4Step(x, y, v, step) : RungeKutta2Step(x, y, v, step);

                x = xStart + i * step;

                if (verbose) PrintState(i, x, y, v);
            }

            return (y, v);
        }

        static (double yNext, double vNext) RungeKutta4Step(double x, double y, double v, double h)
        {
            var k1 = GetDerivatives(x, y, v);
            var k2 = GetDerivatives(x + h / 2, y + h / 2 * k1[0], v + h / 2 * k1[1]);
            var k3 = GetDerivatives(x + h / 2, y + h / 2 * k2[0], v + h / 2 * k2[1]);
            var k4 = GetDerivatives(x + h, y + h * k3[0], v + h * k3[1]);

            double yNext = y + h / 6 * (k1[0] + 2 * k2[0] + 2 * k3[0] + k4[0]);
            double vNext = v + h / 6 * (k1[1] + 2 * k2[1] + 2 * k3[1] + k4[1]);

            return (yNext, vNext);
        }

        static (double yNext, double vNext) RungeKutta2Step(double x, double y, double v, double h)
        {
            var k1 = GetDerivatives(x, y, v);
            var k2 = GetDerivatives(x + h / 2, y + h / 2 * k1[0], v + h / 2 * k1[1]);

            double yNext = y + h * k2[0];
            double vNext = v + h * k2[1];

            return (yNext, vNext);
        }

        static double[] GetDerivatives(double x, double y, double v)
        {
            return
            [
                v,
                (Math.Exp(x) + y + v) / 3.0
            ];
        }

        static double FindOptimalStep(double tolerance, double initialStep, bool useFourthOrder)
        {
            double h = initialStep;
            double error;
            int iteration = 0;

            do
            {
                iteration++;

                var (yCoarse, _) = SolveODE(StartX, EndX, h, InitialY, InitialYPrime, useFourthOrder);
                var (yFine, _) = SolveODE(StartX, EndX, h / 2, InitialY, InitialYPrime, useFourthOrder);

                error = Math.Abs(yCoarse - yFine);

                PrintStepAdjustment(iteration, error, h);

                h /= 2;

            } while (error > tolerance);

            PrintSuccessMessage($"Оптимальный шаг: {h:F6}");

            return h;
        }

        public static void PrintHeader(string title)
        {
            Console.WriteLine(new string('-', 60));
            Console.WriteLine($"{title}");
            Console.WriteLine(new string('-', 60));
        }

        public static void PrintSubheader(string subtitle)
        {
            Console.WriteLine($"\n- {subtitle} -\n");
        }

        public static void PrintEquationInfo()
        {
            Console.WriteLine($"Уравнение: y'' = (e^x + y + y') / 3");
            Console.WriteLine($"Начальные условия: y({StartX}) = {InitialY}, y'({StartX}) = {InitialYPrime}\n");
        }

        public static void PrintState(int step, double x, double y, double v)
        {
            Console.WriteLine($"[{step,3}]  x = {x,6:F4} | y = {y,10:F6} | y' = {v,10:F6}");
        }

        public static void PrintStepAdjustment(int iteration, double error, double step)
        {
            Console.WriteLine($"[{iteration,4}] Ошибка = {error,10:F8}, Шаг = {step,10:F6}");
        }

        public static void PrintSuccessMessage(string message)
        {
            Console.WriteLine($"\n{message}\n");
        }
    }
}
