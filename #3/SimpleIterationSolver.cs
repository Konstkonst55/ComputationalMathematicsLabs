namespace _3
{
    internal class SimpleIterationSolver
    {
        protected double[,] coefficientsMatrix; // A
        protected double[] constantsVector;     // b
        protected double[,] iterationMatrix;    // C
        protected double[] iterationVector;     // B
        protected double epsilon;
        protected int maxIterations;

        public SimpleIterationSolver(double[,] matrix, double[] results, double epsilon = 1e-4, int maxIterations = 100)
        {
            int size = matrix.GetLength(0);
            coefficientsMatrix = matrix;
            constantsVector = results;
            this.epsilon = epsilon;
            this.maxIterations = maxIterations;

            iterationMatrix = new double[size, size];
            iterationVector = new double[size];

            ComputeIterationParameters();
        }

        private void ComputeIterationParameters()
        {
            int size = coefficientsMatrix.GetLength(0);

            for (int i = 0; i < size; i++)
            {
                double diagonalElement = coefficientsMatrix[i, i];

                for (int j = 0; j < size; j++)
                {
                    iterationMatrix[i, j] = i == j ? 1 : coefficientsMatrix[i, j] / diagonalElement;
                }

                iterationVector[i] = constantsVector[i] / diagonalElement;
            }
        }

        virtual public void Solve()
        {
            int size = coefficientsMatrix.GetLength(0);
            double[] previousSolution = new double[size];
            double[] currentSolution = new double[size];

            Console.WriteLine("Матрица итераций C (с единичной диагональю):");
            PrintMatrix(iterationMatrix);

            Console.WriteLine("\nВектор правой части B:");
            PrintVector(iterationVector);

            Console.WriteLine("\nНачальный вектор x:");
            PrintVector(previousSolution);

            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                for (int i = 0; i < size; i++)
                {
                    currentSolution[i] = iterationVector[i];

                    for (int j = 0; j < size; j++)
                    {
                        if (i != j)
                        {
                            currentSolution[i] -= iterationMatrix[i, j] * previousSolution[j];
                        }
                    }
                }

                Console.WriteLine($"\nШаг {iteration + 1}:");
                PrintVector(currentSolution);

                if (HasConverged(previousSolution, currentSolution))
                {
                    Console.WriteLine("\nРешение найдено:");
                    PrintVector(currentSolution);
                    return;
                }

                Array.Copy(currentSolution, previousSolution, size);
            }

            Console.WriteLine("\nДостигнуто максимальное число итераций.");
        }

        protected bool HasConverged(double[] oldSolution, double[] newSolution)
        {
            for (int i = 0; i < oldSolution.Length; i++)
            {
                if (Math.Abs(newSolution[i] - oldSolution[i]) > epsilon)
                {
                    return false;
                }
            }

            return true;
        }

        protected void PrintVector(double[] vector)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                Console.WriteLine($"x[{i}] = {vector[i]:F6}");
            }
        }

        protected void PrintMatrix(double[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write($"{matrix[i, j],8:F4} ");
                }

                Console.WriteLine();
            }
        }
    }
}
