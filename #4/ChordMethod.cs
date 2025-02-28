using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4
{
    public class ChordMethod : RootFindingMethod
    {
        public ChordMethod(Func<double, double> function, double tolerance = 1e-6)
            : base(function, tolerance) { }

        protected override double FindC(double a, double b)
        {
            return (a * _function(b) - b * _function(a)) / (_function(b) - _function(a));
        }

        public override double Solve(double a, double b) => new BisectionMethod(_function, _tolerance).Solve(a, b);
    }
}
