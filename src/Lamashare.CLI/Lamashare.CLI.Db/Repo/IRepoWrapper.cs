using Lamashare.CLI.Db;
using Lamashare.CLI.Db.Entities;

namespace LamashareApi.Database.Repos;

public interface IRepoWrapper
{
    LamashareContext DbContext { get; }
    IRepo<SystemSetting> SystemSettingRepo { get; }
    IRepo<Library> LibraryRepo { get; }
}