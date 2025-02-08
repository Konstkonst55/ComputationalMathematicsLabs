namespace _2
{
    internal class NumberWithError
    {
        public double Value { get; }
        public double Error { get; }

        public NumberWithError(double value, double error)
        {
            Value = value;
            Error = error;
        }

        public static NumberWithError operator +(NumberWithError a, NumberWithError b)
        {
            return new NumberWithError(a.Value + b.Value, a.Error + b.Error);
        }

        public static NumberWithError operator -(NumberWithError a, NumberWithError b)
        {
            return new NumberWithError(a.Value - b.Value, a.Error + b.Error);
        }

        public static NumberWithError operator *(NumberWithError a, NumberWithError b)
        {
            double relativeError = (a.Error / a.Value) + (b.Error / b.Value);

            return new NumberWithError(a.Value * b.Value, a.Value * b.Value * relativeError);
        }

        public static NumberWithError operator /(NumberWithError a, NumberWithError b)
        {
            double relativeError = (a.Error / a.Value) + (b.Error / b.Value);

            return new NumberWithError(a.Value / b.Value, (a.Value / b.Value) * relativeError);
        }

        public NumberWithError Pow(double exponent)
        {
            double newValue = Math.Pow(Value, exponent);
            double relativeError = Math.Abs(exponent) * (Error / Value);

            return new NumberWithError(newValue, newValue * relativeError);
        }

        public NumberWithError Sqrt()
        {
            return Pow(0.5);
        }

        public NumberWithError Sin()
        {
            double newValue = Math.Sin(Value);
            double newError = Math.Cos(Value) * Error;

            return new NumberWithError(newValue, Math.Abs(newError));
        }

        public NumberWithError Cos()
        {
            double newValue = Math.Cos(Value);
            double newError = Math.Sin(Value) * Error;

            return new NumberWithError(newValue, Math.Abs(newError));
        }

        public NumberWithError Ln()
        {
            double newValue = Math.Log(Value);
            double newError = Error / Value;

            return new NumberWithError(newValue, newError);
        }

        public override string ToString()
        {
            return $"{Value:F4} +- {Error:F4}";
        }
    }
}
