using BehaviouralPatterns;

namespace Patterns;

public partial class PatternsTester
{
    public static void TestChainOfResponsibility()
    {
        Helpdesk helpdesk = new();
        Technician tech = new();
        SupportLead lead = new();
        DevelopmentTeam dev = new();
        EscalationSupport boss = new();

        helpdesk.SetNext(tech).SetNext(lead).SetNext(dev).SetNext(boss);

        List<Request> requests = new()
        {
            new("Как выгрузить отчет?", 2),
            new("Не выгружается отчет, выдает ошибку", 5),
            new("Мы приобрели модуль усиленной отчетности, выполните внедрение?", 6),
            new("Можете разработать для нас модуль отчетности по интеграции с NASA?", 8),
            new("Хотим рассмотреть перспективу сотрудничества для разработки совместного проекта", 10)
        };

        foreach (Request request in requests)
        {
            Console.WriteLine($"\n➤ Запрос: {request.Description} (сложность: {request.Complexity})");
            helpdesk.HandleRequest(request);
        }
    }

    public static void TestCommand()
    {
        TextEditor editor = new();
        CommandInvoker invoker = new();

        // Создадим набор команд для написания Hello World.
        InsertTextCommand insertHello = new(editor, "Hello");
        InsertTextCommand insertSpace = new(editor, " ");
        InsertTextCommand insertWorld = new(editor, "World");

        // Создадим макрос и добавим в него созданные команды.
        MacroCommand formatMacro = new("Шаблон документа.");

        formatMacro.AddCommand(new InsertTextCommand(editor, "\n--- Начало документа ---\n", 0));
        formatMacro.AddCommand(insertHello);
        formatMacro.AddCommand(insertSpace);
        formatMacro.AddCommand(insertWorld);

        // Выполняем макрос.
        invoker.ExecuteCommand(formatMacro);

        // Выводим результат.
        editor.Display();

        Console.WriteLine(new string('-', 50));
        // --------------------------------------------------

        // Выполним другую команду: замена выделенного текста.
        editor.SetSelection(26, 11);  // Выделяем "Hello World"
        ReplaceSelectionCommand replaceCommand = new(editor, "Ай мамма родила хулиганнна");
        invoker.ExecuteCommand(replaceCommand);
        editor.Display();

        // --------------------------------------------------

        InsertTextCommand redoCommand = new(editor, "Повтори меня");
        invoker.ExecuteCommand(redoCommand);
        invoker.Undo();
        invoker.Redo();
        invoker.Undo();

        // --------------------------------------------------

        Console.WriteLine(new string('-', 50));
        Console.WriteLine("\nИстория выполненных команд:");
        invoker.ShowHistory();

        // --------------------------------------------------

        // Отменим последнюю команду.
        // Примечание: макрос считается за одну команду,
        // поэтому при вызове Undo отменятся все команды,
        // выполненные под эгидой макроса. (Удобно!)
        invoker.Undo();
        editor.Display();
    }

