/****************/
// 202418944
// Bùi Nhật Minh
/****************/
public class SmartDoor : IDevice, ILockable
{
    private string name;
    public bool IsOn;
    public bool IsLocked;

    public SmartDoor(string name)
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

    public void Lock()
    {
        IsLocked = true;
        Console.WriteLine(name + ": LOCKED");
    }

    public void Unlock()
    {
        IsLocked = false;
        Console.WriteLine(name + ": UNLOCKED");
    }
}
