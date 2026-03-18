namespace StructuralPatterns;

// Базовый интерфейс для всех объектов
// иерархической структуры.
public interface IOrderItem
{
    string GetName();
    decimal GetPrice();
    void Display(int indent = 0);
}

// Простой объект, не содержит вложенных элементов.
public class Product : IOrderItem
{
    private string _name;
    private decimal _price;

    public Product(string name, decimal price)
    {
        _name = name;
        _price = price;
    }

    public string GetName() => _name;

    public decimal GetPrice() => _price;

    public void Display(int indent = 0)
    {
        Console.WriteLine($"{new string(' ', indent)}- {_name}: {_price:C}");
    }
}

// Составной компонент, может содержать другие объекты.
public class ProductSet : IOrderItem
{
    private string _name;
    private List<IOrderItem> _items = new List<IOrderItem>();
    private decimal _discount = 0; // Скидка на набор в процентах.

    public ProductSet(string name, decimal discount = 0)
    {
        _name = name;
        _discount = discount;
    }

    public string GetName() => _name;
    public void Add(IOrderItem item) => _items.Add(item);
    public void Remove(IOrderItem item) => _items.Remove(item);

    public IOrderItem GetChild(int index) => _items[index];

    public decimal GetPrice()
    {
        decimal total = 0;
        foreach (IOrderItem item in _items)
            total += item.GetPrice();

        // Применяем скидку к общей стоимости набора.
        return total * (1 - _discount / 100);
    }

    public void Display(int indent = 0)
    {
        Console.WriteLine($"{new string(' ', indent)}+ {_name} (скидка {_discount}%, итого: {GetPrice():C})");
        foreach (IOrderItem item in _items)
            item.Display(indent + 2);
    }
}

// Cоставной объект для подарочной упаковки.
public class GiftBox : IOrderItem
{
    private string _message;
    private IOrderItem _content;
    private decimal _wrappingPrice = 50; // Стоимость упаковки.

    public GiftBox(string message, IOrderItem content)
    {
        _message = message;
        _content = content;
    }

    public string GetName() => $"Подарок с надписью: \"{_message}\"";

    public decimal GetPrice() => _content.GetPrice() + _wrappingPrice;

    public void Display(int indent = 0)
    {
        Console.WriteLine($"{new string(' ', indent)}* Подарочная упаковка (+{_wrappingPrice:C}):");
        Console.WriteLine($"{new string(' ', indent + 2)}Сообщение: \"{_message}\"");
        _content.Display(indent + 2);
        Console.WriteLine($"{new string(' ', indent)}  Итого в подарке: {GetPrice():C}");
    }
}
