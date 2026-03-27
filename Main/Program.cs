using System.Reflection;

namespace Patterns;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Title = "Паттерны проектирования — Демонстрация";

        // Если переданы аргументы командной строки
        if (args.Length > 0)
        {
            RunWithArguments(args);
        }
        else
        {
            RunInteractive();
        }
    }

    static void RunWithArguments(string[] args)
    {
        var patternMap = GetPatternMap();
        bool executed = false;

        foreach (string arg in args)
        {
            string cleanArg = arg.TrimStart('-', '/').ToLower();

            if (cleanArg == "help" || cleanArg == "h" || cleanArg == "?")
            {
                ShowHelp();
                return;
            }

            if (cleanArg == "list")
            {
                ShowPatternList();
                return;
            }

            if (patternMap.ContainsKey(cleanArg))
            {
                ExecutePattern(patternMap[cleanArg]);
                executed = true;
            }
        }

        if (!executed)
        {
            Console.WriteLine("❌ Неизвестный аргумент. Используйте --help для списка команд.\n");
            ShowHelp();
        }
    }

    static void RunInteractive()
    {
        Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║     Демонстрация паттернов проектирования (Gang of Four)     ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
        Console.WriteLine();

        var patterns = GetPatternsList();

        while (true)
        {
            Console.WriteLine("\n📋 Доступные паттерны:");
            Console.WriteLine(new string('─', 60));

            for (int i = 0; i < patterns.Count; i++)
            {
                Console.WriteLine($"{i + 1,2}. {patterns[i].Name}");
            }

            Console.WriteLine("\n 0. Выход");
            Console.WriteLine(new string('─', 60));
            Console.Write("\n👉 Выберите номер паттерна (или 0 для выхода): ");

            string input = Console.ReadLine() ?? throw new NullReferenceException("На входе был получен null");

            if (input == "0")
            {
                Console.WriteLine("\n👋 До свидания! Удачного проектирования!");
                break;
            }

            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= patterns.Count)
            {
                Console.Clear();
                ExecutePattern(patterns[choice - 1]);
                Console.WriteLine("\n🔹 Нажмите любую клавишу для возврата в меню...");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                Console.WriteLine("⚠️ Некорректный ввод. Пожалуйста, выберите номер из списка.");
            }
        }
    }

    static void ExecutePattern(PatternInfo pattern)
    {
        Console.WriteLine($"\n╔══════════════════════════════════════════════════════════════╗");
        Console.WriteLine($"║  {pattern.Name} ({pattern.RussianName})");
        Console.WriteLine($"╚══════════════════════════════════════════════════════════════╝");
        Console.WriteLine();

        try
        {
            pattern.Method.Invoke(null, null);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка при выполнении: {ex.InnerException?.Message ?? ex.Message}");
        }
    }

    static Dictionary<string, PatternInfo> GetPatternMap()
    {
        return GetPatternsList().ToDictionary(
            p => p.CommandKey,
            p => p,
            StringComparer.OrdinalIgnoreCase
        );
    }

    static List<PatternInfo> GetPatternsList()
    {
        return new()
            {
                // Порождающие паттерны
                new PatternInfo("Singleton", "Одиночка", "singleton", "singleton", PatternsTester.TestSingleton),
                new PatternInfo("FactoryMethod", "Фабричный метод", "factory-method", "factory", PatternsTester.TestFactoryMethod),
                new PatternInfo("AbstractFactory", "Абстрактная фабрика", "abstract-factory", "abstract", PatternsTester.TestAbstractFactory),
                new PatternInfo("Builder", "Строитель", "builder", "builder", PatternsTester.TestBuilder),
                new PatternInfo("Prototype", "Прототип", "prototype", "prototype", PatternsTester.TestPrototype),
                
                // Структурные паттерны
                new PatternInfo("Adapter", "Адаптер", "adapter", "adapter", PatternsTester.TestAdapter),
                new PatternInfo("Bridge", "Мост", "bridge", "bridge", PatternsTester.TestBridge),
                new PatternInfo("Composite", "Компоновщик", "composite", "composite", PatternsTester.TestComposite),
                new PatternInfo("Decorator", "Декоратор", "decorator", "decorator", PatternsTester.TestDecorator),
                new PatternInfo("Facade", "Фасад", "facade", "facade", PatternsTester.TestFacade),
                new PatternInfo("Flyweight", "Приспособленец", "flyweight", "flyweight", PatternsTester.TestFlyweight),
                new PatternInfo("Proxy", "Заместитель", "proxy", "proxy", PatternsTester.TestProxy),
                
                // Паттерны поведения
                new PatternInfo("ChainOfResponsibility", "Цепочка обязанностей", "chain", "chain-of-responsibility", PatternsTester.TestChainOfResponsibility),
                new PatternInfo("Command", "Команда", "command", "command", PatternsTester.TestCommand),
                new PatternInfo("Interpreter", "Интерпретатор", "interpreter", "interpreter", PatternsTester.TestInterpreter),
                new PatternInfo("Iterator", "Итератор", "iterator", "iterator", PatternsTester.TestIterator),
                new PatternInfo("Mediator", "Посредник", "mediator", "mediator", PatternsTester.TestMediator),
                new PatternInfo("Memento", "Хранитель", "memento", "memento", PatternsTester.TestMemento),
                new PatternInfo("Observer", "Наблюдатель", "observer", "observer", PatternsTester.TestObserver),
                new PatternInfo("State", "Состояние", "state", "state", PatternsTester.TestState),
                new PatternInfo("Strategy", "Стратегия", "strategy", "strategy", PatternsTester.TestStrategy),
                new PatternInfo("TemplateMethod", "Шаблонный метод", "template", "template-method", PatternsTester.TestTemplateMethod),
                new PatternInfo("Visitor", "Посетитель", "visitor", "visitor", PatternsTester.TestVisitor)
            };
    }

    static void ShowHelp()
    {
        Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  Демонстрация паттернов проектирования                      ║");
        Console.WriteLine("║  Gang of Four (23 паттерна)                                 ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
        Console.WriteLine();
        Console.WriteLine("📌 Использование:");
        Console.WriteLine("  PatternsTester.exe [--паттерн] [--паттерн2] ...");
        Console.WriteLine();
        Console.WriteLine("📌 Команды:");
        Console.WriteLine("  --help, -h, /?     Показать эту справку");
        Console.WriteLine("  --list             Показать список всех доступных паттернов");
        Console.WriteLine();
        Console.WriteLine("📌 Примеры запуска:");
        Console.WriteLine("  PatternsTester.exe --singleton");
        Console.WriteLine("  PatternsTester.exe --factory-method --observer");
        Console.WriteLine("  PatternsTester.exe --strategy");
        Console.WriteLine();
        Console.WriteLine("📌 Без аргументов запускается интерактивный режим с меню.");
        Console.WriteLine();
    }

    static void ShowPatternList()
    {
        var patterns = GetPatternsList();

        Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  Доступные паттерны проектирования                          ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
        Console.WriteLine();

        Console.WriteLine("📦 ПОРОЖДАЮЩИЕ ПАТТЕРНЫ:");
        var creational = patterns.Take(5);
        foreach (var p in creational)
        {
            Console.WriteLine($"  • {p.RussianName} (--{p.CommandKey})");
        }

        Console.WriteLine();
        Console.WriteLine("🏗️ СТРУКТУРНЫЕ ПАТТЕРНЫ:");
        var structural = patterns.Skip(5).Take(7);
        foreach (var p in structural)
        {
            Console.WriteLine($"  • {p.RussianName} (--{p.CommandKey})");
        }

        Console.WriteLine();
        Console.WriteLine("🎭 ПАТТЕРНЫ ПОВЕДЕНИЯ:");
        var behavioral = patterns.Skip(12);
        foreach (var p in behavioral)
        {
            Console.WriteLine($"  • {p.RussianName} (--{p.CommandKey})");
        }

        Console.WriteLine();
        Console.WriteLine($"📊 Всего: {patterns.Count} паттернов");
        Console.WriteLine();
    }
}

/// <summary>
/// Информация о паттерне
/// </summary>
public class PatternInfo
{
    public string MethodName { get; }
    public string RussianName { get; }
    public string CommandKey { get; }
    public string ShortKey { get; }
    public MethodInfo Method { get; }

    public string Name => $"{RussianName} ({MethodName})";

    public PatternInfo(string methodName, string russianName, string commandKey, string shortKey, Action action)
    {
        MethodName = methodName;
        RussianName = russianName;
        CommandKey = commandKey;
        ShortKey = shortKey;
        Method = action.Method;
    }
}
