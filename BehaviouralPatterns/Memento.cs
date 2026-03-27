namespace BehaviouralPatterns;

using System.Text;

// Хранитель — содержит снимок состояния
public class TextMemento
{
    private string _content;
    private int _cursorPosition;
    private DateTime _timestamp;

    public TextMemento(string content, int cursorPosition)
    {
        _content = content;
        _cursorPosition = cursorPosition;
        _timestamp = DateTime.Now;
    }

    // "Широкий" интерфейс — доступен только хозяину
    internal string GetContent() => _content;
    internal int GetCursorPosition() => _cursorPosition;

    // "Узкий" интерфейс — доступен всем
    public DateTime GetTimestamp() => _timestamp;
    public string GetSummary() => $"{_timestamp:HH:mm:ss} | Длина текста: {_content.Length} символов";
}

// Хозяин — объект, состояние которого нужно сохранять
public class TextEditor_Memento
{
    private StringBuilder _content;
    private int _cursorPosition;

    public TextEditor_Memento()
    {
        _content = new StringBuilder();
        _cursorPosition = 0;
    }

    public void Type(string text)
    {
        _content.Insert(_cursorPosition, text);
        _cursorPosition += text.Length;
        Console.WriteLine($"[Редактор] Введено: \"{text}\"");
    }

    public void Delete(int length)
    {
        if (_cursorPosition > 0 && _cursorPosition - length >= 0)
        {
            string deleted = _content.ToString(_cursorPosition - length, length);
            _content.Remove(_cursorPosition - length, length);
            _cursorPosition -= length;
            Console.WriteLine($"[Редактор] Удалено: \"{deleted}\"");
        }
    }

    public void MoveCursor(int position)
    {
        if (position >= 0 && position <= _content.Length)
        {
            _cursorPosition = position;
            Console.WriteLine($"[Редактор] Курсор перемещен на позицию {position}");
        }
    }

    public void Display()
    {
        string content = _content.ToString();
        Console.WriteLine($"\n--- Текст редактора ---");
        if (string.IsNullOrEmpty(content))
        {
            Console.WriteLine("(пусто)");
        }
        else
        {
            Console.WriteLine(content);
            Console.WriteLine(new string(' ', _cursorPosition) + "^");
            Console.WriteLine($"Курсор: позиция {_cursorPosition}");
        }
        Console.WriteLine("------------------------\n");
    }

    public string GetContent() => _content.ToString();
    public int GetCursorPosition() => _cursorPosition;

    // Создание хранителя (сохраняем состояние)
    public TextMemento CreateMemento()
    {
        Console.WriteLine($"[Редактор] СОХРАНЕНИЕ состояния");
        return new TextMemento(_content.ToString(), _cursorPosition);
    }

    // Восстановление состояния из хранителя
    public void RestoreMemento(TextMemento memento)
    {
        Console.WriteLine($"[Редактор] ВОССТАНОВЛЕНИЕ состояния от {memento.GetTimestamp():HH:mm:ss}");
        _content = new StringBuilder(memento.GetContent());
        _cursorPosition = memento.GetCursorPosition();
    }
}

// Посыльный — отвечает за хранение истории
public class HistoryManager
{
    private Stack<TextMemento> _undoStack = new();
    private Stack<TextMemento> _redoStack = new();
    private TextEditor_Memento _editor;

    public HistoryManager(TextEditor_Memento editor)
    {
        _editor = editor;
    }

    public void SaveState()
    {
        // Сохраняем текущее состояние
        _undoStack.Push(_editor.CreateMemento());
        // При новом действии очищаем стек повтора
        _redoStack.Clear();
        Console.WriteLine($"История: {_undoStack.Count} сохраненных состояний");
    }

    public void Undo()
    {
        if (_undoStack.Count > 0)
        {
            // Сохраняем текущее состояние в стек повтора
            _redoStack.Push(_editor.CreateMemento());

            // Восстанавливаем предыдущее состояние
            TextMemento previousState = _undoStack.Pop();
            _editor.RestoreMemento(previousState);
            Console.WriteLine($"Отмена выполнена. Осталось состояний: {_undoStack.Count}");
        }
        else
        {
            Console.WriteLine("❌ Нечего отменять!");
        }
    }

    public void Redo()
    {
        if (_redoStack.Count > 0)
        {
            // Сохраняем текущее состояние в стек отмены
            _undoStack.Push(_editor.CreateMemento());

            // Восстанавливаем следующее состояние
            TextMemento nextState = _redoStack.Pop();
            _editor.RestoreMemento(nextState);
            Console.WriteLine($"Повтор выполнен. Осталось повторов: {_redoStack.Count}");
        }
        else
        {
            Console.WriteLine("❌ Нечего повторять!");
        }
    }

    public void ShowHistory()
    {
        Console.WriteLine("\n--- История сохраненных состояний ---");
        if (_undoStack.Count == 0)
        {
            Console.WriteLine("(пусто)");
        }
        else
        {
            int index = _undoStack.Count;
            foreach (var memento in _undoStack.Reverse())
            {
                Console.WriteLine($"{index--}. {memento.GetSummary()}");
            }
        }
        Console.WriteLine();
    }
}
