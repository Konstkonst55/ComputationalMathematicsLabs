namespace _2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            NumberWithError x = new NumberWithError(2.384, 0.021);
            NumberWithError y = new NumberWithError(9.485, 0.014);

            Console.WriteLine($"x = {x}");
            Console.WriteLine($"y = {y}\n");

            NumberWithError sum = x + y;
            NumberWithError diff = x - y;
            NumberWithError prod = x * y;
            NumberWithError quotient = x / y;
            NumberWithError ySquared = y.Pow(2);
            NumberWithError sqrtX = x.Sqrt();
            NumberWithError sinX = x.Sin();
            NumberWithError cosX = x.Cos();
            NumberWithError lnX = x.Ln();

            Console.WriteLine($"x + y = {sum}");
            Console.WriteLine($"x - y = {diff}");
            Console.WriteLine($"x * y = {prod}");
            Console.WriteLine($"x / y = {quotient}");
            Console.WriteLine($"y^2 = {ySquared}");
            Console.WriteLine($"sqrt(x) = {sqrtX}");
            Console.WriteLine($"sin(x) = {sinX}");
            Console.WriteLine($"cos(x) = {cosX}");
            Console.WriteLine($"ln(x) = {lnX}");
        }
    }
}
