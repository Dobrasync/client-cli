using Lamashare.CLI.Db;
using Lamashare.CLI.Db.Entities;
using File = Lamashare.CLI.Db.Entities.File;

namespace LamashareApi.Database.Repos;

public interface IRepoWrapper
{
    LamashareContext DbContext { get; }
    IRepo<SystemSetting> SystemSettingRepo { get; }
    IRepo<Library> LibraryRepo { get; }
    IRepo<File> FileRepo { get; }
}