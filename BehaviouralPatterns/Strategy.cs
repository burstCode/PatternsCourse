namespace BehaviouralPatterns;

// Интерфейс стратегии.
public interface IRouteStrategy
{
    string BuildRoute(string start, string destination);
    int CalculateEstimatedTime(int distance);
    string GetStrategyName();
}

// Конкретные стратегии //

// 1. Самый быстрый маршрут (оптимизация по времени)
public class FastestRouteStrategy : IRouteStrategy
{
    public string BuildRoute(string start, string destination)
    {
        // Здесь была бы сложная логика построения маршрута
        return $"[Быстрый маршрут] {start} -> трасса М-4 -> {destination}";
    }

    public int CalculateEstimatedTime(int distance)
    {
        // Средняя скорость 90 км/ч для быстрого маршрута
        return (int)(distance / 90.0 * 60);
    }

    public string GetStrategyName() => "Самый быстрый";
}

// 2. Самый короткий маршрут (оптимизация по расстоянию)
public class ShortestRouteStrategy : IRouteStrategy
{
    public string BuildRoute(string start, string destination)
    {
        return $"[Короткий маршрут] {start} -> объездная дорога -> {destination}";
    }

    public int CalculateEstimatedTime(int distance)
    {
        // Средняя скорость 50 км/ч для короткого маршрута (могут быть пробки)
        return (int)(distance / 50.0 * 60);
    }

    public string GetStrategyName() => "Самый короткий";
}

// 3. Экономичный маршрут (оптимизация по расходу топлива)
public class EconomicalRouteStrategy : IRouteStrategy
{
    public string BuildRoute(string start, string destination)
    {
        return $"[Экономичный маршрут] {start} -> проселочная дорога -> {destination}";
    }

    public int CalculateEstimatedTime(int distance)
    {
        // Средняя скорость 60 км/ч для экономичного маршрута
        return (int)(distance / 60.0 * 60);
    }

    public string GetStrategyName() => "Экономичный";
}

// 4. Маршрут с живописными видами (оптимизация по красоте)
public class ScenicRouteStrategy : IRouteStrategy
{
    public string BuildRoute(string start, string destination)
    {
        return $"[Живописный маршрут] {start} -> набережная, парк, смотровая -> {destination}";
    }

    public int CalculateEstimatedTime(int distance)
    {
        // Средняя скорость 40 км/ч для живописного маршрута (много остановок)
        return (int)(distance / 40.0 * 60);
    }

    public string GetStrategyName() => "Живописный";
}

// 5. Маршрут без пробок (использует данные о трафике)
public class NoTrafficRouteStrategy : IRouteStrategy
{
    public string BuildRoute(string start, string destination)
    {
        return $"[Без пробок] {start} -> объезд пробок -> {destination}";
    }

    public int CalculateEstimatedTime(int distance)
    {
        // Средняя скорость 70 км/ч с учетом объезда пробок
        return (int)(distance / 70.0 * 60);
    }

    public string GetStrategyName() => "Без пробок";
}

// Контекст — навигатор
public class Navigator
{
    private IRouteStrategy _strategy;
    private string _currentLocation;
    private Dictionary<string, int> _distances = new Dictionary<string, int>();

    public Navigator(string startLocation)
    {
        _currentLocation = startLocation;
        // Стратегия по умолчанию
        _strategy = new FastestRouteStrategy();

        // Инициализируем расстояния между городами
        _distances["Москва-Санкт-Петербург"] = 700;
        _distances["Москва-Казань"] = 800;
        _distances["Москва-Сочи"] = 1600;
        _distances["Москва-Екатеринбург"] = 1800;
        _distances["Москва-Новосибирск"] = 3400;
    }

    // Смена стратегии во время выполнения
    public void SetStrategy(IRouteStrategy strategy)
    {
        _strategy = strategy;
        Console.WriteLine($"\n📌 Стратегия изменена на: {_strategy.GetStrategyName()}");
    }

    // Построение маршрута
    public void BuildRoute(string destination)
    {
        string key = $"{_currentLocation}-{destination}";

        if (!_distances.ContainsKey(key))
        {
            Console.WriteLine($"\n❌ Маршрут {_currentLocation} -> {destination} не найден в базе");
            return;
        }

        int distance = _distances[key];
        string route = _strategy.BuildRoute(_currentLocation, destination);
        int estimatedTime = _strategy.CalculateEstimatedTime(distance);

        Console.WriteLine($"\n🚗 Построение маршрута: {_currentLocation} -> {destination}");
        Console.WriteLine($"   {route}");
        Console.WriteLine($"   Расстояние: {distance} км");
        Console.WriteLine($"   Расчетное время: {estimatedTime} мин ({estimatedTime / 60} ч {estimatedTime % 60} мин)");
    }

    public void SetCurrentLocation(string location)
    {
        _currentLocation = location;
        Console.WriteLine($"📍 Текущее местоположение: {_currentLocation}");
    }

    public void ShowCurrentStrategy()
    {
        Console.WriteLine($"Текущая стратегия: {_strategy.GetStrategyName()}");
    }
}
