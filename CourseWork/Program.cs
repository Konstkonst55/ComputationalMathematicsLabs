namespace CourseWork
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RungeKuttaSolver.PrintHeader("Решение ДУ методом Рунге-Кутта с адаптивным шагом");
            RungeKuttaSolver.PrintEquationInfo();

            RungeKuttaSolver.RunSolver("RK4", useFourthOrder: true);
            RungeKuttaSolver.RunSolver("RK2", useFourthOrder: false);
        }
    }
}
