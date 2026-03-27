namespace BehaviouralPatterns;

// Интерфейс наблюдателя.
public interface IObserver
{
    void Update(float temperature, float humidity, float pressure);
    string Name { get; }
}

// Интерфейс субъекта (наблюдаемого объекта).
public interface ISubject
{
    void RegisterObserver(IObserver observer);
    void RemoveObserver(IObserver observer);
    void NotifyObservers();
}

// Конкретный субъект — метеостанция.
public class WeatherStation : ISubject
{
    private List<IObserver> _observers = new List<IObserver>();
    private float _temperature;
    private float _humidity;
    private float _pressure;

    public void RegisterObserver(IObserver observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
            Console.WriteLine($"[Метеостанция] Подключен наблюдатель: {observer.Name}");
        }
    }

    public void RemoveObserver(IObserver observer)
    {
        if (_observers.Contains(observer))
        {
            _observers.Remove(observer);
            Console.WriteLine($"[Метеостанция] Отключен наблюдатель: {observer.Name}");
        }
    }

    public void NotifyObservers()
    {
        Console.WriteLine($"[Метеостанция] Рассылка уведомлений {_observers.Count} наблюдателям...");
        foreach (IObserver observer in _observers)
            observer.Update(_temperature, _humidity, _pressure);
    }

    // Метод для обновления показаний — автоматически оповещает наблюдателей
    public void SetMeasurements(float temperature, float humidity, float pressure)
    {
        _temperature = temperature;
        _humidity = humidity;
        _pressure = pressure;

        Console.WriteLine($"\n[Метеостанция] Новые показания: температура={_temperature}°C, влажность={_humidity}%, давление={_pressure} мм рт.ст.");
        NotifyObservers();
    }

    public float GetTemperature() => _temperature;
    public float GetHumidity() => _humidity;
    public float GetPressure() => _pressure;
}

// Конкретный наблюдатель — дисплей текущих условий
public class CurrentConditionsDisplay : IObserver
{
    private float _temperature;
    private float _humidity;
    private ISubject _weatherStation;

    public CurrentConditionsDisplay(ISubject weatherStation, string name)
    {
        _weatherStation = weatherStation;
        Name = name;
        _weatherStation.RegisterObserver(this);
    }

    public string Name { get; }

    public void Update(float temperature, float humidity, float pressure)
    {
        _temperature = temperature;
        _humidity = humidity;
        Display();
    }

    public void Display()
    {
        Console.WriteLine($"[{Name}] Текущие условия: {_temperature}°C, влажность {_humidity}%");
    }
}

// Конкретный наблюдатель — дисплей статистики
public class StatisticsDisplay : IObserver
{
    private List<float> _temperatures = new();
    private ISubject _weatherStation;

    public StatisticsDisplay(ISubject weatherStation, string name)
    {
        _weatherStation = weatherStation;
        Name = name;
        _weatherStation.RegisterObserver(this);
    }

    public string Name { get; }

    public void Update(float temperature, float humidity, float pressure)
    {
        _temperatures.Add(temperature);
        Display();
    }

    public void Display()
    {
        float average = _temperatures.Average();
        float max = _temperatures.Max();
        float min = _temperatures.Min();
        Console.WriteLine($"[{Name}] Статистика: средняя={average:F1}°C, макс={max}°C, мин={min}°C (всего измерений: {_temperatures.Count})");
    }
}

// Конкретный наблюдатель — дисплей прогноза (простой алгоритм)
public class ForecastDisplay : IObserver
{
    private float _lastPressure;
    private float _currentPressure;
    private ISubject _weatherStation;

    public ForecastDisplay(ISubject weatherStation, string name)
    {
        _weatherStation = weatherStation;
        Name = name;
        _weatherStation.RegisterObserver(this);
        _lastPressure = 760;
        _currentPressure = 760;
    }

    public string Name { get; }

    public void Update(float temperature, float humidity, float pressure)
    {
        _lastPressure = _currentPressure;
        _currentPressure = pressure;
        Display();
    }

    public void Display()
    {
        string forecast;
        if (_currentPressure > _lastPressure)
        {
            forecast = "Ожидается улучшение погоды! ☀️";
        }
        else if (_currentPressure < _lastPressure)
        {
            forecast = "Возможно ухудшение погоды ⛈️";
        }
        else
        {
            forecast = "Погода без изменений ⛅";
        }
        Console.WriteLine($"[{Name}] Прогноз: {forecast}");
    }
}

// Конкретный наблюдатель — система оповещения о критических условиях
public class AlertSystem : IObserver
{
    private ISubject _weatherStation;

    public AlertSystem(ISubject weatherStation, string name)
    {
        _weatherStation = weatherStation;
        Name = name;
        _weatherStation.RegisterObserver(this);
    }

    public string Name { get; }

    public void Update(float temperature, float humidity, float pressure)
    {
        if (temperature > 35)
        {
            Console.WriteLine($"[{Name}] ⚠️ ВНИМАНИЕ! Экстремальная жара: {temperature}°C!");
        }
        if (humidity > 90)
        {
            Console.WriteLine($"[{Name}] ⚠️ ВНИМАНИЕ! Очень высокая влажность: {humidity}%!");
        }
        if (pressure < 740 || pressure > 780)
        {
            Console.WriteLine($"[{Name}] ⚠️ ВНИМАНИЕ! Критическое давление: {pressure} мм рт.ст.!");
        }
    }
}
