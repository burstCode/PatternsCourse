namespace StructuralPatterns;

// Внутреннее представление дорожного знака.
public class RoadSignType
{
    private string _shape;
    private string _color;
    private string _symbol;

    public RoadSignType(string shape, string color, string symbol)
    {
        _shape = shape;
        _color = color;
        _symbol = symbol;
    }

    public void Display(int x, int y, bool isNight)  // Внешнее состояние: координаты и время суток.
    {
        string timeOfDay = isNight ? "ночь" : "день";
        Console.WriteLine($"Знак '{_symbol}' ({_shape}, {_color}) в координатах ({x}, {y}) — сейчас {timeOfDay}");
    }
}

// Внешнее представление дорожного знака.
public class RoadSign
{
    private int _x, _y;
    private bool _isNight;
    private RoadSignType _type;

    public RoadSign(int x, int y, bool isNight, RoadSignType type)
    {
        _x = x;
        _y = y;
        _isNight = isNight;
        _type = type;
    }

    public void Display()
    {
        _type.Display(_x, _y, _isNight);
    }
}


public class RoadSignFactory
{
    private Dictionary<string, RoadSignType> _signTypes = new();

    public RoadSignType GetSignType(string shape, string color, string symbol)
    {
        string key = $"{shape}_{color}_{symbol}";

        if (!_signTypes.ContainsKey(key))
        {
            _signTypes[key] = new RoadSignType(shape, color, symbol);
            Console.WriteLine($"[Фабрика] Создан новый тип знака: {symbol}");
        }

        return _signTypes[key];
    }

    public int Count() => _signTypes.Count();
}