    public static void TestInterpreter()
    {
        ExpressionParser parser = new();
        Context context = new();

        context.SetVariable("x", 10);
        context.SetVariable("y", 5);
        context.SetVariable("z", 3);
        context.SetVariable("width", 100);
        context.SetVariable("height", 50);

        string[] expressions =
        [
            "3 + 5",
                "10 - 4",
                "2 * 6",
                "15 / 3",
                "(2 + 3) * 4",
                "x + y",
                "x * y - z",
                "(width + height) * 2",
                "x + y * z",
                "(x + y) * z"
        ];

        Console.WriteLine("Арифметические выражения с переменными:\n");
        Console.WriteLine("Переменные:");
        Console.WriteLine($"  x = {context.GetVariable("x")}");
        Console.WriteLine($"  y = {context.GetVariable("y")}");
        Console.WriteLine($"  z = {context.GetVariable("z")}");
        Console.WriteLine($"  width = {context.GetVariable("width")}");
        Console.WriteLine($"  height = {context.GetVariable("height")}");
        Console.WriteLine(new string('-', 60));

        foreach (string expr in expressions)
        {
            try
            {
                IExpression expression = parser.Parse(expr);
                int result = expression.Interpret(context);
                Console.WriteLine($"{expr,20} = {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{expr,20} -> Ошибка: {ex.Message}");
            }
        }

        Console.WriteLine("\n" + new string('=', 60));

        Console.WriteLine("\nИзменение переменных во время выполнения:");
        Console.WriteLine(new string('-', 60));

        IExpression dynamicExpr = parser.Parse("x * 2 + y");
        Console.WriteLine($"Выражение: x * 2 + y");

        context.SetVariable("x", 5);
        context.SetVariable("y", 3);
        Console.WriteLine($"  При x=5, y=3 -> {dynamicExpr.Interpret(context)}");

        context.SetVariable("x", 10);
        context.SetVariable("y", 7);
        Console.WriteLine($"  При x=10, y=7 -> {dynamicExpr.Interpret(context)}");

        context.SetVariable("x", 0);
        context.SetVariable("y", 100);
        Console.WriteLine($"  При x=0, y=100 -> {dynamicExpr.Interpret(context)}");

        // Пример с делением
        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("\nПримеры с делением:");
        Console.WriteLine(new string('-', 60));

        string[] divisionExpressions =
        [
            "100 / 4",
                "100 / x",
                "100 / 0",
            ];

        foreach (string expr in divisionExpressions)
        {
            try
            {
                IExpression expression = parser.Parse(expr);
                int result = expression.Interpret(context);
                Console.WriteLine($"{expr,15} = {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{expr,15} -> {ex.Message}");
            }
        }
    }

    public static void TestIterator()
    {
        // 1. Работа с коллекцией книг.
        Console.WriteLine("1. Коллекция книг:");
        Console.WriteLine(new string('-', 50));

        BookCollection library = new();
        library.AddBook("Паттерны объектно-ориентированного проектирования");
        library.AddBook("О дивный новый мир");
        library.AddBook("Docker без секретов");
        library.AddBook("1984");
        library.AddBook("Монологи о наслаждении, апатии и смерти");
        library.AddBook("Скотный двор");

        Console.WriteLine("Прямой обход:");
        IIterator<string> iterator = library.CreateIterator();
        for (iterator.First(); !iterator.IsDone(); iterator.Next())
        {
            Console.WriteLine($"  • {iterator.CurrentItem()}");
        }

        Console.WriteLine("\nОбратный обход:");
        IIterator<string> reverseIterator = library.CreateReverseIterator();
        for (reverseIterator.First(); !reverseIterator.IsDone(); reverseIterator.Next())
        {
            Console.WriteLine($"  • {reverseIterator.CurrentItem()}");
        }

        // 2. Работа с плейлистом.
        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("\n2. Музыкальный плейлист:");
        Console.WriteLine(new string('-', 50));

        Playlist myPlaylist = new();
        myPlaylist.AddSong("Painters of the Tempest");
        myPlaylist.AddSong("Маленькая Италия");
        myPlaylist.AddSong("Boom Boom Japan");
        myPlaylist.AddSong("burn this moment into the retina of my eye");

        IIterator<string> playlistIterator = myPlaylist.CreateIterator();
        for (playlistIterator.First(); !playlistIterator.IsDone(); playlistIterator.Next())
        {
            Console.WriteLine($"  ♪ {playlistIterator.CurrentItem()}");
        }

        // 3. Внутренний итератор.
        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("\n3. Внутренний итератор (автоматический обход):");
        Console.WriteLine(new string('-', 50));

        Console.WriteLine("Книги с нумерацией:");
        int index = 1;
        InternalIterator.ForEach(library, book =>
        {
            Console.WriteLine($"  {index++}. {book}");
        });

        // 4. Демонстрация независимых итераторов.
        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("\n4. Несколько независимых итераторов одновременно:");
        Console.WriteLine(new string('-', 50));

        IIterator<string> iter1 = library.CreateIterator();
        IIterator<string> iter2 = library.CreateReverseIterator();

        Console.WriteLine("Прямой обход (iter1) и обратный обход (iter2) одновременно:");
        iter1.First();
        iter2.First();

        while (!iter1.IsDone() && !iter2.IsDone())
        {
            Console.WriteLine($"  {iter1.CurrentItem(),-25} ←→ {iter2.CurrentItem(),25}");
            iter1.Next();
            iter2.Next();
        }

        // 5. Пример с разными типами коллекций.
        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("\n5. Единообразная работа с разными коллекциями:");
        Console.WriteLine(new string('-', 50));

        Console.WriteLine("Книги:");
        PrintCollection(library);
        Console.WriteLine("\nПесни:");
        PrintCollection(myPlaylist);

        // Небольшой вспомогательный метод :)
        static void PrintCollection(IAggregate<string> aggregate)
        {
            IIterator<string> iterator = aggregate.CreateIterator();
            int count = 1;
            for (iterator.First(); !iterator.IsDone(); iterator.Next())
            {
                Console.WriteLine($"  {count++}. {iterator.CurrentItem()}");
            }
        }
    }

