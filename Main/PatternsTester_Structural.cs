using StructuralPatterns;

namespace Patterns;

public partial class PatternsTester
{
    public static void TestAdapter()
    {
        FahrenheitSensor sensor = new();
        ITemperatureSensor adaptedSensor = new TemperatureAdapter(sensor);

        double temperature = adaptedSensor.GetTemperatureCelsius();
        Console.WriteLine($"Температура по Цельсию: {temperature}");
    }

    public static void TestBridge()
    {
        SmartLight livingRoomLight = new(new WifiProtocol("192.168.1.101"));
        livingRoomLight.TurnOn();
        livingRoomLight.SetBrightness(75);
        Console.WriteLine(livingRoomLight.GetStatus());
        livingRoomLight.TurnOff();

        Console.WriteLine();

        SmartTeapot teapot = new(new BluetoothProtocol("AA:BB:CC:DD:EE:FF"));
        teapot.TurnOn();
        teapot.SetTemperature(97);
        Console.WriteLine(teapot.GetStatus());
        teapot.TurnOff();
    }

    public static void TestComposite()
    {
        Product laptop = new("Ноутбук", 75000);
        Product mouse = new("Мышь", 1500);
        Product keyboard = new("Клавиатура", 3000);
        Product headphones = new("Наушники", 4500);
        Product charger = new("Зарядное устройство", 2000);

        // Создаем набор "Компьютерный комплект" со скидкой 10%.
        ProductSet computerSet = new("Компьютерный комплект", 10);
        computerSet.Add(laptop);
        computerSet.Add(mouse);
        computerSet.Add(keyboard);

        // Создаем набор "Аксессуары".
        ProductSet accessories = new("Аксессуары");
        accessories.Add(headphones);
        accessories.Add(charger);

        // Создаем большой набор "Все для работы".
        ProductSet workSet = new("Все для работы", 5);
        workSet.Add(computerSet);
        workSet.Add(accessories);

        // Создаем подарочную упаковку для всего набора.
        GiftBox giftBox = new("С днем рождения, коллега!", workSet);

        // Здесь создаем список покупок без скидок.
        List<IOrderItem> shoppingCart = new()
        {
            laptop,
            mouse,
            headphones,
            new Product("Книга", 800)
        };

        // Работа со всеми структурами единообразная!!!
        Console.WriteLine("Заказ 1: Подарочный набор");
        Console.WriteLine($"Общая стоимость: {giftBox.GetPrice():C}");
        giftBox.Display();

        Console.WriteLine("\nЗаказ 2: Рабочий набор");
        Console.WriteLine($"Общая стоимость: {workSet.GetPrice():C}");
        workSet.Display();

        Console.WriteLine("\nЗаказ 3: Простой список товаров");
        decimal total = 0;

        // Безобразно, но единообразно. Очень красиво.
        foreach (IOrderItem item in shoppingCart)
        {
            item.Display();
            total += item.GetPrice();
        }
        Console.WriteLine($"Итого: {total:C}");
    }

    public static void TestDecorator()
    {
        // Базовое уведомление только по Email.
        INotifier simpleNotifier = new EmailNotifier("user@example.com");
        Console.WriteLine("Простое уведомление:");
        simpleNotifier.Send("Совещание в 15:00");

        Console.WriteLine();

        // Добавляем SMS.
        INotifier smsNotifier = new SmsNotifier(
            new EmailNotifier("user@example.com"),
            "+7-999-123-45-67"
        );
        Console.WriteLine("Уведомление с SMS:");
        smsNotifier.Send("Совещание в 15:00");

        Console.WriteLine();

        // Добавляем Telegram и брендирование.
        INotifier fullNotifier = new BrandingDecorator(
            new TelegramNotifier(
                new SmsNotifier(
                    new EmailNotifier("user@example.com"),
                    "+7-999-123-45-67"
                ),
                "@user_chat"
            ),
            "ООО Ромашка"
        );
        Console.WriteLine("Полный комплект уведомлений с брендированием:");
        fullNotifier.Send("Совещание в 15:00");

        Console.WriteLine();

        // Добавляем временную метку.
        INotifier superNotifier = new TimestampDecorator(fullNotifier);
        Console.WriteLine("Добавляем временную метку:");
        superNotifier.Send("Совещание в 15:00");
    }

    public static void TestFacade()
    {
        SmartHomeFacade home = new SmartHomeFacade();

        home.LeaveHome();
        home.ReturnHome();
        home.MovieNight();
        home.GoodNight();

        home.QuickSetup(withMusic: true, temperature: 24);
    }

    public static void TestFlyweight()
    {
        RoadSignFactory signFactory = new();

        List<RoadSign> signs = new();
        Random rand = new();

        RoadSignType stopSign = signFactory.GetSignType("Восьмиугольник", "Красный", "СТОП");
        RoadSignType yieldSign = signFactory.GetSignType("Треугольник", "Красный", "Уступи дорогу");
        RoadSignType speedLimitSign = signFactory.GetSignType("Круг", "Белый", "50");

        for (int i = 0; i < 15; i++)
        {
            // Чередуем знаки, случайно выбираем день/ночь
            RoadSignType type = i % 3 == 0 ? stopSign : (i % 3 == 1 ? yieldSign : speedLimitSign);
            signs.Add(new RoadSign(i * 50, 400, rand.Next(2) == 1, type));
        }

        Console.WriteLine($"\nВсего создано знаков: {signs.Count}");
        Console.WriteLine($"Уникальных типов знаков: {signFactory.Count()}");
    }

    public static void TestProxy()
    {
        // 1. Виртуальный заместитель — загрузка по требованию
        Console.WriteLine("1. Виртуальный прокси:");

        IImage image1 = new VirtualImageProxy("family_photo.jpg");

        Console.WriteLine("Создали прокси, реальное изображение еще не загружено.");
        Console.WriteLine($"Информация: {image1.GetInfo()}"); // Информация из кэша

        Console.WriteLine("\nТеперь пытаемся отобразить изображение:");
        image1.Display(); // Здесь произойдет реальная загрузка

        Console.WriteLine("\nОтображаем второй раз — загрузка не нужна:");
        image1.Display();

        Console.WriteLine("\n" + new string('-', 60));

        // 2. Защитный заместитель — контроль доступа
        Console.WriteLine("\n2. Защитный прокси:");

        IImage secretImage = new HighResolutionImage("top_secret.jpg");

        IImage proxyForUser = new ProtectedImageProxy(secretImage, "User");
        IImage proxyForAdmin = new ProtectedImageProxy(secretImage, "Admin");

        Console.WriteLine("\nПопытка пользователя с ролью User:");
        proxyForUser.Display();

        Console.WriteLine("\nПопытка администратора:");
        proxyForAdmin.Display();

        Console.WriteLine("\n" + new string('-', 60));

        // 3. Кэширующий заместитель — подсчет обращений
        Console.WriteLine("\n3. Кэширующий прокси:");
        Console.WriteLine("------------------------------------");

        IImage cacheProxy = new CachingImageProxy("vacation.jpg");

        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine($"\nОбращение #{i + 1}:");
            cacheProxy.Display();
            Thread.Sleep(500);
        }

        Console.WriteLine($"\nИтоговая информация: {cacheProxy.GetInfo()}");
    }
}
