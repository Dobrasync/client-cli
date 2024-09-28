using Lamashare.CLI.Db.Enums;
using Lamashare.CLI.Shared.Exceptions;
using LamashareApi.Database.Repos;

namespace Lamashare.CLI.Services.SystemSetting;

public class SystemSettingService(IRepoWrapper repoWrap, ILoggerService logger) : ISystemSettingService
{
    public async Task<List<Db.Entities.SystemSetting>> GetAllSystemSettingsAsync()
    {
        return await repoWrap.SystemSettingRepo.QueryAll().ToListAsync();
    }
    public async Task<Db.Entities.SystemSetting> SetSettingAsync(ESystemSetting key, string newValue)
    {
        var setting = await repoWrap.DbContext.SystemSettings.FirstOrDefaultAsync(x => x.Id == key.ToString());
        if (setting == null)
        {
            throw new EntityNotFoundException();
        }

        setting.Value = newValue;
        
        await repoWrap.SystemSettingRepo.UpdateAsync(setting);
        logger.LogInfo($"System setting {key} updated to '{newValue}'.");
        return setting;
    }
    public async Task<Db.Entities.SystemSetting?> TryGetSettingAsync(ESystemSetting key)
    {
        var setting = await repoWrap.SystemSettingRepo.QueryAll().FirstOrDefaultAsync(x => x.Id == key.ToString());
        return setting;
    }
    
    public async Task<Db.Entities.SystemSetting> GetSettingThrowsAsync(ESystemSetting key)
    {
        var set = await TryGetSettingAsync(key);
        if (set == null)
        {
            throw new KeyNotFoundException($"The setting for {key} was not found.");
        }
        
        return set;
    }
}