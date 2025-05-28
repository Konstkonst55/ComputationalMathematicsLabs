using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkExt
{
    public class ShootingMethodSolver : BaseSolver
    {
        private readonly RungeKuttaSolver _rungeKuttaSolver;

        public ShootingMethodSolver(Func<double, double, double, (double, double)> function, bool useRk4)
        {
            _rungeKuttaSolver = new RungeKuttaSolver(function, useRk4);
        }

        public override (double Y, double DY) Compute(double a, double b, double h, double y0, double dy0, bool useWrite = true)
        {
            return _rungeKuttaSolver.Compute(a, b, h, y0, dy0, useWrite);
        }

        public double FindInitialSlope(double left, double right, double kIntervalTolerance, double integrationStep, bool useWrite = true)
        {
            if (useWrite) Console.WriteLine($"Начальный интервал для k: [{left:F14}, {right:F14}], шаг интегрирования для ShotFunction = {integrationStep:E}, точность для k = {kIntervalTolerance:E}");

            double fa = ShotFunction(left, integrationStep);
            double fb = ShotFunction(right, integrationStep);

            if (Math.Abs(fa) < Epsilon)
            {
                if (useWrite) Console.WriteLine($"\nКорень найден на левой границе: k = {left:F14}");

                return left;
            }

            if (Math.Abs(fb) < Epsilon)
            {
                if (useWrite) Console.WriteLine($"\nКорень найден на правой границе: k = {right:F14}");

                return right;
            }

            if (fa * fb >= 0)
            {
                if (useWrite) Console.WriteLine($"Предупреждение: f(a) и f(b) одного знака. fa={fa:E}, fb={fb:E}. Поиск может быть некорректным.");
            }

            double mid = left;
            int iter = 0;
            const int maxIter = 200;

            while (Math.Abs(right - left) > kIntervalTolerance && iter < maxIter)
            {
                iter++;
                mid = (left + right) / 2;

                double fm = ShotFunction(mid, integrationStep);

                if (Math.Abs(fm) < Epsilon)
                {
                    break;
                }

                if (fa * fm < 0)
                {
                    right = mid;
                    fb = fm;
                }
                else
                {
                    left = mid;
                    fa = fm;
                }
            }

            if (iter == maxIter && useWrite)
            {
                Console.WriteLine("Предупреждение: Достигнуто максимальное количество итераций в FindInitialSlope.");
            }

            if (useWrite) Console.WriteLine($"\nЗавершение поиска k: итераций={iter}, интервал k = [{left:F14}, {right:F14}]");

            return (left + right) / 2;
        }

        public double AdjustStep(double eps, double initialH, double y0, double dy0, bool useWrite = true)
        {
            const double MinH = 1e-15;
            double h = initialH;

            if (useWrite) Console.WriteLine($"Подбор шага для y'(0)={dy0:F14}, целевая ошибка для y(B) < {eps:E}");

            int iter = 0;
            const int maxIter = 100;

            while (h > MinH && iter < maxIter)
            {
                iter++;
                var result = _rungeKuttaSolver.Compute(A, B, h, y0, dy0, false);
                double error = Math.Abs(result.Y - Yb);

                if (useWrite) Console.WriteLine($"Итерация {iter}: h = {h:E}, y(b) = {result.Y:F14}, ошибка |y(b)-Yb| = {error:E}");

                if (error < eps)
                {
                    if (useWrite) Console.WriteLine($"Достигнута точность {eps:E}: оптимальный шаг = {h:E}");

                    return h;
                }

                h /= 2;
            }

            if (iter == maxIter && h > MinH)
            {
                if (useWrite) Console.WriteLine("Предупреждение: Достигнуто максимальное количество итераций в AdjustStep, точность может быть не достигнута.");
            }
            else if (h <= MinH)
            {
                if (useWrite) Console.WriteLine("Минимальный шаг достигнут, но требуемая точность не достигнута.");
            }

            return h;
        }

        private double ShotFunction(double k, double integrationStep)
        {
            var result = _rungeKuttaSolver.Compute(A, B, integrationStep, Ya, k, false);

            return result.Y - Yb;
        }
    }
}
