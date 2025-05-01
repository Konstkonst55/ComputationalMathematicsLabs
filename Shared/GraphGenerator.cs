using System.Drawing;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Shared
{
    public static class GraphGenerator
    {
        public static List<GraphParameters> GenerateData(InterpolationBase<double> interpolator, Func<double, double> function, double xMin)
        {
            const double step = 0.1, margin = 1, smoothing = 100.0;

            List<(double x, double y)> dataPoints = interpolator.GetPoints(), xyValues = new();

            const double viewStep = step / smoothing;
            double xMax = dataPoints.Max().x;
            int viewCount = Convert.ToInt32(((xMax - xMin) / viewStep) + 1);

            List<(double x, double y)> viewDataPoints = ListUtils.FillDataPoints(function, xMin, viewStep, viewCount);

            double minX = dataPoints.Min(p => p.x);
            double maxX = dataPoints.Max(p => p.x);

            double delta = (maxX - minX) * margin;
            double plotMinX = minX - delta;
            double plotMaxX = maxX + delta;

            for (double x = plotMinX; x <= plotMaxX; x += step)
            {
                double y = interpolator.Compute(x, false);

                if (double.IsFinite(y) && Math.Abs(y) < 1e6)
                {
                    xyValues.Add((x, y));
                }
            }

            double yMin = xyValues.Min(p => p.y);
            double yMax = xyValues.Max(p => p.y);
            double yRange = yMax - yMin;

            List<GraphParameters> graphData = new()
            {
                new(xyValues, 0, 6, Color.Red),
                new(dataPoints, 10, 0, Color.Green),
                new(viewDataPoints, 0, 2, Color.Green, true),
            };

            return graphData;
        }

        public static List<GraphParameters> GenerateTrigonometricData(InterpolationBase<Complex> interpolator, Func<double, double> function, double xMin)
        {
            const double step = 0.01, margin = 0.5, smoothing = 100.0;

            List<(double x, double y)> dataPoints = interpolator.GetPoints();
            List<(double x, double y)> realValues = new();
            List<(double x, double y)> imaginaryValues = new();

            const double viewStep = step / smoothing;
            double xMax = dataPoints.Max().x;
            int viewCount = Convert.ToInt32(((xMax - xMin) / viewStep) + 1);

            List<(double x, double y)> viewDataPoints = ListUtils.FillDataPoints(function, xMin, viewStep, viewCount);

            double minX = dataPoints.Min(p => p.x);
            double maxX = dataPoints.Max(p => p.x);

            double delta = (maxX - minX) * margin;
            double plotMinX = minX - delta;
            double plotMaxX = maxX + delta;

            for (double x = plotMinX; x <= plotMaxX; x += step)
            {
                Complex result = interpolator.Compute(x, false);

                if (double.IsFinite(result.Real) && Math.Abs(result.Real) < 1e6)
                {
                    realValues.Add((x, result.Real));
                }

                if (double.IsFinite(result.Imaginary) && Math.Abs(result.Imaginary) < 1e6)
                {
                    imaginaryValues.Add((x, result.Imaginary));
                }
            }

            double yMinReal = realValues.Min(p => p.y);
            double yMaxReal = realValues.Max(p => p.y);
            double yMinImag = imaginaryValues.Min(p => p.y);
            double yMaxImag = imaginaryValues.Max(p => p.y);

            double yMin = Math.Min(yMinReal, yMinImag);
            double yMax = Math.Max(yMaxReal, yMaxImag);

            List<GraphParameters> graphData = new()
            {
                new(realValues, 0, 6, Color.Red, true),
                new(imaginaryValues, 0, 6, Color.Green),
                new(dataPoints, 10, 0, Color.Blue),
                new(viewDataPoints, 0, 2, Color.Blue)
            };

            return graphData;
        }
    }
}
