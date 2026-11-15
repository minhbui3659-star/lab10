public class SmartLight : IDevice
{
    private string name;
    public bool IsOn;

    public SmartLight(string name)
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
