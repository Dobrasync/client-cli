using Lamashare.CLI.Db.Entities;

namespace Lamashare.CLI.Db.Repo;

public interface IRepoWrapper
{
    LamashareContext DbContext { get; }
    IRepo<SystemSettingEntity> SystemSettingRepo { get; }
    IRepo<LibraryEntity> LibraryRepo { get; }
    IRepo<FileEntity> FileRepo { get; }
    IRepo<BlockEntity> BlockRepo { get; }
}