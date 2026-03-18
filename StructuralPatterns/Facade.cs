namespace StructuralPatterns;

// Некоторые подсистемы (подразумевается, что у каждой из них свой интерфейс):
public class LightingSystem
{
    public void TurnOnLights()
    {
        Console.WriteLine("[Свет] Включено освещение во всех комнатах");
    }

    public void TurnOffLights()
    {
        Console.WriteLine("[Свет] Выключено освещение");
    }

    public void SetDimLevel(int percent)
    {
        Console.WriteLine($"[Свет] Яркость установлена на {percent}%");
    }
}

public class ClimateSystem
{
    private double _currentTemperature = 22.0;

    public void SetTemperature(double temperature)
    {
        _currentTemperature = temperature;
        Console.WriteLine($"[Климат] Температура установлена на {temperature}°C");
    }

    public void TurnOnAirConditioner()
    {
        Console.WriteLine("[Климат] Кондиционер включен");
    }

    public void TurnOffAirConditioner()
    {
        Console.WriteLine("[Климат] Кондиционер выключен");
    }

    public void SetFanSpeed(int speed)
    {
        Console.WriteLine($"[Климат] Скорость вентилятора установлена на {speed}");
    }
}

public class SecuritySystem
{
    public void Arm()
    {
        Console.WriteLine("[Безопасность] Сигнализация включена");
    }

    public void Disarm()
    {
        Console.WriteLine("[Безопасность] Сигнализация отключена");
    }

    public void LockAllDoors()
    {
        Console.WriteLine("[Безопасность] Все двери заблокированы");
    }

    public void UnlockMainDoor()
    {
        Console.WriteLine("[Безопасность] Входная дверь разблокирована");
    }
}

public class EntertainmentSystem
{
    public void TurnOnTV()
    {
        Console.WriteLine("[Развлечения] Телевизор включен");
    }

    public void TurnOffTV()
    {
        Console.WriteLine("[Развлечения] Телевизор выключен");
    }

    public void PlayMusic(string playlist)
    {
        Console.WriteLine($"[Развлечения] Воспроизведение плейлиста: {playlist}");
    }

    public void StopMusic()
    {
        Console.WriteLine("[Развлечения] Музыка остановлена");
    }
}

// Фасад — предоставляет простой интерфейс для управления всем домом.
public class SmartHomeFacade
{
    private LightingSystem _lighting;
    private ClimateSystem _climate;
    private SecuritySystem _security;
    private EntertainmentSystem _entertainment;

    public SmartHomeFacade()
    {
        _lighting = new LightingSystem();
        _climate = new ClimateSystem();
        _security = new SecuritySystem();
        _entertainment = new EntertainmentSystem();
    }

    // Режим "Уход из дома" — один вызов вместо множества
    public void LeaveHome()
    {
        Console.WriteLine("=== Активация режима 'Уход из дома' ===");
        _lighting.TurnOffLights();
        _climate.TurnOffAirConditioner();
        _entertainment.TurnOffTV();
        _entertainment.StopMusic();
        _security.LockAllDoors();
        _security.Arm();
        Console.WriteLine("=== Вы можете спокойно уходить ===\n");
    }

    // Режим "Возвращение домой"
    public void ReturnHome()
    {
        Console.WriteLine("=== Активация режима 'Возвращение домой' ===");
        _security.Disarm();
        _security.UnlockMainDoor();
        _lighting.TurnOnLights();
        _lighting.SetDimLevel(70);
        _climate.SetTemperature(22.5);
        _entertainment.PlayMusic("Лаунж для расслабления");
        Console.WriteLine("=== Добро пожаловать домой! ===\n");
    }

    // Режим "Вечерний киносеанс"
    public void MovieNight()
    {
        Console.WriteLine("=== Активация режима 'Киносеанс' ===");
        _lighting.SetDimLevel(20);
        _climate.SetTemperature(23.0);
        _entertainment.TurnOnTV();
        _entertainment.PlayMusic("Саундтреки к фильмам");
        Console.WriteLine("=== Приятного просмотра! ===\n");
    }

    // Режим "Сон"
    public void GoodNight()
    {
        Console.WriteLine("=== Активация режима 'Спокойной ночи' ===");
        _lighting.TurnOffLights();
        _entertainment.TurnOffTV();
        _entertainment.StopMusic();
        _climate.SetTemperature(20.0);
        _security.LockAllDoors();
        _security.Arm();
        Console.WriteLine("=== Спокойной ночи! ===\n");
    }

    // Режим быстрой настройки
    public void QuickSetup(bool withMusic, int temperature)
    {
        Console.WriteLine("=== Быстрая настройка ===");
        _lighting.TurnOnLights();
        _climate.SetTemperature(temperature);
        if (withMusic)
        {
            _entertainment.PlayMusic("Мои любимые треки");
        }
        Console.WriteLine("=== Настройка завершена ===\n");
    }
}
