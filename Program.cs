/****************/
// 202418944
// Bùi Nhật Minh
/****************/
IDevice light = DeviceFactory.Create("light", "Living room light");
IDevice fan = DeviceFactory.Create("fan", "Living room fan");
IDevice door = DeviceFactory.Create("door", "Main door");
var facade = new SmartHomeFacade(light, fan, (ILockable)door);

// T1
Console.WriteLine("T1");
if (light is SmartLight && fan is SmartFan && door is SmartDoor)
    Console.WriteLine("T1: PASS");
else
    Console.WriteLine("T1: FAIL");

// T2
Console.WriteLine("T2");
try
{
    DeviceFactory.Create("camera", "cam");
    Console.WriteLine("T2: FAIL");
}
catch (ArgumentException)
{
    Console.WriteLine("T2: PASS");
}

// T3
Console.WriteLine("T3");
var c1 = HomeConfiguration.Instance;
var c2 = HomeConfiguration.Instance;
Console.WriteLine(ReferenceEquals(c1, c2));
if (!ReferenceEquals(c1, c2))
{
    throw new Exception("Singleton implementation is incorrect.");
}

// T4
Console.WriteLine("T4");
facade.GoToWork();

// T5
Console.WriteLine("T5");
((ILockable)door).Unlock();
facade.GoToSleep();
if (!((SmartFan)fan).IsOn && ((SmartDoor)door).IsLocked && ((SmartLight)light).IsOn)
    Console.WriteLine("T5: PASS");
else
    Console.WriteLine("T5: FAIL");
