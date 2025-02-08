namespace _3
{
    internal class SimpleIterationSolver
    {
        private double[,] coefficientsMatrix; // A
        private double[] constantsVector;     // b
        private double[,] iterationMatrix;    // C
        private double[] iterationVector;     // B
        private double epsilon;
        private int maxIterations;

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
                    iterationMatrix[i, j] = i == j ? 0 : -coefficientsMatrix[i, j] / diagonalElement;
                }

                iterationVector[i] = constantsVector[i] / diagonalElement;
            }
        }

        public void Solve()
        {
            int size = coefficientsMatrix.GetLength(0);
            double[] previousSolution = new double[size];
            double[] currentSolution = new double[size];

            Console.WriteLine("Начальный вектор x:");
            PrintVector(previousSolution);

            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                for (int i = 0; i < size; i++)
                {
                    currentSolution[i] = iterationVector[i];

                    for (int j = 0; j < size; j++)
                    {
                        currentSolution[i] += iterationMatrix[i, j] * previousSolution[j];
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

        private bool HasConverged(double[] oldSolution, double[] newSolution)
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

        private void PrintVector(double[] vector)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                Console.WriteLine($"x[{i}] = {vector[i]:F6}");
            }
        }
    }
}
