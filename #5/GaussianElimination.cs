using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5
{
    public class GaussianElimination
    {
        private int size;
        private double[,] matrix;
        private double[] results;

        public GaussianElimination(double[,] inputMatrix, double[] inputResults)
        {
            size = inputMatrix.GetLength(0);
            matrix = (double[,])inputMatrix.Clone();
            results = (double[])inputResults.Clone();
        }

        private void ConvertToLowerTriangular()
        {
            for (int i = 0; i < size - 1; i++)
            {
                for (int j = i + 1; j < size; j++)
                {
                    double factor = matrix[j, i] / matrix[i, i];

                    for (int k = i; k < size; k++)
                    {
                        matrix[j, k] -= factor * matrix[i, k];
                    }

                    results[j] -= factor * results[i];
                }
            }
        }

        private double[] BackSubstitution()
        {
            double[] solution = new double[size];

            for (int i = size - 1; i >= 0; i--)
            {
                double sum = results[i];

                for (int j = i + 1; j < size; j++)
                {
                    sum -= matrix[i, j] * solution[j];
                }

                solution[i] = sum / matrix[i, i];
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
