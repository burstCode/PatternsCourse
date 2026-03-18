namespace StructuralPatterns;

// Базовый интерфейс объекта.
public interface INotifier
{
    void Send(string message);
}

// Базовая реализация конкретного объекта.
public class EmailNotifier : INotifier
{
    private string _email;

    public EmailNotifier(string email)
    {
        _email = email;
    }

    public void Send(string message)
    {
        Console.WriteLine($"Отправка Email на {_email}: {message}");
    }
}

// Базовый класс декоратора — реализует тот же интерфейс и хранит ссылку на объект.
public abstract class NotifierDecorator : INotifier
{
    protected INotifier _wrappee;

    public NotifierDecorator(INotifier notifier)
    {
        _wrappee = notifier;
    }

    public virtual void Send(string message)
    {
        // По умолчанию просто передаем вызов дальше.
        _wrappee.Send(message);
    }
}

// Декоратор, добавляющий SMS-уведомления.
public class SmsNotifier : NotifierDecorator
{
    private string _phoneNumber;

    public SmsNotifier(INotifier notifier, string phoneNumber) : base(notifier)
    {
        _phoneNumber = phoneNumber;
    }

    public override void Send(string message)
    {
        // Сначала выполняем базовую отправку
        base.Send(message);
        // Потом добавляем свое поведение
        Console.WriteLine($"Отправка SMS на {_phoneNumber}: {message}");
    }
}

// Декоратор, добавляющий Telegram-уведомления.
public class TelegramNotifier : NotifierDecorator
{
    private string _chatId;

    public TelegramNotifier(INotifier notifier, string chatId) : base(notifier)
    {
        _chatId = chatId;
    }

    public override void Send(string message)
    {
        base.Send(message);
        Console.WriteLine($"Отправка Telegram в чат {_chatId}: {message}");
    }
}

// Декоратор, добавляющий уведомления в Slack.
public class SlackNotifier : NotifierDecorator
{
    private string _channel;

    public SlackNotifier(INotifier notifier, string channel) : base(notifier)
    {
        _channel = channel;
    }

    public override void Send(string message)
    {
        base.Send(message);
        Console.WriteLine($"Отправка Slack в канал {_channel}: {message}");
    }
}

// Декоратор, добавляющий логотип компании к сообщению.
public class BrandingDecorator : NotifierDecorator
{
    private string _companyName;

    public BrandingDecorator(INotifier notifier, string companyName) : base(notifier)
    {
        _companyName = companyName;
    }

    public override void Send(string message)
    {
        string brandedMessage = $"[{_companyName}] {message}";
        base.Send(brandedMessage);
    }
}

// Декоратор, добавляющий временную метку.
public class TimestampDecorator : NotifierDecorator
{
    public TimestampDecorator(INotifier notifier) : base(notifier)
    {
    }

    public override void Send(string message)
    {
        string timestampedMessage = $"[{DateTime.Now:HH:mm:ss}] {message}";
        base.Send(timestampedMessage);
    }
}
