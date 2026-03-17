namespace CreationalPatterns;

// Абстрактные продукты-интерфейсы для каждого типа юнитов.
public interface IWarrior { void Attack(); }
public interface IMage { void CastSpell(); }
public interface IArcher { void Shoot(); }

// Конкретные продукты для семейства людей.
public class HumanWarrior : IWarrior
{
    public void Attack() { Console.WriteLine("Человек-воин атакует мечом"); }
}

public class HumanMage : IMage
{
    public void CastSpell() { Console.WriteLine("Человек-маг произносит заклинание огненного шара"); }
}

public class HumanArcher : IArcher
{
    public void Shoot() { Console.WriteLine("Человек-лучник стреляет из длинного лука"); }
}

// Конкретные продукты для семейства орков.
public class OrcWarrior : IWarrior
{
    public void Attack() { Console.WriteLine("Орк-воин крушит все топором"); }
}

public class OrcMage : IMage
{
    public void CastSpell() { Console.WriteLine("Шаман орков призывает духов предков"); }
}

public class OrcArcher : IArcher
{
    public void Shoot() { Console.WriteLine("Орк-лучник стреляет из грубого лука"); }
}

// Абстрактная фабрика описывает интерфейс для создания семейства продуктов.
public interface ICharacterFactory
{
    IWarrior CreateWarrior();
    IMage CreateMage();
    IArcher CreateArcher();
}

// Конкретная фабрика для создания юнитов человеческого рода.
public class HumanFactory : ICharacterFactory
{
    public IWarrior CreateWarrior() => new HumanWarrior();
    public IMage CreateMage() => new HumanMage();
    public IArcher CreateArcher() => new HumanArcher();
}

// Конкретная фабрика для создания юнитов семейства орков.
public class OrcFactory : ICharacterFactory
{
    public IWarrior CreateWarrior() => new OrcWarrior();
    public IMage CreateMage() => new OrcMage();
    public IArcher CreateArcher() => new OrcArcher();
}

// Клиентский код.

public class Game
{
    private IWarrior _warrior;
    private IMage _mage;
    private IArcher _archer;

    public Game(ICharacterFactory factory)
    {
        _warrior = factory.CreateWarrior();
        _mage = factory.CreateMage();
        _archer = factory.CreateArcher();
    }

    public void StartBattle()
    {
        Console.WriteLine("Битва начинается!");
        _warrior.Attack();
        _mage.CastSpell();
        _archer.Shoot();
    }
}
