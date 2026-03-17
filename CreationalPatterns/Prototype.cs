namespace CreationalPatterns;

// Базовый интерфейс для всех прототипов.
public interface IPrototype
{
    IPrototype Clone();
}

// Конкретный класс, реализующий клонирование.
public class Robot : IPrototype
{
    public string Model { get; set; } = "<Неизвестная модель>";
    public string Processor { get; set; } = "<Неизвестный процессор>";
    public List<string> Weapons { get; set; } = new();

    public Robot(string model, string processor, List<string> weapons)
    {
        Model = model;
        Processor = processor;
        Weapons = weapons;
    }

    // Реализация метода клонирования.
    public IPrototype Clone() => new Robot(Model, Processor, new(Weapons));

    public void ShowInfo()
    {
        Console.WriteLine($"\tМодель: {Model}");
        Console.WriteLine($"\tПроцессор: {Processor}");
        Console.WriteLine($"\tОружие: {string.Join(", ", Weapons)}");
    }
}

// Еще один пример прототипа.
public class Enemy : IPrototype
{
    public string Type { get; set; } = "<Тип неизвестен>";
    public int Health { get; set; }
    public int Damage { get; set; }

    public Enemy(string type, int health, int damage)
    {
        Type = type;
        Health = health;
        Damage = damage;
    }

    public IPrototype Clone() => new Enemy(Type, Health, Damage);

    public void ShowInfo()
    {
        Console.WriteLine($"Тип врага: {Type}, Здоровье: {Health}, Урон: {Damage}");
    }
}

public class EnemyFactory
{
    private Dictionary<string, IPrototype> _prototypes = new();

    public void RegisterPrototype(string key, IPrototype prototype)
    {
        _prototypes[key] = prototype;
    }

    public IPrototype Create(string key)
    {
        if (_prototypes.ContainsKey(key))
        {
            return _prototypes[key].Clone();
        }

        throw new ArgumentException($"Прототип с ключом {key} не найден");
    }
}