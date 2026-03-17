namespace CreationalPatterns;

// Продукт - то, что мы собираем.
public class Meal
{
    public string? MainDish { get; set; }
    public string? SideDish { get; set; }
    public string? Drink { get; set; }
    public string? Dessert { get; set; }

    public void Show()
    {
        Console.WriteLine($"Основное блюдо: {MainDish ?? "нет"}");
        Console.WriteLine($"Гарнир: {SideDish ?? "нет"}");
        Console.WriteLine($"Напиток: {Drink ?? "нет"}");
        Console.WriteLine($"Десерт: {Dessert ?? "нет"}");
    }
}

// Абстрактный строитель - определяет интерфейс для всех строителей.
public abstract class MealBuilder
{
    protected Meal _meal = new();

    public abstract void BuildMainDish();
    public abstract void BuildSideDish();
    public abstract void BuildDrink();
    public abstract void BuildDessert();

    public Meal GetMeal() => _meal;
}

// Конкретный строитель для комплексного обеда.
public class FullMealBuilder : MealBuilder
{
    public override void BuildMainDish() { _meal.MainDish = "Стейк"; }
    public override void BuildSideDish() { _meal.SideDish = "Картофель фри"; }

    public override void BuildDrink() { _meal.Drink = "Кола"; }

    public override void BuildDessert() { _meal.Dessert = "Чизкейк"; }
}

// Конкретный строитель для легкого перекуса.
public class LightMealBuilder : MealBuilder
{
    public override void BuildMainDish() { _meal.MainDish = "Салат"; }
    public override void BuildSideDish() { /* Без гарнира */ }

    public override void BuildDrink() { _meal.Drink = "Вода"; }

    public override void BuildDessert() { _meal.Dessert = "Фрукт"; }
}

// Распорядитель - управляет процессом сборки.
public class Cook
{
    public void Prepare(MealBuilder builder)
    {
        builder.BuildMainDish();
        builder.BuildSideDish();
        builder.BuildDrink();
        builder.BuildDessert();
    }
}
