using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LINQ
{
    public static class Extensions
    {
        public static int Sum(this int[] array)
        {
            int sum = 0;
            foreach (int number in array)
            {
                sum += number;
            }
            return sum;
        }

        public static bool IsPositiveInteger(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            foreach (char c in str)
                if (!char.IsDigit(c))
                    return false;

            return true;
        }

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

    //public static class ArrayExtensions
    //{
    //    // расширяющий метод, который определяет сумму элементов массива
    //    public static int Sum(this int[] array)
    //    {
    //        int sum = 0;
    //        for (int i = 0; i < array.Length; i++)
    //        {
    //            sum += array[i];
    //        }
    //        return sum;
    //    }

    //    // методы поиска элемента в массиве
    //    public static IEnumerable<int> FindPositiveNumbers(this int[] array)
    //    {
    //        for (int i = 0; i < array.Length; i++)
    //        {
    //            if (array[i] > 0)
    //            {
    //                yield return array[i];
    //            }
    //        }
    //    }

    //    public static IEnumerable<int> FindNumbers(this int[] array, Func<int, bool> predicate)
    //    {
    //        for (int i = 0; i < array.Length; i++)
    //        {
    //            if (predicate(array[i]))
    //            {
    //                yield return array[i];
    //            }
    //        }
    //    }

    //    public static IEnumerable<int> FindNumbers1(this int[] array, Func<int, bool> predicate)
    //    {
    //        for (int i = 0; i < array.Length; i++)
    //        {
    //            if (predicate(array[i]))
    //            {
    //                yield return array[i];
    //            }
    //        }
    //    }

    //    public static IEnumerable<int> FindNumbers2(this int[] array, Func<int, bool> predicate)
    //    {
    //        for (int i = 0; i < array.Length; i++)
    //        {
    //            if (predicate(array[i]))
    //            {
    //                yield return array[i];
    //            }
    //        }
    //    }

    //    public static IEnumerable<int> FindNumbers3(this int[] array, Func<int, bool> predicate)
    //    {
    //        return array.Where(predicate);
    //    }

    //}


    //// расширяющий метод, который определяет, является ли строка положительным целым числом
    //public static class StringExtensions
    //{
    //    public static bool IsPositiveInteger(this string str)
    //    {
    //        if (string.IsNullOrEmpty(str))
    //        {
    //            return false;
    //        }

    //        foreach (char c in str)
    //        {
    //            if (!char.IsDigit(c))
    //            {
    //                return false;
    //            }
    //        }

    //        return true;
    //    }
    //}


    internal class Program
    {
        static void Main(string[] args)
        {
            // Задание 1
            int[] array = { 1, 2, 3, 4, 5 };
            int sum = array.Sum();
            Console.WriteLine($"Сумма элементов массива: {sum}");

            // Задание 2
            string str = "123";
            bool isPositiveInteger = str.IsPositiveInteger();
            Console.WriteLine($"Строка является положительным целым числом: {isPositiveInteger}");

            // Задание 3
            int[] array2 = { -1, 2, -3, 4, -5 };
            IEnumerable<int> positiveNumbers = array2.FindPositiveNumbers();
            Console.WriteLine("Положительные числа в массиве:");
            foreach (int number in positiveNumbers)
            {
                Console.WriteLine(number);
            }

            IEnumerable<int> numbers = array2.FindNumbers(x => x > 0);
            Console.WriteLine("Числа больше нуля в массиве:");
            foreach (int number in numbers)
            {
                Console.WriteLine(number);
            }

            IEnumerable<int> numbers2 = array2.FindNumbers(delegate (int x) { return x > 0; });
            Console.WriteLine("Числа больше нуля в массиве:");
            foreach (int number in numbers2)
            {
                Console.WriteLine(number);
            }

            IEnumerable<int> numbers3 = array2.FindNumbers((x) => x > 0);
            Console.WriteLine("Числа больше нуля в массиве:");
            foreach (int number in numbers3)
            {
                Console.WriteLine(number);
            }

            IEnumerable<int> numbers4 = array2.FindNumbers(x => x > 0);
            Console.WriteLine("Числа больше нуля в массиве:");
            foreach (int number in numbers4)
            {
                Console.WriteLine(number);
            }

            IEnumerable<int> numbers5 = from number in array2
                                        where number > 0
                                        select number;
            Console.WriteLine("Числа больше нуля в массиве:");
            foreach (int number in numbers5)
            {
                Console.WriteLine(number);
            }

            // Сравнение скорости выполнения вычислений
            int[] bigArray = new int[100000];

            // Заполнение массива случайными числами
            Random random = new Random();
            for (int i = 0; i < bigArray.Length; i++)
            {
                bigArray[i] = random.Next(-1000000, 1000000);
            }

            // Измерение времени выполнения метода FindPositiveNumbers()
            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();
            for (int i = 0; i < 100; i++)
            {
                IEnumerable<int> positiveNumbers2 = bigArray.FindPositiveNumbers();
                foreach (int number in positiveNumbers2) { }
            }
            stopwatch1.Stop();
            Console.WriteLine($"Время выполнения метода FindPositiveNumbers(): {stopwatch1.ElapsedMilliseconds} мс");

            // Измерение времени выполнения метода FindNumbers() с использованием делегата
            Stopwatch stopwatch2 = new Stopwatch();
            stopwatch2.Start();
            for (int i = 0; i < 100; i++)
            {
                IEnumerable<int> numbers6 = bigArray.FindNumbers(delegate (int x) { return x > 0; });
                foreach (int number in numbers6) { }
            }
            stopwatch2.Stop();
            Console.WriteLine($"Время выполнения метода FindNumbers() с использованием делегата: {stopwatch2.ElapsedMilliseconds} мс");

            // Измерение времени выполнения метода FindNumbers() с использованием анонимного метода
            Stopwatch stopwatch3 = new Stopwatch();
            stopwatch3.Start();
            for (int i = 0; i < 100; i++)
            {
                IEnumerable<int> numbers7 = bigArray.FindNumbers((x) => x > 0);
                foreach (int number in numbers7) { }
            }
            stopwatch3.Stop();
            Console.WriteLine($"Время выполнения метода FindNumbers() с использованием анонимного метода: {stopwatch3.ElapsedMilliseconds} мс");

            // Измерение времени выполнения метода FindNumbers() с использованием лямбда-выражения
            Stopwatch stopwatch4 = new Stopwatch();
            stopwatch4.Start();
            for (int i = 0; i < 100; i++)
            {
                IEnumerable<int> numbers8 = bigArray.FindNumbers(x => x > 0);
                foreach (int number in numbers8) { }
            }
            stopwatch4.Stop();
            Console.WriteLine($"Время выполнения метода FindNumbers() с использованием лямбда-выражения: {stopwatch4.ElapsedMilliseconds} мс");
            Console.ReadLine();
        }
    }
}