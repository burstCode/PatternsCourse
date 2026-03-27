namespace BehaviouralPatterns;

// Абстрактное выражение.
public interface IExpression
{
    int Interpret(Context context);
}

// Контекст — хранит значения переменных.
public class Context
{
    private Dictionary<string, int> _variables = new();

    public void SetVariable(string name, int value) => _variables[name] = value;

    public int GetVariable(string name)
    {
        if (_variables.ContainsKey(name))
            return _variables[name];

        throw new Exception($"Переменная '{name}' не определена");
    }
}

// Терминальное выражение — число.
public class NumberExpression : IExpression
{
    private int _value;

    public NumberExpression(int value) => _value = value;

    public int Interpret(Context context) => _value;
}

// Терминальное выражение — переменная.
public class VariableExpression : IExpression
{
    private string _name;

    public VariableExpression(string name) => _name = name;

    public int Interpret(Context context) => context.GetVariable(_name);
}

// Нетерминальное выражение — сложение.
public class AddExpression : IExpression
{
    private IExpression _left;
    private IExpression _right;

    public AddExpression(IExpression left, IExpression right)
    {
        _left = left;
        _right = right;
    }

    public int Interpret(Context context) => _left.Interpret(context) + _right.Interpret(context);
}

// Нетерминальное выражение — вычитание.
public class SubtractExpression : IExpression
{
    private IExpression _left;
    private IExpression _right;

    public SubtractExpression(IExpression left, IExpression right)
    {
        _left = left;
        _right = right;
    }

    public int Interpret(Context context) => _left.Interpret(context) - _right.Interpret(context);
}

// Нетерминальное выражение — умножение.
public class MultiplyExpression : IExpression
{
    private IExpression _left;
    private IExpression _right;

    public MultiplyExpression(IExpression left, IExpression right)
    {
        _left = left;
        _right = right;
    }

    public int Interpret(Context context) => _left.Interpret(context) * _right.Interpret(context);
}

// Нетерминальное выражение — деление.
public class DivideExpression : IExpression
{
    private IExpression _left;
    private IExpression _right;

    public DivideExpression(IExpression left, IExpression right)
    {
        _left = left;
        _right = right;
    }

    public int Interpret(Context context)
    {
        int rightValue = _right.Interpret(context);
        if (rightValue == 0)
            throw new Exception("Деление на ноль!");

        return _left.Interpret(context) / rightValue;
    }
}

// Простой парсер для создания дерева выражений из строки.
public class ExpressionParser
{
    private int _position;
    private string _input = string.Empty;

    public IExpression Parse(string input)
    {
        _position = 0;
        _input = input.Replace(" ", "");
        return ParseExpression();
    }

    private IExpression ParseExpression()
    {
        IExpression left = ParseTerm();

        while (_position < _input.Length)
        {
            char op = _input[_position];
            if (op == '+' || op == '-')
            {
                _position++;
                IExpression right = ParseTerm();
                if (op == '+')
                    left = new AddExpression(left, right);
                else
                    left = new SubtractExpression(left, right);
            }
            else
            {
                break;
            }
        }
        return left;
    }

    private IExpression ParseTerm()
    {
        IExpression left = ParseFactor();

        while (_position < _input.Length)
        {
            char op = _input[_position];
            if (op == '*' || op == '/')
            {
                _position++;
                IExpression right = ParseFactor();
                if (op == '*')
                    left = new MultiplyExpression(left, right);
                else
                    left = new DivideExpression(left, right);
            }
            else
            {
                break;
            }
        }
        return left;
    }

    private IExpression ParseFactor()
    {
        if (_position >= _input.Length)
            throw new Exception("Неожиданный конец выражения");

        char current = _input[_position];

        if (char.IsDigit(current))
        {
            int start = _position;
            while (_position < _input.Length && char.IsDigit(_input[_position]))
                _position++;
            int value = int.Parse(_input.Substring(start, _position - start));
            return new NumberExpression(value);
        }
        else if (char.IsLetter(current))
        {
            // Переменная
            int start = _position;
            while (_position < _input.Length && char.IsLetterOrDigit(_input[_position]))
                _position++;
            string name = _input.Substring(start, _position - start);
            return new VariableExpression(name);
        }
        else if (current == '(')
        {
            _position++;
            IExpression expr = ParseExpression();
            if (_position < _input.Length && _input[_position] == ')')
            {
                _position++;
                return expr;
            }
            throw new Exception("Отсутствует закрывающая скобка");
        }

        throw new Exception($"Неожиданный символ: {current}");
    }
}
