namespace BehaviouralPatterns;

// Интерфейс состояния.
public interface IOrderState
{
    void Next(Order order);
    void Previous(Order order);
    void Cancel(Order order);
    string GetStatus();
}

// Контекст — заказ, чье состояние меняется.
public class Order
{
    private IOrderState _currentState;
    private List<string> _items = new();
    private decimal _totalAmount;

    public Order() => _currentState = new NewOrderState();

    public void SetState(IOrderState state)
    {
        _currentState = state;
        Console.WriteLine($"[Заказ] Состояние изменено на: {GetStatus()}");
    }

    public void Next() => _currentState.Next(this);

    public void Previous() => _currentState.Previous(this);

    public void Cancel() => _currentState.Cancel(this);

    public string GetStatus() => _currentState.GetStatus();

    public void AddItem(string item, decimal price)
    {
        if (_currentState is NewOrderState || _currentState is PendingOrderState)
        {
            _items.Add(item);
            _totalAmount += price;
            Console.WriteLine($"[Заказ] Добавлен товар: {item} (${price})");
        }
        else
        {
            Console.WriteLine($"[Заказ] Невозможно добавить товар в состоянии {GetStatus()}");
        }
    }

    public void ShowInfo()
    {
        Console.WriteLine($"\n=== Заказ ===");
        Console.WriteLine($"Статус: {GetStatus()}");
        Console.WriteLine($"Товары: {(_items.Count > 0 ? string.Join(", ", _items) : "нет")}");
        Console.WriteLine($"Сумма: ${_totalAmount}");
        Console.WriteLine("==============\n");
    }

    public List<string> GetItems() => _items;
    public decimal GetTotalAmount() => _totalAmount;
}

// --- Конкретные состояния //
// 1. Новый заказ.
public class NewOrderState : IOrderState
{
    public void Next(Order order)
    {
        Console.WriteLine("[Состояние: Новый] Переход к обработке заказа");
        order.SetState(new PendingOrderState());
    }

    public void Previous(Order order)
    {
        Console.WriteLine("[Состояние: Новый] Это первое состояние, назад нельзя");
    }

    public void Cancel(Order order)
    {
        Console.WriteLine("[Состояние: Новый] Заказ отменен");
        order.SetState(new CancelledOrderState());
    }

    public string GetStatus() => "🟡 Новый заказ";
}

// 2. Заказ в обработке.
public class PendingOrderState : IOrderState
{
    public void Next(Order order)
    {
        if (order.GetItems().Count == 0)
        {
            Console.WriteLine("[Состояние: Обработка] Невозможно подтвердить заказ без товаров!");
        }
        else
        {
            Console.WriteLine("[Состояние: Обработка] Заказ подтвержден, ожидает оплаты");
            order.SetState(new PaymentPendingState());
        }
    }

    public void Previous(Order order)
    {
        Console.WriteLine("[Состояние: Обработка] Возврат к новому заказу");
        order.SetState(new NewOrderState());
    }

    public void Cancel(Order order)
    {
        Console.WriteLine("[Состояние: Обработка] Заказ отменен");
        order.SetState(new CancelledOrderState());
    }

    public string GetStatus() => "🔵 В обработке";
}

// 3. Ожидание оплаты
public class PaymentPendingState : IOrderState
{
    public void Next(Order order)
    {
        Console.WriteLine("[Состояние: Ожидание оплаты] Оплата получена, заказ собран");
        order.SetState(new ShippedOrderState());
    }

    public void Previous(Order order)
    {
        Console.WriteLine("[Состояние: Ожидание оплаты] Возврат к обработке заказа");
        order.SetState(new PendingOrderState());
    }

    public void Cancel(Order order)
    {
        Console.WriteLine("[Состояние: Ожидание оплаты] Заказ отменен до оплаты");
        order.SetState(new CancelledOrderState());
    }

    public string GetStatus() => "🟠 Ожидает оплаты";
}

// 4. Заказ отправлен.
public class ShippedOrderState : IOrderState
{
    public void Next(Order order)
    {
        Console.WriteLine("[Состояние: Отправлен] Заказ доставлен получателю");
        order.SetState(new DeliveredOrderState());
    }

    public void Previous(Order order)
    {
        Console.WriteLine("[Состояние: Отправлен] Невозможно вернуть отправленный заказ");
    }

    public void Cancel(Order order)
    {
        Console.WriteLine("[Состояние: Отправлен] Отмена невозможна, заказ уже отправлен");
    }

    public string GetStatus() => "🟣 Отправлен";
}

// 5. Заказ доставлен.
public class DeliveredOrderState : IOrderState
{
    public void Next(Order order)
    {
        Console.WriteLine("[Состояние: Доставлен] Заказ завершен. Спасибо за покупку!");
        order.SetState(new CompletedOrderState());
    }

    public void Previous(Order order)
    {
        Console.WriteLine("[Состояние: Доставлен] Невозможно вернуть доставленный заказ без оформления возврата");
    }

    public void Cancel(Order order)
    {
        Console.WriteLine("[Состояние: Доставлен] Возврат товара оформлен");
        order.SetState(new ReturnedOrderState());
    }

    public string GetStatus() => "🟢 Доставлен";
}

// 6. Заказ завершен
public class CompletedOrderState : IOrderState
{
    public void Next(Order order)
    {
        Console.WriteLine("[Состояние: Завершен] Это финальное состояние");
    }

    public void Previous(Order order)
    {
        Console.WriteLine("[Состояние: Завершен] Нельзя вернуться из завершенного состояния");
    }

    public void Cancel(Order order)
    {
        Console.WriteLine("[Состояние: Завершен] Невозможно отменить завершенный заказ");
    }

    public string GetStatus() => "✅ Завершен";
}

// 7. Заказ отменен
public class CancelledOrderState : IOrderState
{
    public void Next(Order order)
    {
        Console.WriteLine("[Состояние: Отменен] Нельзя продолжить отмененный заказ");
    }

    public void Previous(Order order)
    {
        Console.WriteLine("[Состояние: Отменен] Нельзя вернуться из отмененного состояния");
    }

    public void Cancel(Order order)
    {
        Console.WriteLine("[Состояние: Отменен] Заказ уже отменен");
    }

    public string GetStatus() => "❌ Отменен";
}

// 8. Заказ возвращен
public class ReturnedOrderState : IOrderState
{
    public void Next(Order order)
    {
        Console.WriteLine("[Состояние: Возврат] Возврат обработан, деньги возвращены");
        order.SetState(new CompletedOrderState());
    }

    public void Previous(Order order)
    {
        Console.WriteLine("[Состояние: Возврат] Нельзя вернуться после возврата");
    }

    public void Cancel(Order order)
    {
        Console.WriteLine("[Состояние: Возврат] Процесс возврата уже запущен");
    }

    public string GetStatus() => "↩️ Возврат оформлен";
}
