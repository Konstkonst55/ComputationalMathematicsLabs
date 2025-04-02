using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7
{
    public class GaussianElimination
    {
        private int _size;
        private double[,] _matrix;
        private double[] _results;

        public GaussianElimination(double[,] inputMatrix, double[] inputResults)
        {
            _size = inputMatrix.GetLength(0);
            _matrix = (double[,])inputMatrix.Clone();
            _results = (double[])inputResults.Clone();
        }

        private void ConvertToLowerTriangular()
        {
            for (int i = 0; i < _size - 1; i++)
            {
                for (int j = i + 1; j < _size; j++)
                {
                    double factor = _matrix[j, i] / _matrix[i, i];

                    for (int k = i; k < _size; k++)
                    {
                        _matrix[j, k] -= factor * _matrix[i, k];
                    }

                    _results[j] -= factor * _results[i];
                }
            }
        }

        private double[] BackSubstitution()
        {
            double[] solution = new double[_size];

            for (int i = _size - 1; i >= 0; i--)
            {
                double sum = _results[i];

                for (int j = i + 1; j < _size; j++)
                {
                    sum -= _matrix[i, j] * solution[j];
                }

                solution[i] = sum / _matrix[i, i];
            }

            return solution;
        }

        public double[] Solve()
        {
            ConvertToLowerTriangular();

            return BackSubstitution();
        }
    }
}
