using Lamashare.CLI.Db.Enums;

namespace Lamashare.CLI.Services.SystemSetting;

public interface ISystemSettingService
{
    public Task<List<Db.Entities.SystemSettingEntity>> GetAllSystemSettingsAsync();
    public Task<Db.Entities.SystemSettingEntity> SetSettingAsync(ESystemSetting key, string newValue);
    public Task<Db.Entities.SystemSettingEntity?> TryGetSettingAsync(ESystemSetting key);
    public Task<Db.Entities.SystemSettingEntity> GetSettingThrowsAsync(ESystemSetting key);
    public Task<string> GetSettingValueThrowsAsync(ESystemSetting key);
}