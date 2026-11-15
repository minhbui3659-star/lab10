public static class DeviceFactory
{
    public static IDevice Create(string type, string name)
    {
        // TODO: chua xu ly khoang trang thua o dau/cuoi chuoi
        switch (type.ToLower())
        {
            case "light": return new SmartLight(name);
            case "fan": return new SmartFan(name);
            case "door": return new SmartDoor(name);
            default: throw new ArgumentException("Unsupported device type: " + type);
        }
    }
}
