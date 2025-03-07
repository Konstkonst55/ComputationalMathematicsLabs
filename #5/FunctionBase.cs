namespace _5
{
    public abstract class FunctionBase
    {
        protected readonly double _tolerance;
        private readonly int _precision;

        public FunctionBase(double tolerance = 1e-6)
        {
            _tolerance = tolerance;
            _precision = Math.Max(1, (int)Math.Ceiling(-Math.Log10(tolerance)) + 1);
        }

        protected string Round(double value) => value.ToString($"F{_precision}");
    }
}