    public static void TestMediator()
    {
        // Создаем посредника.
        ControlTower tower = new();

        // Создаем самолеты.
        Aircraft flight101 = new("SU101", tower);
        Aircraft flight202 = new("BA202", tower);
        Aircraft flight303 = new("AF303", tower);
        Aircraft flight404 = new("DL404", tower);

        Console.WriteLine(new string('=', 60));

        // 1. Обмен сообщениями
        Console.WriteLine("\n1. Обмен сообщениями между самолетами:\n");
        flight101.SendMessage("Всем добрый день, погода отличная!");
        flight202.SendMessage("Принято, SU101. Держусь на эшелоне 100");

        Console.WriteLine(new string('=', 60));

        // 2. Посадка с очередью
        Console.WriteLine("\n2. Посадка самолетов (с очередью):\n");

        flight101.RequestLanding();   // Первый запрос — посадка разрешена
        flight202.RequestLanding();   // Второй — встает в очередь
        flight303.RequestLanding();   // Третий — встает в очередь

        Console.WriteLine("\n(Ожидаем завершения посадки первого самолета...)\n");
        Thread.Sleep(1500);

        Console.WriteLine(new string('=', 60));

        // 3. Взлет
        Console.WriteLine("\n3. Взлет самолета:\n");
        flight101.RequestTakeOff();

        Console.WriteLine(new string('=', 60));

        Console.WriteLine("\n4. Дополнительные запросы на посадку:\n");

        flight404.RequestLanding();
        flight202.RequestLanding();
    }

    public static void TestMemento()
    {
        TextEditor_Memento editor = new();
        HistoryManager history = new(editor);

        editor.Display();

        // Работа с редактором
        Console.WriteLine("1. Вводим текст:");
        Console.WriteLine(new string('-', 50));

        history.SaveState();
        editor.Type("АЙЙЙ");
        history.SaveState();

        editor.Type(" ");
        history.SaveState();

        editor.Type("МАМА");
        history.SaveState();

        editor.Type(" ");

        editor.Type("РАДИЛА");

        editor.Type(" ");

        editor.Type("ХУУЛИГАНААА");
        history.SaveState();

        editor.Display();

        // Отмена действий
        Console.WriteLine("2. Отмена действий:");
        Console.WriteLine(new string('-', 50));

        history.Undo();
        history.Undo();
        history.Undo();
        editor.Display();

        // Повтор действий
        Console.WriteLine("3. Повтор действий:");
        Console.WriteLine(new string('-', 50));

        history.Redo();
        history.Redo();
        history.Redo();
        editor.Display();

        // Новое действие после отмены
        Console.WriteLine("4. Новое действие после отмены (стек повтора очищается):");
        Console.WriteLine(new string('-', 50));

        history.SaveState();
        editor.Type(" swag");
        history.SaveState();
        editor.Display();

        // История состояний
        Console.WriteLine("5. История сохраненных состояний:");
        Console.WriteLine(new string('-', 50));
        history.ShowHistory();

        // Демонстрация сохранения курсора
        Console.WriteLine("6. Сохранение позиции курсора:");
        Console.WriteLine(new string('-', 50));

        editor.MoveCursor(9);
        history.SaveState();

        editor.Type("АЧКА");
        editor.Display();

        history.Undo();
        editor.Display();


        TextMemento memento = editor.CreateMemento();
        Console.WriteLine("Посыльный может получить только метаинформацию:");
        Console.WriteLine($"  {memento.GetSummary()}");
    }

