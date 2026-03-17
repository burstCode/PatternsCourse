using CreationalPatterns;

namespace Patterns;

public class PatternTester
{
    public static void TestSingleton()
    {
        // Получаем доступ к единственному экземпляру.
        Singleton singleton = Singleton.Instance();

        // Вызываем метод, выполняющий
        // какую-то полезную работу.
        singleton.UsefulMethod();

        // Создадим еще один объект и получим экземпляр Singleton.
        Singleton singleton2 = Singleton.Instance();

        // Выведем хеш-сумму, при расчете она учитывает ссылку на объект.
        Console.WriteLine($"\nХеш-сумма объекта {nameof(singleton)}: {singleton.GetHashCode()}");
        Console.WriteLine($"Хеш-сумма объекта {nameof(singleton2)}: {singleton2.GetHashCode()}");

        // Чтобы не считать руками, вызовем метод для сравнения ссылок.
        if (ReferenceEquals(singleton, singleton2))
            Console.WriteLine("\nСсылка совпадает!");
    }

    public static void TestBuilder()
    {
        Cook cook = new();

        // Готовим комплексный обед!
        MealBuilder fullBuilder = new FullMealBuilder();
        cook.Prepare(fullBuilder);
        Meal fullMeal = fullBuilder.GetMeal();
        Console.WriteLine("Комплексный обед:");
        fullMeal.Show();

        Console.WriteLine();

        // Готовим легкий перекус!
        MealBuilder lightBuilder = new LightMealBuilder();
        cook.Prepare(lightBuilder);
        Meal lightMeal = lightBuilder.GetMeal();
        Console.WriteLine("Легкий перекус:");
        lightMeal.Show();
    }

    public static void TestPrototype()
    {
        // Создадим фабрику прототипов.
        EnemyFactory factory = new();

        // Регистрируем базовые прототипы: боевой робот...
        Robot battleRobot = new("X-100", "Квантовый ЦП", new() { "лазер", "ракеты" });
        factory.RegisterPrototype("battleRobot", battleRobot);

        // ...и гоблин.
        Enemy basicEnemy = new("Гоблин", 50, 10);
        factory.RegisterPrototype("goblin", basicEnemy);

        // Клонируем прототип робота и модифицируем его.
        Robot robotClone = (Robot)factory.Create("battleRobot");
        robotClone.Model = "X-200";
        robotClone.Weapons.Add("плазма");

        // Клонируем прототипы... гоблинов.
        Enemy goblinClone1 = (Enemy)factory.Create("goblin");
        Enemy goblinClone2 = (Enemy)factory.Create("goblin");

        Console.WriteLine("Оригинальный робот:");
        battleRobot.ShowInfo();

        Console.WriteLine("\nКлон робота с изменениями:");
        robotClone.ShowInfo();

        Console.WriteLine("\nДва клона гоблина:");
        goblinClone1.ShowInfo();
        goblinClone2.ShowInfo();

        // Стоит отметить, что это разные объекты, где каждый
        // клон прототипа может быть сконфигурирован по-своему.
        Console.WriteLine("\nРезультат сравнения ссылок на двух гоблинов: "
            + (ReferenceEquals(goblinClone1, goblinClone2) ? "ссылки равны." : "ссылки не равны."));
    }

    public static void TestAbstractFactory()
    {
        Console.WriteLine("Игра за людей:");
        Game humanGame = new(new HumanFactory());
        humanGame.StartBattle();

        Console.WriteLine("\nИгра за орков:");
        Game orcGame = new(new OrcFactory());
        orcGame.StartBattle();
    }
}
