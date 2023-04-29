using System;
using System.Diagnostics;
using System.Linq;

namespace LINQ
{
    // ститический класс для расширяющих методов
    public static class Extensions
    {
        // расширяющий метод, который определяет сумму элементов массива
        public static int SumArray(this int[] array)
        {            
            return array.Sum();
        }

        // расширяющий метод, который определяет, является ли строка положительным целым числом
        public static bool IsPositiveInt(this string str)
        {
            return str.All(char.IsDigit) && str.Length > 0 && str[0] != '0' && str[0] != '-';
        }

        // поиск всех положительных элементов в массиве в виде метода, реализующего поиск напрямую
        public static int[] FindPositiveNumbers(this int[] numbers)
        {
            int count = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] > 0)
                {
                    count++;
                }
            }
            int[] result = new int[count];
            int index = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] > 0)
                {
                    result[index] = numbers[i];
                    index++;
                }
            }
            return result;
        }

        // поиск всех положительных элементов в массиве в виде метода, которому условие поиска передается через делегат
        public static int[] FindPositiveNumbersUsingDelegate(this int[] numbers, Func<int, bool> condition)
        {
            int count = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                if (condition(numbers[i]))
                {
                    count++;
                }
            }
            int[] result = new int[count];
            int index = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                if (condition(numbers[i]))
                {
                    result[index] = numbers[i];
                    index++;
                }
            }
            return result;
        }

        // поиск всех положительных элементов в массиве в виде метода, которому условие поиска передается через делегат в виде анонимного метода
        public static int[] FindPositiveNumbersUsingAnonymousMethod(this int[] numbers)
        {
            return FindPositiveNumbersUsingDelegate(numbers, delegate (int number) { return number > 0; });
        }

        // поиск всех положительных элементов в массиве в виде метода, которому условие поиска передается через делегат в виде лямбда-выражения
        public static int[] FindPositiveNumbersUsingLambdaExpression(this int[] numbers)
        {
            return FindPositiveNumbersUsingDelegate(numbers, number => number > 0);
        }

        // поиск всех положительных элементов в массиве в виде LINQ-выражения
        public static int[] FindPositiveNumbersUsingLinq(this int[] numbers)
        {
            return numbers.Where(number => number > 0).ToArray();
        }
    }

    public class Program
    {        
        static void Main(string[] args)
        {
            // Задание 1
            // Определяем сумму элементов массива расширяющим методом
            int[] array1 = { 1, 2, 3, 4, 5 };
            int sum = array1.SumArray();
            Console.WriteLine($"Сумма элементов массива {array1}: {sum}");            

            // Задание 2
            // Определяем, является ли строка целым положительным числом
            string str = "123";
            bool isPositiveInt = str.IsPositiveInt();
            Console.WriteLine($"Строка {str} является положительным целым числом: {isPositiveInt}");            

            // Задание 3
            // Создаем большой массив
            int[] bigArray = new int[1000000];

            // Заполнение массива случайными числами
            Random random = new Random();
            for (int i = 0; i < bigArray.Length; i++)
            {
                bigArray[i] = random.Next(-1000000, 1000000);
            }

            int count = 100; //количество измерений

            // Измерение времени выполнения метода FindPositiveNumbers()
            long[] measurements1 = new long[count];            
            for (int i = 0; i < measurements1.Length; i++)
            {
                Stopwatch stopwatch1 = new Stopwatch();                
                stopwatch1.Start();
                var numbers1 = bigArray.FindPositiveNumbers();                
                stopwatch1.Stop();
                measurements1[i] = stopwatch1.ElapsedMilliseconds;                              
            }
            Array.Sort(measurements1);
            long measurements1Middle = measurements1[measurements1.Length / 2];
            Console.WriteLine($"Время выполнения метода FindPositiveNumbers(): {measurements1Middle} мс");


            // Измерение времени выполнения метода FindPositiveNumbersUsingDelegate() с использованием делегата
            var measurements2 = new int[count];
            for (int i = 0; i < measurements2.Length; i++)
            {
                Stopwatch stopwatch2 = new Stopwatch();                
                stopwatch2.Start();
                var numbers2 = bigArray.FindPositiveNumbersUsingDelegate(delegate (int x) { return x > 0; });
                stopwatch2.Stop();
                measurements2[i] = (int)stopwatch2.ElapsedMilliseconds;                
            }
            Array.Sort(measurements2);
            var measurements2Middle = measurements2[measurements2.Length / 2];
            Console.WriteLine($"Время выполнения метода FindPositiveNumbersUsingDelegate() с использованием делегата: {measurements2Middle} мс");


            // Измерение времени выполнения метода FindPositiveNumbersUsingAnonymousMethod() с использованием анонимного метода            
            var measurements3 = new int[count];
            for (int i = 0; i < measurements3.Length; i++)
            {
                Stopwatch stopwatch3 = new Stopwatch();                
                stopwatch3.Start();
                var numbers3 = bigArray.FindPositiveNumbersUsingAnonymousMethod();
                stopwatch3.Stop();
                measurements3[i] = (int)stopwatch3.ElapsedMilliseconds;                
            }
            Array.Sort(measurements3);
            var measurements3Middle = measurements3[measurements3.Length / 2];
            Console.WriteLine($"Время выполнения метода FindPositiveNumbersUsingAnonymousMethod() с использованием анонимного метода: {measurements3Middle} мс");


            // Измерение времени выполнения метода FindPositiveNumbersUsingLambdaExpression() с использованием лямбда-выражения
            var measurements4 = new int[count];
            for (int i = 0; i < measurements4.Length; i++)
            {
                Stopwatch stopwatch4 = new Stopwatch();                
                stopwatch4.Start();
                var numbers4 = bigArray.FindPositiveNumbersUsingLambdaExpression();
                stopwatch4.Stop();
                measurements4[i] = (int)stopwatch4.ElapsedMilliseconds;                
            }
            Array.Sort(measurements4);
            var measurements4Middle = measurements4[measurements4.Length / 2];
            Console.WriteLine($"Время выполнения метода FindPositiveNumbersUsingLambdaExpression() с использованием лямбда-выражения: {measurements4Middle} мс");


            // Измерение времени выполнения метода FindPositiveNumbersUsingLinq() с использованием LINQ
            var measurements5 = new int[count];
            for (int i = 0; i < measurements5.Length; i++)
            {
                Stopwatch stopwatch5 = new Stopwatch();
                stopwatch5.Start();
                var numbers5 = bigArray.FindPositiveNumbersUsingLinq();
                stopwatch5.Stop();
                measurements5[i] = (int)stopwatch5.ElapsedMilliseconds;
            }
            Array.Sort(measurements5);
            var measurements5Middle = measurements5[measurements5.Length / 2];
            Console.WriteLine($"Время выполнения метода FindPositiveNumbersUsingLinq() с использованием LINQ: {measurements5Middle} мс");
            
            Console.ReadLine();
        }
    }
}