    public static void TestObserver()
    {
        // Создаем субъект.
        WeatherStation weatherStation = new();

        // Создаем наблюдателей.
        Console.WriteLine("1. Регистрация наблюдателей:");
        Console.WriteLine(new string('-', 50));

        CurrentConditionsDisplay currentDisplay = new(weatherStation, "Текущие условия");
        StatisticsDisplay statisticsDisplay = new(weatherStation, "Статистика");
        ForecastDisplay forecastDisplay = new(weatherStation, "Прогноз");
        AlertSystem alertSystem = new(weatherStation, "Система оповещения");

        Console.WriteLine(new string('=', 60));

        // Обновляем показания — наблюдатели автоматически получают уведомления
        Console.WriteLine("\n2. Первое обновление показаний:");
        Console.WriteLine(new string('-', 50));
        weatherStation.SetMeasurements(25.5f, 65, 755);

        Console.WriteLine(new string('=', 60));

        // Второе обновление
        Console.WriteLine("\n3. Второе обновление показаний:");
        Console.WriteLine(new string('-', 50));
        weatherStation.SetMeasurements(28.0f, 70, 748);

        Console.WriteLine(new string('=', 60));

        // Третье обновление — с критическими значениями
        Console.WriteLine("\n4. Третье обновление показаний (критические значения):");
        Console.WriteLine(new string('-', 50));
        weatherStation.SetMeasurements(38.5f, 95, 735);

        Console.WriteLine(new string('=', 60));

        // Удаляем одного наблюдателя
        Console.WriteLine("\n5. Отключение дисплея статистики:");
        Console.WriteLine(new string('-', 50));
        weatherStation.RemoveObserver(statisticsDisplay);

        Console.WriteLine("\n6. Обновление после отключения:");
        Console.WriteLine(new string('-', 50));
        weatherStation.SetMeasurements(22.0f, 55, 762);

        Console.WriteLine(new string('=', 60));

        // Демонстрация повторного подключения
        Console.WriteLine("\n7. Повторное подключение дисплея статистики:");
        Console.WriteLine(new string('-', 50));
        weatherStation.RegisterObserver(statisticsDisplay);

        Console.WriteLine("\n8. Финальное обновление:");
        Console.WriteLine(new string('-', 50));
        weatherStation.SetMeasurements(24.0f, 60, 758);
    }

