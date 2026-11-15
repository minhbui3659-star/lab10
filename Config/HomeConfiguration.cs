/****************/
// 202418944
// Bùi Nhật Minh
/****************/
public sealed class HomeConfiguration
{
    private static readonly Lazy<HomeConfiguration> lazy =
        new(() => new HomeConfiguration());

    public static HomeConfiguration Instance => lazy.Value;

    public string HomeName { get; set; } = "My Home";
    public int NightLightLevel { get; set; } = 30;

    private HomeConfiguration() { }
}
