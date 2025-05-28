using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkExt
{
    public class RungeKuttaSolver : BaseSolver
    {
        private readonly Func<double, double, double, (double, double)> _function;
        private readonly bool _useRk4;

        public RungeKuttaSolver(Func<double, double, double, (double, double)> function, bool useRk4)
        {
            _function = function;
            _useRk4 = useRk4;
        }

        public override (double Y, double DY) Compute(double a, double b, double h, double y0, double dy0, bool useWrite = true)
        {
            return _useRk4 ? RungeKutta4(a, b, h, y0, dy0, useWrite) : RungeKutta2(a, b, h, y0, dy0, useWrite);
        }

        private (double Y, double DY) RungeKutta4(double a, double b, double h, double y0, double dy0, bool useWrite = true)
        {
            if (h <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(h), "Шаг h должен быть положительным.");
            }

            int n = (int)Math.Ceiling(Math.Abs(b - a) / h);

            if (n == 0)
            {
                n = 1;
            }

            double actualH = (b - a) / n;
            double[] X = new double[n + 1];

            for (int i = 0; i <= n; ++i)
            {
                X[i] = a + i * actualH;
            }

            double y = y0, dy = dy0;

            if (useWrite && X.Length > 0) Console.WriteLine($"x = {X[0]:F14} | y = {y:F14} | y' = {dy:F14}");
            
            for (int i = 1; i <= n; i++)
            {
                var (k10, k11) = _function(X[i - 1], y, dy);
                var (k20, k21) = _function(X[i - 1] + actualH / 2, y + actualH / 2 * k10, dy + actualH / 2 * k11);
                var (k30, k31) = _function(X[i - 1] + actualH / 2, y + actualH / 2 * k20, dy + actualH / 2 * k21);
                var (k40, k41) = _function(X[i - 1] + actualH, y + actualH * k30, dy + actualH * k31);

                y += actualH / 6 * (k10 + 2 * k20 + 2 * k30 + k40);
                dy += actualH / 6 * (k11 + 2 * k21 + 2 * k31 + k41);

                if (useWrite) Console.WriteLine($"x = {X[i]:F14} | y = {y:F14} | y' = {dy:F14}");
            }

            return (y, dy);
        }

        private (double Y, double DY) RungeKutta2(double a, double b, double h, double y0, double dy0, bool useWrite = true)
        {
            if (h <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(h), "Шаг h должен быть положительным.");
            }

            int n = (int)Math.Ceiling(Math.Abs(b - a) / h);

            if (n == 0)
            {
                n = 1;
            }

            double actualH = (b - a) / n;

            double[] X = new double[n + 1];

            for (int i = 0; i <= n; ++i)
            {
                X[i] = a + i * actualH;
            }

            double y = y0, dy = dy0;

            if (useWrite && X.Length > 0) Console.WriteLine($"x = {X[0]:F14} | y = {y:F14} | y' = {dy:F14}");

            for (int i = 1; i <= n; i++)
            {
                var (k10, k11) = _function(X[i - 1], y, dy);
                var (k20, k21) = _function(X[i - 1] + actualH / 2, y + actualH / 2 * k10, dy + actualH / 2 * k11);

                y += actualH * k20;
                dy += actualH * k21;

                if (useWrite) Console.WriteLine($"x = {X[i]:F14} | y = {y:F14} | y' = {dy:F14}");
            }

            return (y, dy);
        }
    }

}
