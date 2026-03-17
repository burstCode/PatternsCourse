namespace CreationalPatterns;

// Абстрактный класс продукта.
public abstract class Notification
{
    public abstract void Send(string message);
}

// Конкретные продукты.
public class EmailNotification : Notification
{
    public override void Send(string message) { Console.WriteLine($"Отправка Email: {message}"); }
}

public class SmsNotification : Notification
{
    public override void Send(string message) { Console.WriteLine($"Отправка SMS: {message}"); }
}

// Абстрактный класс создателя с фабричным методом.
public abstract class NotificationService
{
    public abstract Notification CreateNotification();

    public void NotifyUser(string message)
    {
        Notification notification = CreateNotification();
        notification.Send(message);
    }
}

public class EmailService : NotificationService
{
    public override Notification CreateNotification() { return new EmailNotification(); }
}

public class SmsService : NotificationService
{
    public override Notification CreateNotification() { return new SmsNotification(); }
}