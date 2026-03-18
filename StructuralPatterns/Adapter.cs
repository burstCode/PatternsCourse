namespace StructuralPatterns;

// Целевой интерфейс, который ожидает клиент.
public interface ITemperatureSensor
{
    double GetTemperatureCelsius();
}

// Адаптируемый интерфейс.
public class FahrenheitSensor
{
    private Random _random = new();

    public double GetTemperatureFahrenheit()
    {
        // Имитация датчика.
        return 68 + (_random.NextDouble() * 10 - 5);
    }
}

// Адаптер объекта.
public class TemperatureAdapter : ITemperatureSensor
{
    private FahrenheitSensor _fahrenheitSensor;

    public TemperatureAdapter(FahrenheitSensor sensor)
    {
        _fahrenheitSensor = sensor;
    }

    public double GetTemperatureCelsius()
    {
        double fahrenheit = _fahrenheitSensor.GetTemperatureFahrenheit();
        Console.WriteLine($"Получена температура в Фаренгейтах: {fahrenheit}");

        double celsius = (fahrenheit - 32) * 5.0 / 9.0;

        return Math.Round(celsius, 1);
    }
}