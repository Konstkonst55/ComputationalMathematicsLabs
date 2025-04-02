using _7;

internal class Program
{
    static List<(double x, double y)> dataPoints = new()
    {
        (1, 2),
        (3, 5),
        (5, 2),
        (7, -1),
        (9, 2)
    };

    static CubicSplineInterpolation csInterpolator;
    static double result;
    static double xTarget = 2;

    [STAThread]
    static void Main(string[] args)
    {
        csInterpolator = new CubicSplineInterpolation(dataPoints);
        result = csInterpolator.Compute(xTarget);

        GenerateGraph();
    }

    static void GenerateGraph()
    {
        List<(double x, double y)> xyValues = new();
        const double axisLimit = 100, step = 0.5;

        for (double x = -axisLimit; x <= axisLimit; x += step)
        {
            double y = csInterpolator.Compute(x, false);
            xyValues.Add((x, y));
        }

        List<(double x, double y)> targetPoint = new()
        {
            (xTarget, result)
        };

        double minX = dataPoints.Min(p => p.x);
        double maxX = dataPoints.Max(p => p.x);

        List<(double x, double y)> limitsLeft = new()
        {
            (minX, axisLimit),
            (minX, -axisLimit)
        };
        
        List<(double x, double y)> limitsRight = new()
        {
            (maxX, axisLimit),
            (maxX, -axisLimit)
        };

        List<List<(double x, double y)>> graphData = new()
        {
            xyValues,
            dataPoints,
            targetPoint,
            limitsRight,
            limitsLeft
        };

        PlotterForms.Program.ShowGraph(graphData);
    }
}