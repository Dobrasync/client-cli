using Lamashare.CLI.Db;
using Lamashare.CLI.Db.Entities;

namespace LamashareApi.Database.Repos;

public interface IRepoWrapper
{
    LamashareContext DbContext { get; }
    IRepo<SystemSettingEntity> SystemSettingRepo { get; }
    IRepo<LibraryEntity> LibraryRepo { get; }
    IRepo<FileEntity> FileRepo { get; }
}