using System;
using System.Collections.Generic;

namespace Laba5_3
{
    public class IdentityMatrix : Matrix
    {
        public IdentityMatrix(int order) : base(order, order)
        {
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    elements[i, j] = i == j ? 1d : 0d;
        }

        public override void AddElements(double[] elems)
        {
            throw new NotSupportedException(
                "Единичная матрица не поддерживает " +
                "добавление элементов"
            );
        }

        public override double CalculateDeterminant()
        {
            return 1d;
        }
    }
    public class IdentityMatrixLaplace : IdentityMatrix
    {
        public IdentityMatrixLaplace(int order) : base(order)
        {
        }

        public override double CalculateDeterminant()
        {
            return CalculateDeterminantLaplace();
        }
    }
    public class Matrix
    {
        public int Width { get; }
        public int Height { get; }

        public int ElementsCount
        {
            get => Width * Height;
        }

        protected double[,] elements;

        protected const int Padding = 3;

        public Matrix(int width, int height)
        {
            Width = width;
            Height = height;

            elements = new double[height, width];
        }

        public void PrintElements()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                    Console.Write(elements[i, j].ToString().PadLeft(Padding) + ' ');

                Console.WriteLine();
            }
        }

        public virtual void AddElements(double[] elems)
        {
            if (elems.Length != this.elements.Length)
                throw new Exception(
                    $"Неверное количество элементов. " +
                    $"Ожидалось: {this.elements.Length}"
                );

            int inputIndex = 0;
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    this.elements[i, j] = elems[inputIndex++];
        }

        public virtual double CalculateDeterminant()
        {
            if (Width != Height)
                throw new Exception(
                    "Невозможно вычислить определитель " +
                    "не-квадратной матрицы"
                );

            return CalculateDeterminantLaplace();
        }
        protected double CalculateDeterminantLaplace()
        {

            if (Width == 2)
                return
                    elements[0, 0] * elements[1, 1] -
                    elements[1, 0] * elements[0, 1];

            const int row = 0;

            double result = 0d;
            for (int j = 0; j < Width; j++)
            {
                Matrix minorMatrix = new Matrix(Width - 1, Height - 1);
                double[] minorElems = new double[minorMatrix.ElementsCount];

                int index = 0;
                for (int i1 = 0; i1 < Height; i1++)
                {
                    if (i1 == row)
                        continue;

                    for (int j1 = 0; j1 < Width; j1++)
                    {
                        if (j1 == j)
                            continue;

                        minorElems[index++] = elements[i1, j1];
                    }
                }

                minorMatrix.AddElements(minorElems);
                minorElems = null;

                int sign = ((row + j + 2) % 2) == 0 ? 1 : -1;

                result +=
                    sign *
                    elements[row, j] *
                    minorMatrix.CalculateDeterminantLaplace();
            }

            return result;
        }
        protected double CalculateDeterminantLeibniz()
        {
            var numbers = new List<int>();

            for (int i = 0; i < Width; i++)
                numbers.Add(i);

            var permutations = GetAllPermutations(numbers);
            numbers = null;

            double result = 0d;
            int sign = 1;
            bool flipSign = true;
            foreach (var p in permutations)
            {
                double summand = 1d;
                for (int i = 0; i < Height; i++)
                    summand *= elements[i, p[i]];

                result += summand * sign;

                if (flipSign)
                    sign *= -1;

                flipSign = !flipSign;
            }

            return result;
        }
        private static List<List<int>> GetAllPermutations(List<int> numbers)
        {
            if (numbers.Count == 1)
                return new List<List<int>>()
                {
                    new List<int>() { numbers[0] }
                };

            var result = new List<List<int>>();
            var temp = new List<int>();

            for (int i = 0; i < numbers.Count; i++)
            {
                for (int j = 0; j < numbers.Count; j++)
                {
                    if (j == i)
                        continue;

                    temp.Add(numbers[j]);
                }

                var permutations = GetAllPermutations(temp);
                temp.Clear();

                foreach (var p in permutations)
                {
                    p.Insert(0, numbers[i]);
                    result.Add(p);
                }
            }

            return result;
        }
    }
    public class UpperTriangularMatrix : Matrix
    {
        public UpperTriangularMatrix(int order) : base(order, order)
        {
        }

        public override void AddElements(double[] elems)
        {
            int expectedElemsLen = (Width + 1) * Width / 2;

            if (elems.Length != expectedElemsLen)
                throw new Exception(
                    $"Неверное количество элементов. " +
                    $"Ожидалось: {expectedElemsLen}"
                );

            int inputIndex = 0;
            for (int i = 0; i < Height; i++)
                for (int j = i; j < Width; j++)
                    this.elements[i, j] = elems[inputIndex++];
        }

        public override double CalculateDeterminant()
        {         
            double result = 1d;
            for (int i = 0; i < Width; i++)
                result *= elements[i, i];

            return result;
        }
    }

    public class UpperTriangularMatrixLaplace : UpperTriangularMatrix
    {
        public UpperTriangularMatrixLaplace(int order) : base(order)
        {
        }

        public override double CalculateDeterminant()
        {
            return CalculateDeterminantLaplace();
        }
    }
    
    public class UpperTriangularMatrixLeibniz : UpperTriangularMatrix
    {
        public UpperTriangularMatrixLeibniz(int order) : base(order)
        {
        }

        public override double CalculateDeterminant()
        {
            return CalculateDeterminantLeibniz();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.InputEncoding =
                    Console.OutputEncoding =
                        System.Text.Encoding.Unicode;

                List<Matrix> matrices = new List<Matrix>();

                {
                    Matrix matrix = new Matrix(4, 4);
                    matrix.AddElements(new double[16]
                    {
                        1d, -1d,  0d,  0d,
                        4d, -3d,  3d,  1d,
                        0d,  2d,  4d, -6d,
                        5d, -7d, -2d,  0d
                    });

                    matrices.Add(matrix);
                }

                matrices.Add(new IdentityMatrix(4));
                matrices.Add(new IdentityMatrixLaplace(4));


                {
                    double[] upperTriangularMatrixElems = new double[]
                    {
                        7d, -5d, 8d,     1d,
                            4d, -5d,     3d,
                                 9d,     2d,
                                     2d/1d
                    };

                    var m1 = new UpperTriangularMatrix(4);
                    var m2 = new UpperTriangularMatrixLaplace(4);
                    var m3 = new UpperTriangularMatrixLeibniz(4);

                    m1.AddElements(upperTriangularMatrixElems);
                    m2.AddElements(upperTriangularMatrixElems);
                    m3.AddElements(upperTriangularMatrixElems);

                    matrices.Add(m1);
                    matrices.Add(m2);
                    matrices.Add(m3);
                }

                foreach (Matrix m in matrices)
                {
                    Console.WriteLine($"Матрица {m.GetType().Name}:");
                    m.PrintElements();
                    Console.WriteLine($"Определитель: {m.CalculateDeterminant()}");
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            Console.ReadLine();
        }
    }
}
