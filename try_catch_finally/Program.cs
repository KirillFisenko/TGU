using System;

namespace TGU_lesson1
{
    public class Matrix
    {
        // Поле генерации случайных чисел
        public static Random random = new Random();

        // Поле для хранения данных матрицы
        public int[,] data;

        // Свойство для получения количества строк матрицы
        public int Rows { get; }

        // Свойство для получения количества столбцов матрицы
        public int Columns { get; }

        // Конструктор для создания матрицы заданного размера
        public Matrix(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            data = new int[rows, columns];
        }

        // Индексатор для доступа к элементам матрицы по индексам строки и столбца
        public int this[int i, int j]
        {
            get
            {
                return data[i, j];
            }
            set
            {
                data[i, j] = value;
            }
        }

        // Метод для сложения двух матриц с проверкой возможности операции и генерацией исключения в случае ошибки
        public static Matrix Add(Matrix a, Matrix b)
        {
            if (a.Rows != b.Rows || a.Columns != b.Columns)
            {
                throw new MatrixException("Невозможно сложить две матрицы разных размеров", a.Rows, a.Columns, b.Rows, b.Columns);
            }

            Matrix result = new Matrix(a.Rows, a.Columns);

            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Columns; j++)
                {
                    result[i, j] = a[i, j] + b[i, j];
                }
            }
            return result;
        }

        // Метод для вычитания двух матриц с проверкой возможности операции и генерацией исключения в случае ошибки
        public static Matrix Subtract(Matrix a, Matrix b)
        {
            if (a.Rows != b.Rows || a.Columns != b.Columns)
            {
                throw new MatrixException("Невозможно вычесть две матрицы разных размеров", a.Rows, a.Columns, b.Rows, b.Columns);
            }

            Matrix result = new Matrix(a.Rows, a.Columns);

            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Columns; j++)
                {
                    result[i, j] = a[i, j] - b[i, j];
                }
            }
            return result;
        }

        // Метод для умножения двух матриц с проверкой возможности операции и генерацией исключения в случае ошибки
        public static Matrix Multiply(Matrix a, Matrix b)
        {
            if (a.Columns != b.Rows)
            {
                throw new MatrixException("Невозможно умножить две матрицы несовместимых размеров", a.Rows, a.Columns, b.Rows, b.Columns);
            }

            Matrix result = new Matrix(a.Rows, b.Columns);

            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < b.Columns; j++)
                {
                    for (int k = 0; k < a.Columns; k++)
                    {
                        result[i, j] += a[i, k] * b[k, j];
                    }
                }
            }
            return result;
        }

        // Метод создания новой матрицы с заданными размерами и заполненой нулями, на мой вгляд он здесь лишний
        public static Matrix GetEmpty(int rows, int columns)
        {
            Matrix result = new Matrix(rows, columns);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    result[i, j] = 0;
                }
            }
            return result;
        }

        // Метод создания новой матрицы матрицы со случайными числами от 0 до 9
        public static Matrix GetMatrixRandomNumbers(int rows, int columns)
        {            
            Matrix result = new Matrix(rows, columns);            

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {                    
                    result[i, j] = random.Next(10);
                }
            }
            return result;
        }             

        // Метод для вывода на консоль матрицы
        public void Print()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Console.Write(this[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }    

    // Создание нового класса исключения, который будет наследоваться от базового класса
    public class MatrixException : Exception
    {
        public int Rows1 { get; }
        public int Columns1 { get; }
        public int Rows2 { get; }
        public int Columns2 { get; }

        public MatrixException(int rows1, int columns1, int rows2, int columns2)
        {
            Rows1 = rows1;
            Columns1 = columns1;
            Rows2 = rows2;
            Columns2 = columns2;
        }

        public MatrixException(string message, int rows1, int columns1, int rows2, int columns2) : base(message)
        {
            Rows1 = rows1;
            Columns1 = columns1;
            Rows2 = rows2;
            Columns2 = columns2;
        }

        public MatrixException(string message, Exception innerException, int rows1, int columns1, int rows2, int columns2) : base(message, innerException)
        {
            Rows1 = rows1;
            Columns1 = columns1;
            Rows2 = rows2;
            Columns2 = columns2;
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            // Создаем матрицы и заполняем их
            Matrix matrix1 = Matrix.GetMatrixRandomNumbers(4, 4);            
            Matrix matrix2 = Matrix.GetMatrixRandomNumbers(5, 6);                                 

            // Выводим матрицы на консоль
            Console.WriteLine("Матрица 1:");
            matrix1.Print();
            Console.WriteLine("Матрица 2:");
            matrix2.Print();                      

            try
            {   
                // Пытаемся сложить матрицы и вывести результат на консоль
                Console.WriteLine("Матрица 1 + Матрица 2:");
                Matrix resultAdd = Matrix.Add(matrix1, matrix2);
                resultAdd.Print();
            }
            catch (MatrixException e) // Перехватываем исключение при работе с матрицами
            {
                // Выводим сообщение об ошибке и размеры матриц
                Console.WriteLine(e.Message);
                Console.WriteLine($"Размер первой матрицы: {e.Rows1} x {e.Columns1}");
                Console.WriteLine($"Размер второй матрицы: {e.Rows2} x {e.Columns2}");
            }
            Console.WriteLine();
            
            try
            {
                // Пытаемся вычесть матрицы и вывести результат на консоль
                Console.WriteLine("Матрица 1 - Матрица 2:");
                Matrix resultSubtract = Matrix.Subtract(matrix1, matrix2);
                resultSubtract.Print();
            }
            catch (MatrixException e) // Перехватываем исключение при работе с матрицами
            {
                // Выводим сообщение об ошибке и размеры матриц
                Console.WriteLine(e.Message);
                Console.WriteLine($"Размер первой матрицы: {e.Rows1} x {e.Columns1}");
                Console.WriteLine($"Размер второй матрицы: {e.Rows2} x {e.Columns2}");
            }
            Console.WriteLine();
            
            try
            {
                // Пытаемся умножить матрицы и вывести результат на консоль
                Console.WriteLine("Матрица 1 * Матрица 2:");
                Matrix resultMultiply = Matrix.Multiply(matrix1, matrix2);
                resultMultiply.Print();
            }
            catch (MatrixException e) // Перехватываем исключение при работе с матрицами
            {
                // Выводим сообщение об ошибке и размеры матриц
                Console.WriteLine(e.Message);
                Console.WriteLine($"Размер первой матрицы: {e.Rows1} x {e.Columns1}");
                Console.WriteLine($"Размер второй матрицы: {e.Rows2} x {e.Columns2}");
            }
            
            finally // Выполняем блок независимо от наличия исключения
            {
                // Освобождаем ресурсы, занятые матрицами
                matrix1 = null;
                matrix2 = null;               
                Console.WriteLine();                
                Console.WriteLine("Программа завершила работу.");
            }
            Console.ReadLine();
        }
    }
}