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

## Ba design pattern

| Pattern | Vị trí sử dụng | Lợi ích |
| --- | --- | --- |
| Factory | `Factory/DeviceFactory.cs`, phương thức `Create()` | Tập trung việc chọn và tạo thiết bị; client làm việc qua `IDevice`. |
| Singleton | `Config/HomeConfiguration.cs`, thuộc tính `Instance` | Dùng chung một đối tượng cấu hình cho toàn ứng dụng. |
| Facade | `Facade/SmartHomeFacade.cs`, phương thức `GoToWork()` và `GoToSleep()` | Client gọi một phương thức để thực hiện một nhóm thao tác trên thiết bị. |

Một hạn chế của thiết kế: cấu hình Singleton có trạng thái dùng chung có thể thay đổi, nên các lần kiểm thử có thể ảnh hưởng nhau nếu không khôi phục cấu hình.

## Phần C: Factory

1. Factory che giấu việc chọn lớp thiết bị cụ thể và gọi constructor tương ứng. Client chỉ truyền loại, tên và nhận `IDevice`.
2. Nếu thêm `SmartCamera`, cần tạo lớp thiết bị mới và thêm nhánh tương ứng trong `DeviceFactory.Create()`. Client chỉ cần thay đổi nếu muốn sử dụng camera; không cần sửa các lớp thiết bị hiện có.

## Phần D: Singleton

1. Constructor phải là `private` để client không tự tạo thêm đối tượng cấu hình.
2. `sealed` ngăn kế thừa lớp cấu hình, giữ thiết kế Singleton rõ ràng và không bị mở rộng qua lớp con.
3. Singleton gây khó cho unit test vì trạng thái dùng chung có thể ảnh hưởng giữa các test và khó thay thế bằng cấu hình riêng cho từng test.
4. Nếu dùng .NET Dependency Injection với `AddSingleton`, container có thể quản lý một thể hiện dùng chung trong container đó; không nhất thiết phải tự cài Singleton. Bài này vẫn dùng `Lazy<T>` theo mẫu giảng viên.

## Phần G: Câu hỏi trả lời

1. **Factory che giấu quyết định thiết kế nào?** Việc chọn lớp cụ thể (`SmartLight`, `SmartFan`, `SmartDoor`) và cách khởi tạo đối tượng dựa trên chuỗi loại thiết bị.
2. **Singleton bảo đảm điều gì, nhưng tạo ra rủi ro nào?** Bảo đảm dùng chung một thể hiện `HomeConfiguration`; rủi ro là trạng thái toàn cục có thể thay đổi và gây ảnh hưởng giữa các lần kiểm thử.
3. **Facade khác Factory ở mục đích chính như thế nào?** Factory phục vụ việc tạo đối tượng; Facade cung cấp thao tác mức cao để đơn giản hóa việc sử dụng nhiều đối tượng.
4. **Nếu bỏ `SmartHomeFacade`, client phải biết thêm gì?** Client phải tự gọi bật/tắt/khóa từng thiết bị theo từng chế độ, biết thứ tự thao tác và tự đọc, thông báo mức sáng từ cấu hình.

## Kiểm thử T1–T5

Các tình huống nằm trong `Program.cs`, chạy bằng `dotnet run`, không dùng framework kiểm thử.

| Mã | Kiểm tra | Kết quả mong đợi |
| --- | --- | --- |
| T1 | Factory tạo light, fan, door | Đúng ba kiểu thiết bị; in `T1: PASS`. |
| T2 | Tạo loại camera chưa hỗ trợ | Ném `ArgumentException`; in `T2: PASS`. |
| T3 | Lấy `HomeConfiguration.Instance` hai lần | In `True`; ném ngoại lệ nếu hai tham chiếu khác nhau. |
| T4 | Gọi `GoToWork()` | Log đèn tắt, quạt tắt, cửa khóa. |
| T5 | Mở khóa cửa rồi gọi `GoToSleep()` | Quạt tắt, cửa khóa lại, đèn bật, hiện mức sáng `30%`; in `T5: PASS`. |
