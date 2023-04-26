using System;
using System.Threading;

namespace Delegates_Events
{
    // Определяем делегат для событий таймера
    public delegate void TimerEventHandler(object sender, TimerEventArgs args);

    // Определяем класс аргументов для событий таймера
    public class TimerEventArgs : EventArgs
    {
        public string TimerName { get; set; } // Имя таймера
        public int SecondsLeft { get; set; } // Сколько секунд осталось
        public string EventSource { get; set; } // Источник события

        public TimerEventArgs(string timerName, int secondsLeft, string eventSource)
        {
            TimerName = timerName;
            SecondsLeft = secondsLeft;
            EventSource = eventSource;
        }
    }

    // Определяем класс для имитации таймера с обратным отсчетом
    public class CountdownNotifier
    {
        private readonly int _secondsToWait; // Количество секунд ожидания проставляется пользователем
        private readonly string _timerName; // Имя таймера

        public CountdownNotifier(int secondsToWait, string timerName)
        {
            _secondsToWait = secondsToWait;
            _timerName = timerName;
        }

        // Определяем события таймера
        public event TimerEventHandler CountdownFinished; // Событие "обратный отсчет завершён"
        public event TimerEventHandler CountdownTick; // Событие "осталось N секунд"

        // Метод инициализации таймера (ожидание)
        public void Init()
        {
            Console.WriteLine($"Timer '{_timerName}' started."); // Выводим сообщение о начале работы таймера
            Thread.Sleep(_secondsToWait * 1000); // Имитируем ожидание в течение заданного количества секунд
            Console.WriteLine($"Timer '{_timerName}' finished."); // Выводим сообщение о завершении работы таймера
            CountdownFinished?.Invoke(this, new TimerEventArgs(_timerName, 0, "CountdownFinished")); // Вызываем событие "обратный отсчет завершён"
        }

        // Метод запуска таймера (обратный отсчет)
        public void Run()
        {
            Console.WriteLine($"Timer '{_timerName}' started."); // Выводим сообщение о начале работы таймера
            for (int i = _secondsToWait; i >= 0; i--)
            {
                Thread.Sleep(1000); // Имитируем ожидание в течение одной секунды
                Console.WriteLine($"Timer '{_timerName}' ticked. {_secondsToWait - i} seconds left."); // Выводим сообщение о количестве оставшихся секунд
                CountdownTick?.Invoke(this, new TimerEventArgs(_timerName, _secondsToWait - i, "CountdownTick")); // Вызываем событие "осталось N секунд"
            }
            Console.WriteLine($"Timer '{_timerName}' finished."); // Выводим сообщение о завершении работы таймера
            CountdownFinished?.Invoke(this, new TimerEventArgs(_timerName, 0, "CountdownFinished")); // Вызываем событие "обратный отсчет завершён"
        }
    }

    // Главный класс: 
    internal class Program
    {
        static void Main(string[] args)
        {
            var taskStartedDelegate = new Action<string, int>((taskName, time) =>
                Console.WriteLine($"Task '{taskName}' started. Time allocated: {time} seconds."));
            var taskFinishedDelegate = new Action<string, int>((taskName, time) =>
                Console.WriteLine($"Task '{taskName}' finished. Time spent: {time} seconds."));

            var notifier1 = new CountdownNotifier(5, "notifier1");
            notifier1.CountdownFinished += (sender, args) =>
                taskFinishedDelegate.Invoke(args.TimerName, notifier1._secondsToWait);
            notifier1.CountdownTick += (sender, args) =>
                Console.WriteLine($"Seconds left for timer '{args.TimerName}': {args.SecondsLeft}");

            var notifier2 = new CountdownNotifier(10, "notifier2");
            notifier2.CountdownFinished += (sender, args) =>
                taskFinishedDelegate.Invoke(args.TimerName, notifier2._secondsToWait);
            notifier2.CountdownTick += (sender, args) =>
                Console.WriteLine($"Seconds left for timer '{args.TimerName}': {args.SecondsLeft}");

            var notifier3 = new CountdownNotifier(15, "notifier3");
            notifier3.CountdownFinished += (sender, args) =>
                taskFinishedDelegate.Invoke(args.TimerName, notifier3._secondsToWait);
            notifier3.CountdownTick += (sender, args) =>
                Console.WriteLine($"Seconds left for timer '{args.TimerName}': {args.SecondsLeft}");

            notifier1.Init();
            notifier2.Run();
            notifier3.Run();

            Console.ReadKey();
        }
    }
}