    public static void TestState()
    {
        Order order = new();
        order.ShowInfo();

        // Добавляем товары.
        Console.WriteLine("1. Добавление товаров:");
        Console.WriteLine(new string('-', 50));
        order.AddItem("Ноутбук", 1000);
        order.AddItem("Мышь", 50);
        order.ShowInfo();

        // Переход к следующему состоянию.
        Console.WriteLine("2. Переход к обработке:");
        Console.WriteLine(new string('-', 50));
        order.Next();
        order.ShowInfo();

        // Пробуем добавить товар в состоянии "В обработке".
        Console.WriteLine("3. Попытка добавить товар в обработке:");
        Console.WriteLine(new string('-', 50));
        order.AddItem("Клавиатура", 100);
        order.ShowInfo();

        // Переход к ожиданию оплаты.
        Console.WriteLine("4. Подтверждение заказа:");
        Console.WriteLine(new string('-', 50));
        order.Next();
        order.ShowInfo();

        // Отмена на этапе оплаты.
        Console.WriteLine("5. Отмена заказа на этапе оплаты:");
        Console.WriteLine(new string('-', 50));
        order.Cancel();
        order.ShowInfo();

        // Создаем новый заказ для демонстрации полного цикла.
        Console.WriteLine("\n6. Новый заказ (полный цикл):");
        Console.WriteLine(new string('-', 50));

        Order newOrder = new();
        newOrder.AddItem("Телефон", 800);
        newOrder.AddItem("Чехол", 30);
        newOrder.ShowInfo();

        newOrder.Next();  // Обработка
        newOrder.Next();  // Ожидание оплаты
        newOrder.Next();  // Отправлен
        newOrder.Next();  // Доставлен
        newOrder.Next();  // Завершен

        newOrder.ShowInfo();

        // Демонстрация невозможности действий в терминальных состояниях
        Console.WriteLine("7. Попытка действий в завершенном состоянии:");
        Console.WriteLine(new string('-', 50));
        newOrder.AddItem("Аксессуар", 20);
        newOrder.Cancel();
    }

    public static void TestStrategy()
    {
        // Создаем навигатор
        Navigator navigator = new("Москва");

        // Показываем текущую стратегию
        Console.WriteLine("1. Текущая стратегия (по умолчанию):");
        Console.WriteLine(new string('-', 50));
        navigator.ShowCurrentStrategy();

        // Строим маршрут с текущей стратегией
        Console.WriteLine("\n2. Построение маршрута с текущей стратегией:");
        Console.WriteLine(new string('-', 50));
        navigator.BuildRoute("Санкт-Петербург");

        // Меняем стратегию на самую короткую
        Console.WriteLine("\n3. Смена стратегии на 'Самый короткий':");
        Console.WriteLine(new string('-', 50));
        navigator.SetStrategy(new ShortestRouteStrategy());
        navigator.BuildRoute("Санкт-Петербург");

        // Меняем стратегию на экономичную
        Console.WriteLine("\n4. Смена стратегии на 'Экономичный':");
        Console.WriteLine(new string('-', 50));
        navigator.SetStrategy(new EconomicalRouteStrategy());
        navigator.BuildRoute("Санкт-Петербург");

        // Меняем направление
        Console.WriteLine("\n5. Новое направление: Москва -> Казань:");
        Console.WriteLine(new string('-', 50));
        navigator.BuildRoute("Казань");

        // Живописный маршрут
        Console.WriteLine("\n6. Смена стратегии на 'Живописный':");
        Console.WriteLine(new string('-', 50));
        navigator.SetStrategy(new ScenicRouteStrategy());
        navigator.BuildRoute("Казань");

        // Маршрут без пробок
        Console.WriteLine("\n7. Смена стратегии на 'Без пробок':");
        Console.WriteLine(new string('-', 50));
        navigator.SetStrategy(new NoTrafficRouteStrategy());
        navigator.BuildRoute("Казань");

        // Меняем местоположение
        Console.WriteLine("\n8. Смена местоположения: Санкт-Петербург -> Сочи:");
        Console.WriteLine(new string('-', 50));
        navigator.SetCurrentLocation("Санкт-Петербург");
        navigator.SetStrategy(new FastestRouteStrategy());
        navigator.BuildRoute("Сочи");

        // Сравнение стратегий для одного маршрута
        Console.WriteLine("\n9. Сравнение стратегий для маршрута Москва -> Екатеринбург:");
        Console.WriteLine(new string('-', 50));

        List<IRouteStrategy> strategies = new()
        {
            new FastestRouteStrategy(),
            new ShortestRouteStrategy(),
            new EconomicalRouteStrategy(),
            new NoTrafficRouteStrategy(),
            new ScenicRouteStrategy()
        };

        navigator.SetCurrentLocation("Москва");

        foreach (IRouteStrategy strategy in strategies)
        {
            navigator.SetStrategy(strategy);
            navigator.BuildRoute("Екатеринбург");
        }
    }

