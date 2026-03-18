namespace StructuralPatterns;

// Общий интерфейс для изображений.
public interface IImage
{
    void Display();
    string GetInfo();
    int GetSize();
}

// Реальный объект — тяжелое изображение, которое долго загружается.
public class HighResolutionImage : IImage
{
    private string _filename;
    private byte[]? _imageData;
    private int _width;
    private int _height;

    public HighResolutionImage(string filename)
    {
        _filename = filename;
        Console.WriteLine($"[Реальный объект] Начинаем загрузку изображения {filename}...");
        LoadImageFromDisk();
    }

    private void LoadImageFromDisk()
    {
        // Имитация долгой загрузки
        Thread.Sleep(2000);
        _imageData = new byte[1024 * 1024]; // 1 МБ данных
        _width = 1920;
        _height = 1080;
        Console.WriteLine($"[Реальный объект] Изображение {_filename} загружено. Размер: {GetSize()} пикселей");
    }

    public void Display()
    {
        Console.WriteLine($"[Реальный объект] Отображаем изображение {_filename} ({_width}x{_height})");
    }

    public string GetInfo()
    {
        return $"Изображение: {_filename}, размер: {_width}x{_height}";
    }

    public int GetSize()
    {
        return _width * _height;
    }
}

// Виртуальный заместитель — откладывает загрузку до реальной необходимости.
public class VirtualImageProxy : IImage
{
    private string _filename;
    private HighResolutionImage? _realImage;
    private int _cachedWidth = 1920;    // Кэшируем метаданные без загрузки всего изображения.
    private int _cachedHeight = 1080;

    public VirtualImageProxy(string filename)
    {
        _filename = filename;
        Console.WriteLine($"[Виртуальный прокси] Создан заместитель для {filename}. Загрузка отложена.");
    }

    private void EnsureImageLoaded()
    {
        if (_realImage == null)
        {
            Console.WriteLine($"[Виртуальный прокси] Первое обращение — загружаем реальное изображение");
            _realImage = new HighResolutionImage(_filename);
        }
    }

    public void Display()
    {
        EnsureImageLoaded();
        _realImage?.Display();
    }

    public string GetInfo()
    {
        // Можем вернуть информацию без загрузки полного изображения.
        return $"Изображение: {_filename}, размер: {_cachedWidth}x{_cachedHeight} (метаданные из прокси)";
    }

    public int GetSize()
    {
        // Тоже можем вернуть без загрузки.
        return _cachedWidth * _cachedHeight;
    }
}

// Защитный заместитель — контролирует права доступа.
public class ProtectedImageProxy : IImage
{
    private IImage _realImage;
    private string _userRole;
    private bool _isAdmin;

    public ProtectedImageProxy(IImage image, string userRole)
    {
        _realImage = image;
        _userRole = userRole;
        _isAdmin = (userRole == "Admin");
    }

    public void Display()
    {
        if (_isAdmin)
        {
            Console.WriteLine($"[Защитный прокси] Доступ разрешен для роли {_userRole}");
            _realImage.Display();
        }
        else
        {
            Console.WriteLine($"[Защитный прокси] ДОСТУП ЗАПРЕЩЕН! Роль {_userRole} не имеет прав на просмотр");
        }
    }

    public string GetInfo()
    {
        if (_isAdmin)
        {
            return _realImage.GetInfo();
        }
        else
        {
            return "[Защитный прокси] Информация недоступна для вашей роли";
        }
    }

    public int GetSize()
    {
        if (_isAdmin)
        {
            return _realImage.GetSize();
        }
        else
        {
            return 0;
        }
    }
}

// Кэширующий заместитель — сохраняет результаты для повторного использования.
public class CachingImageProxy : IImage
{
    private string _filename;
    private HighResolutionImage? _realImage;
    private DateTime? _lastAccessTime;
    private int _accessCount;

    public CachingImageProxy(string filename)
    {
        _filename = filename;
        _accessCount = 0;
        Console.WriteLine($"[Кэширующий прокси] Создан для {filename}");
    }

    private void LoadIfNeeded()
    {
        if (_realImage == null)
        {
            _realImage = new HighResolutionImage(_filename);
        }
        _lastAccessTime = DateTime.Now;
        _accessCount++;
    }

    public void Display()
    {
        LoadIfNeeded();
        Console.WriteLine($"[Кэширующий прокси] Отображение #{_accessCount}, последний доступ: {_lastAccessTime:HH:mm:ss}");
        _realImage?.Display();
    }

    public string GetInfo()
    {
        if (_realImage != null)
        {
            return _realImage.GetInfo() + $" (просмотров: {_accessCount})";
        }
        return $"Изображение: {_filename} (еще не загружено)";
    }

    public int GetSize()
    {
        if (_realImage != null)
        {
            return _realImage.GetSize();
        }
        return 1920 * 1080; // Возвращаем кэшированное значение.
    }
}
