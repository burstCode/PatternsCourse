namespace BehaviouralPatterns;

// Абстрактный класс с шаблонным методом
public abstract class BeverageMaker
{
    // Шаблонный метод — определяет скелет алгоритма
    // Он объявлен как sealed, чтобы подклассы не могли его переопределить
    public void MakeBeverage()
    {
        Console.WriteLine($"\n=== Приготовление {GetBeverageName()} ===\n");

        BoilWater();
        Brew();
        PourIntoCup();
        AddCondiments();

        Console.WriteLine($"\n{GetBeverageName()} готов! Приятного аппетита!\n");
    }

    // Шаги, общие для всех напитков
    private void BoilWater() => Console.WriteLine("1. Кипятим воду");

    private void PourIntoCup() => Console.WriteLine("3. Наливаем в чашку");

    // Шаги, которые должны быть реализованы в подклассах
    protected abstract string GetBeverageName();
    protected abstract void Brew();
    protected abstract void AddCondiments();

    // Опциональные шаги с реализацией по умолчанию (можно переопределить)
    protected virtual void ExtraSteps()
    {
    }
}

// Конкретный класс для чая
public class TeaMaker : BeverageMaker
{
    protected override string GetBeverageName() => "Чай";

    protected override void Brew() => Console.WriteLine("2. Завариваем чайную заварку в чайнике");

    protected override void AddCondiments() => Console.WriteLine("4. Добавляем лимон и сахар по вкусу");
}

// Конкретный класс для кофе
public class CoffeeMaker : BeverageMaker
{
    protected override string GetBeverageName() => "Кофе";

    protected override void Brew() => Console.WriteLine("2. Варим кофе в турке");

    protected override void AddCondiments() => Console.WriteLine("4. Добавляем молоко и сахар по вкусу");
}

// Конкретный класс для какао
public class CocoaMaker : BeverageMaker
{
    protected override string GetBeverageName() => "Какао";

    protected override void Brew() => Console.WriteLine("2. Смешиваем какао-порошок с сахаром");

    protected override void AddCondiments() => Console.WriteLine("4. Добавляем зефир или взбитые сливки");

    // Добавляем дополнительный шаг
    protected override void ExtraSteps() => Console.WriteLine("   (Дополнительно: посыпаем корицей)");
}

// Более сложный пример — отчеты
public abstract class ReportGenerator
{
    // Шаблонный метод для генерации отчета
    public void GenerateReport()
    {
        Console.WriteLine($"\n--- Генерация отчета: {GetReportName()} ---\n");

        string data = CollectData();
        string processedData = ProcessData(data);
        string formattedContent = FormatContent(processedData);

        if (ShouldAddHeader())
            AddHeader(formattedContent);

        if (ShouldAddFooter())
            AddFooter(formattedContent);

        ExportReport(formattedContent);

        Console.WriteLine($"Отчет '{GetReportName()}' успешно сгенерирован!");
    }

    // Шаги, которые могут быть переопределены
    protected virtual string CollectData()
    {
        Console.WriteLine("  Сбор данных из стандартных источников");
        return "сырые данные";
    }

    protected virtual string ProcessData(string rawData)
    {
        Console.WriteLine($"  Обработка данных: {rawData}");
        return "обработанные данные";
    }

    // Абстрактные шаги
    protected abstract string GetReportName();
    protected abstract string FormatContent(string data);
    protected abstract void ExportReport(string content);

    // Опциональные шаги с реализацией по умолчанию
    protected virtual bool ShouldAddHeader() => false;
    protected virtual bool ShouldAddFooter() => false;
    protected virtual void AddHeader(string content)
    {
        Console.WriteLine("  Добавлен заголовок отчета");
    }

    protected virtual void AddFooter(string content)
    {
        Console.WriteLine("  Добавлен нижний колонтитул");
    }
}

// Отчет по продажам
public class SalesReportGenerator : ReportGenerator
{
    private bool _includeHeader = true;
    private bool _includeFooter = true;

    public SalesReportGenerator(bool includeHeader = true, bool includeFooter = true)
    {
        _includeHeader = includeHeader;
        _includeFooter = includeFooter;
    }

    protected override string GetReportName() => "Отчет по продажам";

    protected override string CollectData()
    {
        Console.WriteLine("  Сбор данных о продажах из CRM");
        return "данные продаж за месяц";
    }

    protected override string ProcessData(string rawData)
    {
        Console.WriteLine($"  Анализ продаж: {rawData}");
        return "итоги: продажи выросли на 15%";
    }

    protected override string FormatContent(string data)
    {
        return $"=== ОТЧЕТ ПО ПРОДАЖАМ ===\n{data}\nОбщая выручка: 1 500 000 руб.";
    }

    protected override void ExportReport(string content)
    {
        Console.WriteLine($"  Экспорт в PDF:\n{content}");
    }

    protected override bool ShouldAddHeader() => _includeHeader;
    protected override bool ShouldAddFooter() => _includeFooter;
}

// Отчет по сотрудникам
public class EmployeeReportGenerator : ReportGenerator
{
    protected override string GetReportName() => "Отчет по сотрудникам";

    protected override string CollectData()
    {
        Console.WriteLine("  Сбор данных о сотрудниках из HR-системы");
        return "данные о сотрудниках";
    }

    protected override string ProcessData(string rawData)
    {
        Console.WriteLine($"  Анализ кадрового состава: {rawData}");
        return "всего сотрудников: 45, новых: 3";
    }

    protected override string FormatContent(string data)
    {
        return $"=== ОТЧЕТ ПО СОТРУДНИКАМ ===\n{data}\nСредний возраст: 32 года";
    }

    protected override void ExportReport(string content)
    {
        Console.WriteLine($"  Экспорт в Excel:\n{content}");
    }

    protected override bool ShouldAddFooter() => true;
}
