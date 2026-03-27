namespace BehaviouralPatterns;

// Базовый класс обработчика.
public abstract class SupportHandler
{
    protected SupportHandler? _nextHandler;
    protected string _levelName;

    public SupportHandler(string levelName) { _levelName = levelName; }

    public SupportHandler SetNext(SupportHandler handler)
    {
        _nextHandler = handler;
        return handler;
    }

    public void HandleRequest(Request request)
    {
        if (CanHandle(request))
        {
            Process(request);
        }
        else if (_nextHandler is not null)
        {
            Console.WriteLine($"❌ [{_levelName}] Не могу обработать `{request.Description}` - передаю дальше...");
            _nextHandler.HandleRequest(request);
        }
        else
        {
            Console.WriteLine($"❌ [{_levelName}] Запрос `{request.Description}` никому не удалось обработать! " +
                "\nС этим придется разбираться начальству!!!");
        }
    }

    // Метод, проевряющий, может ли этот обработчик справиться с запросом.
    protected abstract bool CanHandle(Request request);

    // Метод, выполняющий обработку.
    protected abstract void Process(Request request);
}

// Класс запроса.
public class Request
{
    // Описание запроса.
    public string Description { get; set; }

    // Сложность запроса.
    public int Complexity { get; set; }

    public Request(string description, int complexity)
    {
        Description = description;
        Complexity = complexity;
    }
}

public class Helpdesk : SupportHandler
{
    public Helpdesk() : base("Рядовой сотрудник техподдержки") { }

    protected override bool CanHandle(Request request) => request.Complexity <= 3;

    protected override void Process(Request request)
    {
        Console.WriteLine($"✅ [{_levelName}] Запрос `{request.Description}` обработан на уровне техподдержки (на месте)!");
    }
}

public class Technician : SupportHandler
{
    public Technician() : base("Очень компетентный и технически подкованный сотрудник техподдержки") { }

    protected override bool CanHandle(Request request) => request.Complexity >= 4 && request.Complexity <= 6;

    protected override void Process(Request request)
    {
        Console.WriteLine($"✅ [{_levelName}] Запрос `{request.Description}` обработан инженерами отедела разработки!");
    }
}

public class SupportLead : SupportHandler
{
    public SupportLead() : base("Начальник техподдержки") { }

    protected override bool CanHandle(Request request) => request.Complexity == 9;

    protected override void Process(Request request)
    {
        Console.WriteLine($" [{_levelName}] Запрос `{request.Description}` обработан инженерами отдела разработки!");
    }
}


public class DevelopmentTeam : SupportHandler
{
    public DevelopmentTeam() : base("Отдел разработки") { }

    protected override bool CanHandle(Request request)
        => request.Complexity >= 8 && request.Complexity <= 9;

    protected override void Process(Request request)
    {
        Console.WriteLine($"✅ [{_levelName}] Запрос `{request.Description}` обработан командой разработки!");
    }
}

public class EscalationSupport : SupportHandler
{
    public EscalationSupport() : base("Эскалация") { }

    protected override bool CanHandle(Request request) => request.Complexity >= 10;

    protected override void Process(Request request)
    {
        Console.WriteLine($"✅ [{_levelName}] Запрос `{request.Description}` передан начальству!");
    }
}
