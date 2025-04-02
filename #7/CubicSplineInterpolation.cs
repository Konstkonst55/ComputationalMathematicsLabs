namespace _7
{
    public class CubicSplineInterpolation
    {
        private double[] _xPoints;
        private double[] _yPoints;
        private double[] _segmentLengths;
        private double[] _slopes;
        private double[] _splineCoefficients;

        public CubicSplineInterpolation(List<(double x, double y)> points)
        {
            if (points.Count < 3)
            {
                throw new ArgumentException("Для кубического сплайна требуется минимум 3 точки");
            }

            _xPoints = points.Select(p => p.x).ToArray();
            _yPoints = points.Select(p => p.y).ToArray();
            int numSegments = _xPoints.Length - 1;

            _segmentLengths = new double[numSegments];
            _slopes = new double[numSegments];

            PrintListAsTable(points, "Исходные данные");

            for (int i = 0; i < numSegments; i++)
            {
                _segmentLengths[i] = _xPoints[i + 1] - _xPoints[i];
                _slopes[i] = (_yPoints[i + 1] - _yPoints[i]) / _segmentLengths[i];
            }

            Console.WriteLine("\nВычисленные длины сегментов (h_i):");
            Console.WriteLine(string.Join("; ", _segmentLengths));

            ComputeSplineCoefficients();
        }

        private void ComputeSplineCoefficients()
        {
            int numEquations = _segmentLengths.Length - 1;
            double[,] matrix = new double[numEquations, numEquations];
            double[] results = new double[numEquations];

            Console.WriteLine("\nФормирование системы уравнений (матрица коэффициентов и правая часть):");

            for (int i = 0; i < numEquations; i++)
            {
                if (i > 0)
                {
                    matrix[i, i - 1] = _segmentLengths[i] / 3;
                }

                matrix[i, i] = 2 * (_segmentLengths[i] + (i < numEquations - 1 ? _segmentLengths[i + 1] : 0)) / 3;

                if (i < numEquations - 1)
                {
                    matrix[i, i + 1] = _segmentLengths[i + 1] / 3;
                }

                results[i] = _slopes[i + 1] - _slopes[i];
            }

            PrintMatrix(matrix, results);

            GaussianElimination gauss = new GaussianElimination(matrix, results);
            double[] solution = gauss.Solve();

            _splineCoefficients = new double[_xPoints.Length];

            for (int i = 1; i < _splineCoefficients.Length - 1; i++)
            {
                _splineCoefficients[i] = solution[i - 1];
            }

            Console.WriteLine("\nНайденные коэффициенты M_i:");
            Console.WriteLine(string.Join("; ", _splineCoefficients));
        }

        public double Compute(double x, bool useWrite = true)
        {
            int index = FindSegment(x);

            if (useWrite)
            {
                Console.WriteLine($"\nВычисление S({x}):");
                Console.WriteLine($"Точка x = {x} лежит в промежутке [{_xPoints[index - 1]}; {_xPoints[index]}], значит index = {index}");
            }

            double x0 = _xPoints[index - 1];
            double x1 = _xPoints[index];
            double y0 = _yPoints[index - 1];
            double y1 = _yPoints[index];
            double h = _segmentLengths[index - 1];
            double m0 = _splineCoefficients[index - 1];
            double m1 = _splineCoefficients[index];

            double[] terms =
            {
                ((x1 - x) * (x1 - x) * (x1 - x) * m0) / (6 * h),
                ((x - x0) * (x - x0) * (x - x0) * m1) / (6 * h),
                ((y0 - (m0 * h * h) / 6) * (x1 - x)) / h,
                ((y1 - (m1 * h * h) / 6) * (x - x0)) / h
            };

            double result = terms.Sum();

            if (useWrite) Console.WriteLine($"S({x}) = {string.Join(" + ", terms)} = {result}");

            return result;
        }

        private int FindSegment(double x)
        {
            for (int i = 1; i < _xPoints.Length; i++)
            {
                if (x <= _xPoints[i])
                {
                    return i;
                }
            }

            return _xPoints.Length - 1;
        }

        private void PrintMatrix(double[,] matrix, double[] results, string title = "")
        {
            int size = results.Length;

            if (title.Length > 0)
            {
                Console.WriteLine("\n" + title);
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write($"{matrix[i, j], 8:F4} ");
                }

                Console.WriteLine($" | {results[i], 8:F4}");
            }
        }

        private void PrintListAsTable(List<(double x, double y)> points, string title)
        {
            if (title.Length > 0)
            {
                Console.WriteLine(title + "\n");
            }

            Console.WriteLine("{0,-15} {1,-15}", "X", "Y");
            Console.WriteLine(new string('-', 30));

            foreach (var (x, y) in points)
            {
                Console.WriteLine("{0,-15:F2} {1,-15:F6}", x, y);
            }
        }
    }
}
