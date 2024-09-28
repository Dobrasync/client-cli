using Lamashare.CLI.Db.Enums;

namespace Lamashare.CLI.Services.SystemSetting;

public interface ISystemSettingService
{
    public Task<List<Db.Entities.SystemSetting>> GetAllSystemSettingsAsync();
    public Task<Db.Entities.SystemSetting> SetSettingAsync(ESystemSetting key, string newValue);
    public Task<Db.Entities.SystemSetting?> TryGetSettingAsync(ESystemSetting key);
    public Task<Db.Entities.SystemSetting?> GetSettingThrowsAsync(ESystemSetting key);
}