using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LINQ
{
    public static class Extensions
    {
        // расширяющий метод, который определяет сумму элементов массива
        public static int Sum(this int[] array)
        {
            var sum = 0;
            foreach (int number in array)
            {
                sum += number;
            }
            return sum;
        }

        // расширяющий метод, который определяет, является ли строка положительным целым числом
        public static bool IsPositiveInt(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }

            foreach (var symbol in str)
            {
                if (!char.IsDigit(symbol))
                {
                    return false;
                }
            }

            return true;
        }

        // поиск всех положительных элементов в массиве
        public static IEnumerable<int> FindPositiveNumbers(this int[] array)
        {
            return from number in array
                   where number > 0
                   select number;
        }

        public static IEnumerable<int> FindNumbers(this int[] array, Func<int, bool> predicate)
        {
            return from number in array
                   where predicate(number)
                   select number;
        }
    }

    internal class Program
    {
        public static Random random = new Random();
        static void Main(string[] args)
        {
            // Задание 1
            // Определяем сумму элементов массива расширяющим методом
            int[] array1 = { 1, 2, 3, 4, 5 };
            int sum = array1.Sum();
            Console.WriteLine($"Сумма элементов массива {array1}: {sum}");
            Console.WriteLine();

            // Задание 2
            // Определяем, является ли строка целым положительным числом
            string str = "123";
            bool isPositiveInt = str.IsPositiveInt();
            Console.WriteLine($"Строка {str} является положительным целым числом: {isPositiveInt}");
            Console.WriteLine();

            // Задание 3
            // Создаем большой массив
            int[] bigArray = new int[100000];

            // Заполнение массива случайными числами
            
            for (int i = 0; i < bigArray.Length; i++)
            {
                bigArray[i] = random.Next(-1000000, 1000000);
            }

            var count = 10; //количество измерений

            // Измерение времени выполнения метода FindPositiveNumbers()
            var measurements1 = new int[count];
            
            for (int i = 0; i < measurements1.Length; i++)
            {
                Stopwatch stopwatch1 = new Stopwatch();                
                stopwatch1.Start();
                var numbers1 = bigArray.FindPositiveNumbers();
                stopwatch1.Stop();
                measurements1[i] = (int)stopwatch1.ElapsedMilliseconds;                
            }
            Array.Sort(measurements1);
            var measurements1Middle = measurements1[measurements1.Length / 2];
            Console.WriteLine($"Время выполнения метода FindPositiveNumbers(): {measurements1Middle} мс");
            Console.WriteLine();

            // Измерение времени выполнения метода FindNumbers() с использованием делегата
            var measurements2 = new int[count];
            for (int i = 0; i < measurements2.Length; i++)
            {
                Stopwatch stopwatch2 = new Stopwatch();                
                stopwatch2.Start();
                var numbers2 = bigArray.FindNumbers(delegate (int x) { return x > 0; });
                stopwatch2.Stop();
                measurements2[i] = (int)stopwatch2.ElapsedMilliseconds;                
            }
            Array.Sort(measurements2);
            var measurements2Middle = measurements2[measurements2.Length / 2];
            Console.WriteLine($"Время выполнения метода FindPositiveNumbers() с использованием делегата: {measurements2Middle} мс");
            Console.WriteLine();

            // Измерение времени выполнения метода FindNumbers() с использованием анонимного метода            
            var measurements3 = new int[count];
            for (int i = 0; i < measurements3.Length; i++)
            {
                Stopwatch stopwatch3 = new Stopwatch();                
                stopwatch3.Start();
                var numbers3 = bigArray.FindNumbers((x) => x > 0);
                stopwatch3.Stop();
                measurements3[i] = (int)stopwatch3.ElapsedMilliseconds;                
            }
            Array.Sort(measurements3);
            var measurements3Middle = measurements3[measurements3.Length / 2];
            Console.WriteLine($"Время выполнения метода FindPositiveNumbers() с использованием анонимного метода: {measurements3Middle} мс");
            Console.WriteLine();

            // Измерение времени выполнения метода FindNumbers() с использованием лямбда-выражения
            var measurements4 = new int[count];
            for (int i = 0; i < measurements4.Length; i++)
            {
                Stopwatch stopwatch4 = new Stopwatch();                
                stopwatch4.Start();
                var numbers4 = bigArray.FindNumbers(x => x > 0);
                stopwatch4.Stop();
                measurements4[i] = (int)stopwatch4.ElapsedMilliseconds;                
            }
            Array.Sort(measurements4);
            var measurements4Middle = measurements4[measurements4.Length / 2];
            Console.WriteLine($"Время выполнения метода FindPositiveNumbers() с использованием лямбда-выражения: {measurements4Middle} мс");
            Console.WriteLine();

            Console.ReadLine();
        }
    }
}