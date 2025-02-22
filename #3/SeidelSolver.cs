namespace _3
{
    internal class SeidelSolver : SimpleIterationSolver
    {
        public SeidelSolver(double[,] matrix, double[] results, double epsilon = 1e-4, int maxIterations = 100)
            : base(matrix, results, epsilon, maxIterations) { }

        public override void Solve()
        {
            int size = coefficientsMatrix.GetLength(0);
            double[] currentSolution = new double[size];

            Console.WriteLine("\nМатрица коэффициентов:");
            PrintMatrix(coefficientsMatrix);

            Console.WriteLine("\nВектор правой части:");
            PrintVector(constantsVector);

            Console.WriteLine("\nНачальный вектор x:");
            PrintVector(currentSolution);

            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                double[] previousSolution = (double[])currentSolution.Clone();

                for (int i = 0; i < size; i++)
                {
                    double sum = constantsVector[i];

                    for (int j = 0; j < size; j++)
                    {
                        if (i != j)
                        {
                            sum -= coefficientsMatrix[i, j] * currentSolution[j];
                        }
                    }

                    currentSolution[i] = sum / coefficientsMatrix[i, i];
                }

                Console.WriteLine($"\nШаг {iteration + 1}:");
                PrintVector(currentSolution);

                if (HasConverged(previousSolution, currentSolution))
                {
                    Console.WriteLine("\nРешение найдено:");
                    PrintVector(currentSolution);
                    return;
                }
            }

            Console.WriteLine("\nДостигнуто максимальное число итераций.");
        }
    }
}