    public static void TestTemplateMethod()
    {
        // Демонстрация с напитками
        Console.WriteLine("1. Приготовление напитков:");
        Console.WriteLine(new string('=', 60));

        BeverageMaker tea = new TeaMaker();
        tea.MakeBeverage();

        BeverageMaker coffee = new CoffeeMaker();
        coffee.MakeBeverage();

        BeverageMaker cocoa = new CocoaMaker();
        cocoa.MakeBeverage();

        // Демонстрация с отчетами
        Console.WriteLine("\n2. Генерация отчетов:");
        Console.WriteLine(new string('=', 60));

        ReportGenerator salesReport = new SalesReportGenerator();
        salesReport.GenerateReport();

        Console.WriteLine();

        ReportGenerator employeeReport = new EmployeeReportGenerator();
        employeeReport.GenerateReport();

        // Демонстрация с настройкой поведения через переопределение
        Console.WriteLine("\n3. Отчет с настройками (без заголовка):");
        Console.WriteLine(new string('=', 60));

        ReportGenerator salesReportNoHeader =
            new SalesReportGenerator(includeHeader: false, includeFooter: true);

        salesReportNoHeader.GenerateReport();
    }

    public static void TestVisitor()
    {
        Circle circle1 = new(5, 0, 0);
        Circle circle2 = new(3, 10, 10);
        Rectangle rect1 = new(4, 6, 20, 0);
        Triangle tri1 = new(3, 4, 5, 30, 0);

        Group group1 = new();
        group1.Add(circle1);
        group1.Add(rect1);

        Group mainGroup = new();
        mainGroup.Add(circle2);
        mainGroup.Add(tri1);
        mainGroup.Add(group1);

        // 1. Посетитель для расчета площади
        Console.WriteLine("1. Расчет площади всех фигур:");
        Console.WriteLine(new string('-', 50));
        AreaCalculator areaCalculator = new();
        mainGroup.Accept(areaCalculator);
        Console.WriteLine($"\nОбщая площадь: {areaCalculator.GetTotalArea():F2}");

        // 2. Посетитель для расчета периметра
        Console.WriteLine("\n2. Расчет периметра всех фигур:");
        Console.WriteLine(new string('-', 50));
        PerimeterCalculator perimeterCalculator = new();
        mainGroup.Accept(perimeterCalculator);
        Console.WriteLine($"\nОбщий периметр: {perimeterCalculator.GetTotalPerimeter():F2}");

        // 3. Посетитель для масштабирования
        Console.WriteLine("\n3. Масштабирование всех фигур (коэффициент 2):");
        Console.WriteLine(new string('-', 50));
        ScaleVisitor scaleVisitor = new(2);
        mainGroup.Accept(scaleVisitor);

        // Повторный расчет площади после масштабирования
        Console.WriteLine("\n   Площадь после масштабирования:");
        AreaCalculator areaAfterScale = new();
        mainGroup.Accept(areaAfterScale);
        Console.WriteLine($"   Общая площадь: {areaAfterScale.GetTotalArea():F2}");

        // 4. Посетитель для экспорта в XML
        Console.WriteLine("\n4. Экспорт в XML:");
        Console.WriteLine(new string('-', 50));
        XmlExportVisitor xmlExport = new();
        mainGroup.Accept(xmlExport);
        Console.WriteLine(xmlExport.GetXml());

        // 5. Посетитель для подсчета фигур
        Console.WriteLine("\n5. Подсчет фигур по типам:");
        Console.WriteLine(new string('-', 50));
        CountVisitor countVisitor = new();
        mainGroup.Accept(countVisitor);
        countVisitor.DisplayCounts();

        // Демонстрация: добавление новой операции без изменения классов фигур
        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("\nДобавление новой операции (отрисовка) без изменения классов фигур:");
        Console.WriteLine(new string('-', 50));

        // Можно легко добавить нового посетителя для отрисовки
        DrawVisitor drawVisitor = new();
        mainGroup.Accept(drawVisitor);
    }
}
