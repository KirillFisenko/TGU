using System;
using System.Threading;

namespace CountdownTimer
{
    // Объявление делегатов
    public delegate void CountdownDelegate(string timerName);

    public delegate void TaskDelegate(string taskName, int time);

    // Объявление интерфейса ICutDownNotifier
    public interface ICutDownNotifier
    {
        void Init();
        void Run();
    }

    // Класс Timer, который реализует интерфейс ICutDownNotifier
    public class Timer : ICutDownNotifier
    {
        // Поля
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
            Console.WriteLine($"Таймер '{timerName}' запущен на {waitTime} сек.");
            Thread.Sleep(waitTime * 1000);
            Countdown?.Invoke(timerName);
        }

        // Методы-обработчики событий Countdown
        private void OnCountdownStart(string name)
        {
            Console.WriteLine($"Начался обратный отсчет '{name}'.");
        }

        private void OnCountdownLeft(string name)
        {
            for (int i = waitTime; i > 0; i--)
            {
                Thread.Sleep(1000);
                Console.WriteLine($"{i} сек. осталось на '{name}'.");
            }
        }

        private void OnCountdownEnd(string name)

        {
            Console.WriteLine($"Обратный отсчет '{name}' закончился.");
        }
    }

    // Класс TaskNotifier, который также реализует интерфейс ICutDownNotifier
    public class TaskNotifier : ICutDownNotifier
    {
        // Поля
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
            Console.WriteLine($"У задачи '{name}' осталось {taskTime} сек.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Инициализация массива интерфейсов ICutDownNotifier
            ICutDownNotifier[] notifiers = new ICutDownNotifier[3];

            // Инициализация делегатов
            CountdownDelegate deleg = new CountdownDelegate((string name) => { Console.WriteLine($"Событие, полученное для таймера '{name}'."); });
            TaskDelegate started = new TaskDelegate((string name, int time) => { Console.WriteLine($"Задача '{name}' начата."); });
            Action<string, int> finished = new Action<string, int>((string name, int time) => { Console.WriteLine($"Задача '{name}' закончена после {time} сек."); });

            // Инициализация трех элементов массива интерфейсов
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
                Console.WriteLine();
            }

            Console.ReadLine();
        }
    }
}