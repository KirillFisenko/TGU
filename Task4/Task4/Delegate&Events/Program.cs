using System;
using System.Threading;

namespace CountdownTimer
{
    // Объявление делегатов
    public delegate void CountdownDelegate(string timerName);

    public delegate void TaskDelegate(string taskName, int time);

    // Объявление интерфейса ICutDownNotifier с двумя методами
    public interface ICutDownNotifier
    {
        void Init();
        void Run();
    }

    // Класс Timer, который реализует интерфейс ICutDownNotifier
    public class Timer : ICutDownNotifier
    {
        // Инициализация переменных
        private int waitTime;
        private string timerName;
        public event CountdownDelegate Countdown;

        // Конструктор класса
        public Timer(int time, string name)
        {
            waitTime = time;
            timerName = name;
        }

        // Реализация метода Init() интерфейса ICutDownNotifier
        public void Init()
        {
            Countdown += OnCountdownStart;
            Countdown += OnCountdownLeft;
            Countdown += OnCountdownEnd;
        }

        // Реализация метода Run() интерфейса ICutDownNotifier
        public void Run()
        {
            Console.WriteLine($"Timer '{timerName}' started for {waitTime} seconds.");
            Thread.Sleep(waitTime * 1000);
            Countdown?.Invoke(timerName);
        }

        // Методы-обработчики событий Countdown
        private void OnCountdownStart(string name)
        {
            Console.WriteLine($"Countdown started for timer '{name}'.");
        }

        private void OnCountdownLeft(string name)
        {
            for (int i = waitTime; i > 0; i--)
            {
                Thread.Sleep(1000);
                Console.WriteLine($"{i} seconds left for timer '{name}'.");
            }
        }

        private void OnCountdownEnd(string name)
        {
            Console.WriteLine($"Countdown ended for timer '{name}'.");
        }
    }

    // Класс TaskNotifier, который также реализует интерфейс ICutDownNotifier
    public class TaskNotifier : ICutDownNotifier
    {
        // Инициализация переменных
        private string taskName;
        private int taskTime;
        public event TaskDelegate Started;
        public event Action<string, int> Finished;
        private Timer timer;

        // Конструктор класса, в котором еще инициализируется экземпляр класса Timer
        public TaskNotifier(string name, int time, TaskDelegate start, Action<string, int> finish)
        {
            taskName = name;
            taskTime = time;
            Started += start;
            Finished += finish;
            timer = new Timer(taskTime, name);
        }

        // Реализация метода Init() интерфейса ICutDownNotifier
        public void Init()
        {
            timer.Init();
            timer.Countdown += OnTaskCountdown;
        }

        // Реализация метода Run() интерфейса ICutDownNotifier
        public void Run()
        {
            Started?.Invoke(taskName, taskTime);
            timer.Run();
            Finished?.Invoke(taskName, taskTime);
        }

        // Метод-обработчик события Countdown
        private void OnTaskCountdown(string name)
        {
            Console.WriteLine($"Task '{name}' has {taskTime} seconds left.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Инициализация массива интерфейсов ICutDownNotifier
            ICutDownNotifier[] notifiers = new ICutDownNotifier[3];

            // Инициализация делегатов
            CountdownDelegate deleg = new CountdownDelegate((string name) => { Console.WriteLine($"Event received for timer '{name}'."); });
            TaskDelegate started = new TaskDelegate((string name, int time) => { Console.WriteLine($"Task '{name}' started."); });
            Action<string, int> finished = new Action<string, int>((string name, int time) => { Console.WriteLine($"Task '{name}' finished after {time} seconds."); });

            // Инициализация трех элементов массива интерфейсов, два Таймера и одного TaskNotifier
            notifiers[0] = new Timer(5, "Чтение задания");
            notifiers[0].Init();

            notifiers[1] = new TaskNotifier("Выполнение задания", 10, started, finished);
            notifiers[1].Init();

            notifiers[2] = new Timer(3, "Проверка задания перед отправкой");
            notifiers[2].Init();

            // Запуск всех элементов массива интерфейсов ICutDownNotifier
            foreach (var notifier in notifiers)
            {
                notifier.Run();
            }

            Console.ReadLine();
        }
    }
}