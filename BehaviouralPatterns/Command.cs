using System.Text;

// Интерфейс команды.
public interface ICommand
{
    void Execute();
    void Undo();

    // Это для того, чтобы записывать описание действий.
    string Description { get; }
}

// Получатель — объект, над которым выполняются действия.
public class TextEditor
{
    private StringBuilder _text = new();
    private int _selectionStart;
    private int _selectionLength;

    public void InsertText(string text, int? position = null)
    {
        if (position is null)
        {
            _text.Append(text);
            Console.WriteLine($"[Редактор] Вставка '{text}' в конец строки.");
            return;
        }

        Console.WriteLine($"[Редактор] Вставка '{text}' на позицию {position}.");
        _text.Insert(position.Value, text);
    }

    public void DeleteText(int? position, int length)
    {
        if (position is null)
            position = _text.Length - length;

        string deleted = _text.ToString(position.Value, length);
        Console.WriteLine($"[Редактор] Удаление '{deleted}' с позиции {position}.");
        _text.Remove(position.Value, length);
    }

    public void SetSelection(int start, int length)
    {
        _selectionStart = start;
        _selectionLength = length;
        Console.WriteLine($"[Редактор] Выделен текст на позиции {start} длиной {length}.");
    }

    public string GetSelectedText() => _text.ToString(_selectionStart, _selectionLength);

    public string GetFullText() => _text.ToString();

    public void Display() => Console.WriteLine($"Текст в редакторе:\n\"{_text}\"\n");
}

// Конкретная команда для вставки текста.
public class InsertTextCommand : ICommand
{
    private TextEditor _editor;
    private string _text;
    private int? _position = null;
    private string _description;

    public InsertTextCommand(TextEditor editor, string text, int? position = null)
    {
        _editor = editor;
        _text = text;
        _position = position;

        if (position is null)
        {
            _description = $"Вставка '{text}' в конец строки";
        }
        else
        {
            _description = $"Вставка '{text}' в позицию {position}";
        }
    }

    public void Execute() => _editor.InsertText(_text, _position);

    public void Undo() => _editor.DeleteText(_position, _text.Length);

    public string Description => _description;
}

// Конкретная команда для удаления текста.
public class DeleteTextCommand : ICommand
{
    private TextEditor _editor;
    private int _position;
    private int _length;
    private string _deletedText = string.Empty;
    private string _description;

    public DeleteTextCommand(TextEditor editor, int position, int length)
    {
        _editor = editor;
        _position = position;
        _length = length;
        _description = $"Удаление {length} символов с позиции {position}";
    }

    public void Execute()
    {
        _deletedText = _editor.GetFullText().Substring(_position, _length);
        _editor.DeleteText(_position, _length);
    }

    public void Undo() => _editor.InsertText(_deletedText, _position);

    public string Description => _description;
}

// Конкретная команда для замены выделенного текста.
public class ReplaceSelectionCommand : ICommand
{
    private TextEditor _editor;
    private string _newText;
    private string _oldText = string.Empty;
    private int _position;
    private int _length;
    private string _description;

    public ReplaceSelectionCommand(TextEditor editor, string newText)
    {
        _editor = editor;
        _newText = newText;
        _description = $"Замена выделенного текста на '{newText}'";
    }

    public void Execute()
    {
        _oldText = _editor.GetSelectedText();
        _position = _editor.GetFullText().IndexOf(_oldText);
        _length = _oldText.Length;

        _editor.DeleteText(_position, _length);
        _editor.InsertText(_newText, _position);
    }

    public void Undo()
    {
        _editor.DeleteText(_position, _newText.Length);
        _editor.InsertText(_oldText, _position);
    }

    public string Description => _description;
}

// Составная команда — позволяет выполнять несколько команд как одну.
public class MacroCommand : ICommand
{
    private List<ICommand> _commands = new List<ICommand>();
    private string _description;

    public MacroCommand(string description) => _description = description;

    public void AddCommand(ICommand command) => _commands.Add(command);

    public void RemoveCommand(ICommand command) => _commands.Remove(command);

    public void Execute()
    {
        Console.WriteLine($"[Макрос] Выполнение: {_description}");
        foreach (var command in _commands)
            command.Execute();
    }

    public void Undo()
    {
        Console.WriteLine($"[Макрос] Отмена: {_description}");
        for (int i = _commands.Count - 1; i >= 0; i--)
            _commands[i].Undo();
    }

    public string Description => _description;
}

// Инициатор — отвечает за выполнение команд и историю.
public class CommandInvoker
{
    private Stack<ICommand> _history = new();
    private Stack<ICommand> _redoStack = new();

    public void ExecuteCommand(ICommand command)
    {
        Console.WriteLine($"\n>>> Выполнение: {command.Description}");
        command.Execute();
        _history.Push(command);
        _redoStack.Clear();
    }

    public void Undo()
    {
        if (_history.Count > 0)
        {
            ICommand command = _history.Pop();
            Console.WriteLine($"\n<<< Отмена: {command.Description}");
            command.Undo();
            _redoStack.Push(command);
            Console.WriteLine($"История: {_history.Count} команд");
        }
        else
        {
            Console.WriteLine("\n❌ Нечего отменять!");
        }
    }

    public void Redo()
    {
        if (_redoStack.Count > 0)
        {
            ICommand command = _redoStack.Pop();
            Console.WriteLine($"\n>>> Повтор: {command.Description}");
            command.Execute();
            _history.Push(command);
            Console.WriteLine($"История: {_history.Count} команд");
        }
        else
        {
            Console.WriteLine("\n❌ Нечего повторять!");
        }
    }

    public void ShowHistory()
    {
        Console.WriteLine("\n--- История выполненных команд ---");
        int index = 1;
        foreach (var cmd in _history.Reverse())
            Console.WriteLine($"{index++}. {cmd.Description}");

        if (_history.Count == 0)
            Console.WriteLine("(пусто)");
    }
}
