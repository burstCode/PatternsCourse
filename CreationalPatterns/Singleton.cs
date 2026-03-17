namespace CreationalPatterns;

public class Singleton
{
    // Здесь хранится единственный экземпляр класса.
    private static Singleton? _instance = null;

    // Конструктор является приватным,
    // Вручную никто не сможет создать объект.
    private Singleton() { }

    // Метод для получения экземпляра класса.
    public static Singleton Instance()
    {
        if (_instance == null)
            _instance = new Singleton();

        return _instance;
    }

    // Какой-то метод, выполняющий полезную работу.
    public void UsefulMethod()
    {
        Console.WriteLine("Я выполняю полезную нагрузку!");
    }
}
