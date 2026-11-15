/****************/
// 202418944
// Bùi Nhật Minh
/****************/
public class SmartFan : IDevice
{
    private string name;
    public bool IsOn;

    public SmartFan(string name)
    {
        this.name = name;
    }

    public void TurnOn()
    {
        IsOn = true;
        Console.WriteLine(name + ": ON");
    }

    public void TurnOff()
    {
        IsOn = false;
        Console.WriteLine(name + ": OFF");
    }
}
