using System;

namespace Delegates_Events
{
    // Класс "Таймер":
    public class Timer
    {
        // Поля:
        public int secondsLeft; // количество секунд ожидания
        public string name; // имя таймера

        // События:
        public event EventHandler<string> TimerStarted; // событие начала обратного отсчета, передает имя таймера
        public event EventHandler<Tuple<string, int>> TimeLeft; // событие, передающее количество секунд, оставшихся до завершения таймера, и имя таймера
        public event EventHandler<string> TimerEnded; // событие завершения обратного отсчета, передает имя таймера 

        // Конструкторы:
        public Timer(int seconds, string name)
        {
            secondsLeft = seconds;
            this.name = name;
        }
        public Timer(string name)
        {
            this.name = name;
        }

        // Методы:                                               
        public void Start() // запуск таймера
        {
            // Запуск таймера с использованием System.Timers.Timer
            System.Timers.Timer timer = new System.Timers.Timer(1000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();

            OnTimerStarted(name); // вызов события начала обратного отсчета
        }

        // Метод для обработки события срабатывания таймера
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            secondsLeft--; // уменьшение количества секунд на единицу
            OnTimeLeft(name, secondsLeft); // вызов события, передающего количество оставшихся секунд
            if (secondsLeft == 0)
            {
                OnTimerEnded(name); // вызов события завершения обратного отсчета
                (sender as System.Timers.Timer).Stop(); // остановка таймера
            }
        }

        // Метод для вызова события начала обратного отсчета
        protected virtual void OnTimerStarted(string name)
        {
            TimerStarted?.Invoke(this, name);
        }

        // Метод для вызова события, передающего количество оставшихся секунд
        protected virtual void OnTimeLeft(string name, int secondsLeft)
        {
            TimeLeft?.Invoke(this, new Tuple<string, int>(name, secondsLeft));
        }

        // Метод для вызова события завершения обратного отсчета
        protected virtual void OnTimerEnded(string name)
        {
            TimerEnded?.Invoke(this, name);
        }
    }

    // Интерфейс:
    public interface ICutDownNotifier
    {
        // Методы:
        void Init(Timer timer); // подписывается на события таймера
        void Run(); // запускает таймер

    }

    // Класс "CutDownNotifier":
    public class CutDownNotifier : ICutDownNotifier // Реализует интерфейс ICutDownNotifier
    {
        // Поля:
        public Action<string, int> StartedTask; // делегат, вызываемый при начале задачи, передает название задачи и сколько было отведено времени
        public Action<string, int> TaskFinished; // делегат, вызываемый при завершении задачи, передает название задачи и сколько было отведено времени

        // Конструктор:
        public CutDownNotifier(Action<string, int> startedTask, Action<string, int> taskFinished)
        {
            StartedTask = startedTask;
            TaskFinished = taskFinished;
        }

        // Методы:
        public void Init(Timer timer) // подписывается на события таймера и делает вызов делегата StartedTask
        {
            timer.TimerStarted += Timer_TimerStarted;
            timer.TimeLeft += Timer_TimeLeft;
        }

        // Обработчик события начала обратного отсчета
        private void Timer_TimerStarted(object sender, string name)
        {
            StartedTask?.Invoke(name, ((Timer)sender).secondsLeft);
        }

        // Обработчик события, передающего количество оставшихся секунд
        private void Timer_TimeLeft(object sender, Tuple<string, int> e)
        {
            TaskFinished?.Invoke(e.Item1, e.Item2);
        }

        public void Run() // Запускает таймер 
        {
            Console.WriteLine("Введите количество секунд для таймера: ");
            int seconds = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Введите название задачи: ");
            string name = Console.ReadLine();

            Timer timer = new Timer(seconds, name);
            Init(timer);
            timer.Start();

            Console.WriteLine("Нажмите любую кнопку для завершения таймера");
            Console.ReadKey();
            TaskFinished?.Invoke(name, (seconds - timer.secondsLeft)); // вызов делегата TaskFinished 
        }
    }

    // Главный класс: 
    internal class Program
    {
        static void Main(string[] args)
        {
            CutDownNotifier cutDownNotifier = new CutDownNotifier((name, seconds) =>
            {
                Console.WriteLine($"Задача {name} начата, отведено времени: {seconds} секунд");
            },
            (name, seconds) =>
            {
                Console.WriteLine($"Задача {name} завершена, использовано времени: {seconds} секунд");
            });

            cutDownNotifier.Run();
        }
    }
}