# lab10 
Ứng dụng console mô phỏng nhà thông minh, sử dụng .NET 10.

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

### 1. Factory đang che giấu quyết định thiết kế nào?

- `Factory` che giấu quyết định chọn lớp thiết bị cụ thể dựa trên chuỗi loại. Client chỉ cần gọi `DeviceFactory.Create()` để tạo thiết bị, còn cách lựa chọn và khởi tạo đối tượng nằm bên trong factory. Khi bổ sung loại thiết bị mới, phần lựa chọn được cập nhật tại factory; các đoạn client đang dùng `IDevice` không cần thay đổi nếu vẫn sử dụng các loại hiện có.

### 2. Singleton bảo đảm điều gì, nhưng tạo ra rủi ro nào?

- `Singleton` bảo đảm chỉ có một thể hiện `HomeConfiguration` được dùng chung và không cho phép khởi tạo trực tiếp từ bên ngoài. Rủi ro chính là khó viết unit test vì trạng thái toàn cục có thể thay đổi: một test sửa giá trị cấu hình thì test chạy sau có thể nhận giá trị đã bị sửa, khiến các test phụ thuộc lẫn nhau nếu không khôi phục trạng thái.

### 3. Facade khác Factory ở mục đích chính như thế nào?

- `Factory` giúp client tạo đối tượng mà không cần biết lớp cụ thể hay cách khởi tạo. `Facade` giúp client điều khiển nhiều thiết bị thông qua một thao tác đơn giản, chẳng hạn gọi `GoToWork()` hoặc `GoToSleep()` thay vì tự gọi từng thiết bị.

### 4. Nếu bỏ SmartHomeFacade, client sẽ phải biết thêm những chi tiết nào?

- Nếu bỏ `SmartHomeFacade`, client phải tự gọi bật, tắt và khóa các thiết bị theo đúng thứ tự cho từng chế độ. Client cũng phải biết `HomeConfiguration` có thuộc tính `NightLightLevel` để lấy và in mức sáng ban đêm. Khi thay đổi chế độ Đi ngủ, mọi nơi tự thực hiện chuỗi thao tác đó đều phải được cập nhật theo.

