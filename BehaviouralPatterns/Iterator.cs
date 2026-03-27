namespace BehaviouralPatterns;

// Интерфейс итератора.
public interface IIterator<T>
{
    void First();           // Перейти к первому элементу.
    void Next();            // Перейти к следующему элементу.
    bool IsDone();          // Закончился ли обход?
    T? CurrentItem();       // Получить текущий элемент.
}

// Интерфейс агрегата (коллекции).
public interface IAggregate<T>
{
    IIterator<T> CreateIterator();
}

// Конкретная коллекция — список книг.
public class BookCollection : IAggregate<string>
{
    private string[] _books;
    private int _count;

    public BookCollection(int capacity = 10)
    {
        _books = new string[capacity];
        _count = 0;
    }

    public void AddBook(string title)
    {
        if (_count < _books.Length)
        {
            _books[_count] = title;
            _count++;
        }
        else
        {
            Console.WriteLine("Коллекция заполнена!");
        }
    }

    public string? GetBook(int index)
    {
        if (index >= 0 && index < _count)
            return _books[index];

        return null;
    }

    public int Count => _count;

    public IIterator<string> CreateIterator() => new BookIterator(this);

    // Создаем итератор для обхода в обратном порядке.
    public IIterator<string> CreateReverseIterator() => new ReverseBookIterator(this);
}

// Конкретный итератор для прямого обхода.
public class BookIterator : IIterator<string>
{
    private BookCollection _collection;
    private int _currentIndex;

    public BookIterator(BookCollection collection)
    {
        _collection = collection;
        _currentIndex = -1;
    }

    public void First() => _currentIndex = 0;

    public void Next()
    {
        if (!IsDone())
            _currentIndex++;
    }

    public bool IsDone() => _currentIndex >= _collection.Count;

    public string? CurrentItem()
    {
        if (IsDone())
            throw new Exception("Итерация завершена");

        return _collection.GetBook(_currentIndex);
    }
}

// Итератор для обратного обхода.
public class ReverseBookIterator : IIterator<string>
{
    private BookCollection _collection;
    private int _currentIndex;

    public ReverseBookIterator(BookCollection collection)
    {
        _collection = collection;
        _currentIndex = _collection.Count;
    }

    public void First() => _currentIndex = _collection.Count - 1;

    public void Next()
    {
        if (!IsDone())
            _currentIndex--;
    }

    public bool IsDone() => _currentIndex < 0;

    public string? CurrentItem()
    {
        if (IsDone())
            throw new Exception("Итерация завершена");

        return _collection.GetBook(_currentIndex);
    }
}

// Другая коллекция — музыкальный плейлист (реализован как список).
public class Playlist : IAggregate<string>
{
    private List<string> _songs = new();

    public void AddSong(string song) => _songs.Add(song);

    public string? GetSong(int index)
    {
        if (index >= 0 && index < _songs.Count)
            return _songs[index];

        return null;
    }

    public int Count => _songs.Count;

    public IIterator<string> CreateIterator() => new PlaylistIterator(this);
}

// Итератор для плейлиста.
public class PlaylistIterator : IIterator<string>
{
    private Playlist _playlist;
    private int _currentIndex;

    public PlaylistIterator(Playlist playlist)
    {
        _playlist = playlist;
        _currentIndex = -1;
    }

    public void First() => _currentIndex = 0;

    public void Next()
    {
        if (!IsDone())
            _currentIndex++;
    }

    public bool IsDone() => _currentIndex >= _playlist.Count;

    public string? CurrentItem()
    {
        if (IsDone())
            throw new Exception("Итерация завершена");

        return _playlist.GetSong(_currentIndex);
    }
}

// Внутренний итератор — сам управляет обходом и применяет функцию к каждому элементу.
public static class InternalIterator
{
    public static void ForEach<T>(IAggregate<T> aggregate, Action<T>? action)
    {
        if (action is null)
            return;

        IIterator<T> iterator = aggregate.CreateIterator();
        for (iterator.First(); !iterator.IsDone(); iterator.Next())
        {
            T? currentItem = iterator.CurrentItem();
            if (currentItem is null)
                throw new NullReferenceException("Элемент последовательности оказался null!");

            action(currentItem);
        }
    }
}
