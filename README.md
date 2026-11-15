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

### 1. Factory đang che giấu quyết định thiết kế nào?

- `Factory` che giấu quyết định chọn lớp thiết bị cụ thể dựa trên chuỗi loại. Client chỉ cần gọi `DeviceFactory.Create()` để tạo thiết bị, còn cách lựa chọn và khởi tạo đối tượng nằm bên trong factory. Khi bổ sung loại thiết bị mới, phần lựa chọn được cập nhật tại factory; các đoạn client đang dùng `IDevice` không cần thay đổi nếu vẫn sử dụng các loại hiện có.

### 2. Singleton bảo đảm điều gì, nhưng tạo ra rủi ro nào?

- `Singleton` bảo đảm chỉ có một thể hiện `HomeConfiguration` được dùng chung và không cho phép khởi tạo trực tiếp từ bên ngoài. Rủi ro chính là khó viết unit test vì trạng thái toàn cục có thể thay đổi: một test sửa giá trị cấu hình thì test chạy sau có thể nhận giá trị đã bị sửa, khiến các test phụ thuộc lẫn nhau nếu không khôi phục trạng thái.

### 3. Facade khác Factory ở mục đích chính như thế nào?

- `Factory` giúp client tạo đối tượng mà không cần biết lớp cụ thể hay cách khởi tạo. `Facade` giúp client điều khiển nhiều thiết bị thông qua một thao tác đơn giản, chẳng hạn gọi `GoToWork()` hoặc `GoToSleep()` thay vì tự gọi từng thiết bị.

### 4. Nếu bỏ SmartHomeFacade, client sẽ phải biết thêm những chi tiết nào?

- Nếu bỏ `SmartHomeFacade`, client phải tự gọi bật, tắt và khóa các thiết bị theo đúng thứ tự cho từng chế độ. Client cũng phải biết `HomeConfiguration` có thuộc tính `NightLightLevel` để lấy và in mức sáng ban đêm. Khi thay đổi chế độ Đi ngủ, mọi nơi tự thực hiện chuỗi thao tác đó đều phải được cập nhật theo.

## Kiểm thử T1–T5

Các tình huống nằm trong `Program.cs`, chạy bằng `dotnet run`, không dùng framework kiểm thử.

| Mã | Kiểm tra | Kết quả mong đợi |
| --- | --- | --- |
| T1 | Factory tạo light, fan, door | Đúng ba kiểu thiết bị; in `T1: PASS`. |
| T2 | Tạo loại camera chưa hỗ trợ | Ném `ArgumentException`; in `T2: PASS`. |
| T3 | Lấy `HomeConfiguration.Instance` hai lần | In `True`; ném ngoại lệ nếu hai tham chiếu khác nhau. |
| T4 | Gọi `GoToWork()` | Log đèn tắt, quạt tắt, cửa khóa. |
| T5 | Mở khóa cửa rồi gọi `GoToSleep()` | Quạt tắt, cửa khóa lại, đèn bật, hiện mức sáng `30%`; in `T5: PASS`. |
