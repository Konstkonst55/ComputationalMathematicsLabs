namespace _1
{
    public class ModifiedGaussianElimination : GaussianElimination
    {
        public ModifiedGaussianElimination(double[,] inputMatrix, double[] inputResults)
            : base(inputMatrix, inputResults) { }

        public override void ConvertToLowerTriangular()
        {
            for (int i = 0; i < _size - 1; i++)
            {
                SwapRowsForMaxPivot(i);

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

        private void SwapRowsForMaxPivot(int col)
        {
            int maxRow = col;
            double maxVal = Math.Abs(_matrix[col, col]);

            for (int i = col + 1; i < _size; i++)
            {
                if (Math.Abs(_matrix[i, col]) > maxVal)
                {
                    maxVal = Math.Abs(_matrix[i, col]);
                    maxRow = i;
                }
            }

            if (maxRow != col)
            {
                for (int i = 0; i < _size; i++)
                {
                    double temp = _matrix[col, i];
                    _matrix[col, i] = _matrix[maxRow, i];
                    _matrix[maxRow, i] = temp;
                }

                double tempResult = _results[col];
                _results[col] = _results[maxRow];
                _results[maxRow] = tempResult;
            }
        }
    }
}