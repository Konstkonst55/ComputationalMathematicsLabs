using Shared;

namespace _7
{
    public class CubicSplineInterpolation : InterpolationBase<double>
    {
        private double[] _segmentLengths;
        private double[] _slopes;
        private double[] _splineCoefficients;

        public CubicSplineInterpolation(List<(double x, double y)> dataPoints)
            : base(dataPoints)
        {
            int numSegments = PointsCount - 1;

            _segmentLengths = new double[numSegments];
            _slopes = new double[numSegments];
            _splineCoefficients = new double[PointsCount];

            for (int i = 0; i < numSegments; i++)
            {
                _segmentLengths[i] = _points[i + 1].x - _points[i].x;
                _slopes[i] = (_points[i + 1].y - _points[i].y) / _segmentLengths[i];
            }

            ComputeSplineCoefficients();
        }

        private void ComputeSplineCoefficients()
        {
            int numEquations = PointsCount;
            double[,] matrix = new double[numEquations, numEquations];
            double[] results = new double[numEquations];

            double h0 = _segmentLengths[0];
            double h1 = _segmentLengths[1];

            matrix[0, 0] = Math.Pow(h1, 2);
            matrix[0, 1] = -(Math.Pow(h0, 2) + Math.Pow(h1, 2));
            matrix[0, 2] = Math.Pow(h0, 2);
            results[0] = 0.0;

            for (int i = 1; i < numEquations - 1; i++)
            {
                double hi_1 = _segmentLengths[i - 1];
                double hi = _segmentLengths[i];

                matrix[i, i - 1] = hi_1;
                matrix[i, i] = 2 * (hi_1 + hi);
                matrix[i, i + 1] = hi;
                results[i] = 6 * (_slopes[i] - _slopes[i - 1]);
            }

            double hnm2 = _segmentLengths[numEquations - 2];
            double hnm3 = _segmentLengths[numEquations - 3];

            matrix[numEquations - 1, numEquations - 3] = Math.Pow(hnm2, 2);
            matrix[numEquations - 1, numEquations - 2] = -(Math.Pow(hnm2, 2) + Math.Pow(hnm3, 2));
            matrix[numEquations - 1, numEquations - 1] = Math.Pow(hnm3, 2);
            results[numEquations - 1] = 0.0;

            GaussianElimination gauss = new GaussianElimination(matrix, results);
            double[] M = gauss.Solve();

            for (int i = 0; i < numEquations; i++)
            {
                _splineCoefficients[i] = M[i];
            }
        }

        public override double Compute(double x, bool useWrite = true)
        {
            int index = FindSegment(x);

            double x0 = _points[index - 1].x;
            double x1 = _points[index].x;
            double y0 = _points[index - 1].y;
            double y1 = _points[index].y;
            double m0 = _splineCoefficients[index - 1];
            double m1 = _splineCoefficients[index];
            double h = _segmentLengths[index - 1];

            double t = (x - x0) / h;
            double term1 = (1 - t) * y0 + t * y1;
            double term2 = ((1 - t) * (1 - t) * (1 - t) - (1 - t)) * m0 * h * h / 6.0;
            double term3 = (t * t * t - t) * m1 * h * h / 6.0;

            return term1 + term2 + term3;
        }

        private int FindSegment(double x)
        {
            int n = PointsCount;

            if (x <= _points[0].x)
            {
                return 1;
            }

            if (x >= _points[n - 1].x)
            {
                return n - 1;
            }

            for (int i = 1; i < n; i++)
            {
                if (x <= _points[i].x)
                {
                    return i;
                }
            }
                
            return n - 1;
        }
    }
}
