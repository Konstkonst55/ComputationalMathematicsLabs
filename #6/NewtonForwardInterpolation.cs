﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6
{
    public class NewtonForwardInterpolation : NewtonInterpolation
    {
        public NewtonForwardInterpolation(Dictionary<double, double> dataPoints) : base(dataPoints) { }

        public double Compute(double x, bool useWrite = true)
        {
            List<double> xValues = new List<double>(_points.Keys);
            double h = xValues[1] - xValues[0];
            double q = (x - xValues[0]) / h;

            if (useWrite) Console.WriteLine($"q = ({x} - {xValues[0]}) / {h} = {q:F6}");

            double result = _differenceTable[0, 0];
            double term = 1;

            for (int i = 1; i < _points.Count; i++)
            {
                term *= (q - (i - 1)) / i;
                double addTerm = _differenceTable[0, i] * term;
                result += addTerm;

                if (useWrite) Console.WriteLine($"Шаг {i}: {_differenceTable[0, i]:F6} / {Factorial(i)} * {term:F6} = {addTerm:F6}");
            }

            return result;
        }
    }

}
