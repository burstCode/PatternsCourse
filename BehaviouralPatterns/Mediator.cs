namespace BehaviouralPatterns;

// Интерфейс посредника.
public interface IAirTrafficControl
{
    void RegisterAircraft(IAircraft aircraft);
    void SendMessage(string message, IAircraft sender);
    void RequestLanding(IAircraft aircraft);
    void NotifyTakeOff(IAircraft aircraft);
}

// Интерфейс коллеги (самолета).
public interface IAircraft
{
    string FlightNumber { get; }
    void ReceiveMessage(string message, string fromFlight);
    void ReceiveLandingPermission(bool granted);
    void ReceiveTakeOffNotification(string flightNumber);
}

// Конкретный посредник — диспетчерская вышка.
public class ControlTower : IAirTrafficControl
{
    private List<IAircraft> _aircrafts = new();
    private Queue<IAircraft> _landingQueue = new();
    private IAircraft? _currentlyLanding = null;
    private bool _runwayAvailable = true;

    public void RegisterAircraft(IAircraft aircraft)
    {
        _aircrafts.Add(aircraft);
        Console.WriteLine($"[Диспетчер] Самолет {aircraft.FlightNumber} зарегистрирован в зоне управления");
    }

    public void SendMessage(string message, IAircraft sender)
    {
        Console.WriteLine($"\n[Диспетчер] Передаю сообщение от {sender.FlightNumber}: \"{message}\"");
        foreach (IAircraft aircraft in _aircrafts)
        {
            if (aircraft != sender)
            {
                aircraft.ReceiveMessage(message, sender.FlightNumber);
            }
        }
    }

    public void RequestLanding(IAircraft aircraft)
    {
        if (_currentlyLanding == aircraft)
        {
            Console.WriteLine($"[Диспетчер] {aircraft.FlightNumber} уже ожидает посадки");
            return;
        }

        if (_runwayAvailable && _currentlyLanding is null)
        {
            _runwayAvailable = false;
            _currentlyLanding = aircraft;
            Console.WriteLine($"[Диспетчер] {aircraft.FlightNumber}, ВПП свободна. Разрешаю посадку!");
            aircraft.ReceiveLandingPermission(true);
        }
        else
        {
            // ВПП занята, ставим в очередь.
            if (!_landingQueue.Contains(aircraft))
            {
                _landingQueue.Enqueue(aircraft);
                Console.WriteLine($"[Диспетчер] {aircraft.FlightNumber}, ВПП занята. Вы поставлены в очередь (позиция {_landingQueue.Count})");
                aircraft.ReceiveLandingPermission(false);
            }
        }
    }

    public void CompleteLanding(IAircraft aircraft)
    {
        if (_currentlyLanding == aircraft)
        {
            Console.WriteLine($"[Диспетчер] {aircraft.FlightNumber} успешно приземлился");
            _currentlyLanding = null;
            _runwayAvailable = true;

            // Проверяем очередь на посадку.
            if (_landingQueue.Count > 0)
            {
                IAircraft nextAircraft = _landingQueue.Dequeue();
                Console.WriteLine($"[Диспетчер] Следующий в очереди: {nextAircraft.FlightNumber}");
                RequestLanding(nextAircraft);
            }
        }
    }

    public void NotifyTakeOff(IAircraft aircraft)
    {
        Console.WriteLine($"[Диспетчер] {aircraft.FlightNumber} взлетает");
        _runwayAvailable = true;

        // Уведомляем все самолеты о взлете.
        foreach (IAircraft a in _aircrafts)
        {
            if (a != aircraft)
            {
                a.ReceiveTakeOffNotification(aircraft.FlightNumber);
            }
        }

        // Проверяем очередь на посадку.
        if (_landingQueue.Count > 0 && _currentlyLanding == null)
        {
            IAircraft nextAircraft = _landingQueue.Dequeue();
            RequestLanding(nextAircraft);
        }
    }
}

// Конкретный коллега — самолет.
public class Aircraft : IAircraft
{
    private string _flightNumber;
    private IAirTrafficControl _controlTower;
    private bool _isLanded = false;

    public Aircraft(string flightNumber, IAirTrafficControl controlTower)
    {
        _flightNumber = flightNumber;
        _controlTower = controlTower;
        _controlTower.RegisterAircraft(this);
    }

    public string FlightNumber => _flightNumber;

    public void SendMessage(string message)
    {
        Console.WriteLine($"[{_flightNumber}] Отправляю сообщение: \"{message}\"");
        _controlTower.SendMessage(message, this);
    }

    public void ReceiveMessage(string message, string fromFlight)
    {
        Console.WriteLine($"[{_flightNumber}] Получено сообщение от {fromFlight}: \"{message}\"");
    }

    public void RequestLanding()
    {
        if (_isLanded)
        {
            Console.WriteLine($"[{_flightNumber}] Я уже на земле!");
            return;
        }

        Console.WriteLine($"[{_flightNumber}] Запрашиваю разрешение на посадку");
        _controlTower.RequestLanding(this);
    }

    public void ReceiveLandingPermission(bool granted)
    {
        if (granted)
        {
            Console.WriteLine($"[{_flightNumber}] Получено разрешение на посадку! Выполняю заход...");
            // Имитация посадки :D
            Thread.Sleep(1000);
            Console.WriteLine($"[{_flightNumber}] Посадка завершена");
            _isLanded = true;
            ((ControlTower)_controlTower).CompleteLanding(this);
        }
        else
        {
            Console.WriteLine($"[{_flightNumber}] Посадка не разрешена. Нахожусь в круге ожидания");
        }
    }

    public void RequestTakeOff()
    {
        if (!_isLanded)
        {
            Console.WriteLine($"[{_flightNumber}] Я еще не на земле!");
            return;
        }
        Console.WriteLine($"[{_flightNumber}] Запрашиваю разрешение на взлет");
        // Диспетчер разрешает взлет сразу.
        ((ControlTower)_controlTower).NotifyTakeOff(this);
        _isLanded = false;
        Console.WriteLine($"[{_flightNumber}] Взлет выполнен, покидаю зону");
    }

    public void ReceiveTakeOffNotification(string flightNumber)
    {
        Console.WriteLine($"[{_flightNumber}] Уведомление: самолет {flightNumber} взлетел, ВПП освободилась");
    }
}
