# lab10

Ứng dụng console mô phỏng nhà thông minh, sử dụng .NET 8.

## Cấu trúc thư mục

```text
Config/                 # HomeConfiguration
Devices/                # IDevice, ILockable, SmartLight, SmartFan, SmartDoor
Facade/                 # SmartHomeFacade
Factory/                # DeviceFactory
Program.cs              # Điểm khởi chạy và các tình huống T1–T5
diagram.puml            # Sơ đồ lớp PlantUML
SmartHomeMini.csproj
```

## Chạy ứng dụng

Cài .NET SDK 8, sau đó chạy từ thư mục gốc repository:

```sh
dotnet build
dotnet run
```
