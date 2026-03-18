namespace StructuralPatterns;

// Интерфейс для классов реализации.
// Представляет примитивные операции.
public interface IDeviceProtocol
{
    void SendCommand(string command);
    string ReceiveData();
}

// Интерфейс абстракции.
// Хранит ссылку на объект-реализатор.
public abstract class SmartDevice
{
    // Хранит 
    protected IDeviceProtocol _protocol;

    protected SmartDevice(IDeviceProtocol protocol)
    {
        _protocol = protocol;
    }

    public abstract void TurnOn();
    public abstract void TurnOff();
    public abstract string GetStatus();
}

// ----------------------------------------

// Конкретные реализации для разных протоколов связи.
public class WifiProtocol : IDeviceProtocol
{
    private string _deviceIp;

    public WifiProtocol(string ip)
    {
        _deviceIp = ip;
    }

    public void SendCommand(string command)
    {
        Console.WriteLine($"[WiFi] Отправка команды <{command}> на устройство {_deviceIp}.");
    }

    public string ReceiveData()
    {
        Console.WriteLine($"[WiFi] Получение данных от {_deviceIp}.");

        // Имитируем полученные данные от умного чайника.
        return "temperature:97";
    }
}

public class BluetoothProtocol : IDeviceProtocol
{
    private string _macAddress;

    public BluetoothProtocol(string macAddress)
    {
        _macAddress = macAddress;
    }

    public void SendCommand(string command)
    {
        Console.WriteLine($"[Bluetooth] Отправка команды <{command}> устройству {_macAddress}.");
    }

    public string ReceiveData()
    {
        Console.WriteLine($"[Bluetooth] Получение данных от {_macAddress}.");

        // Имитируем полученные данные от умной лампочки.
        return "brightness:75";
    }
}

// Конкретные типы умных устройств.
public class SmartLight : SmartDevice
{
    private int _brightness = 100;

    public SmartLight(IDeviceProtocol protocol) : base(protocol) { }

    public override void TurnOn()
    {
        _protocol.SendCommand("light_on");
        Console.WriteLine("Лампочка включена.");
    }

    public override void TurnOff()
    {
        _protocol.SendCommand("light_off");
        Console.WriteLine("Лампочка выключена.");
    }

    public override string GetStatus()
    {
        string data = _protocol.ReceiveData();
        return $"Умная лампочка, яркость {_brightness}%, данные: {data}.";
    }

    public void SetBrightness(int level)
    {
        _brightness = level;
        _protocol.SendCommand($"set_brightness:{level}");
        Console.WriteLine($"Яркость установлена на {level}%.");
    }
}

public class SmartTeapot : SmartDevice
{
    private double _temperature = 40.0;

    public SmartTeapot(IDeviceProtocol protocol) : base(protocol) { }

    public override void TurnOn()
    {
        _protocol.SendCommand("teapot_on");
        Console.WriteLine("Умный чайник включен.");
    }

    public override void TurnOff()
    {
        _protocol.SendCommand("teapot_off");
        Console.WriteLine("Умный чайник выключен.");
    }

    public void SetTemperature(double temp)
    {
        _temperature = temp;
        _protocol.SendCommand($"set_temp:{temp}");
        Console.WriteLine($"Температура установлена на {temp}°C.");
    }

    public override string GetStatus()
    {
        string data = _protocol.ReceiveData();
        return $"Умный чайник, температура {_temperature}°C, данные: {data}.";
    }
}
