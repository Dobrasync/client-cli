using Lamashare.CLI.Db;
using Lamashare.CLI.Db.Entities;

namespace LamashareApi.Database.Repos;

public class RepoWrapper(LamashareContext context) : IRepoWrapper
{
    private IRepo<SystemSettingEntity> _systemSettingRepo = null!;
    private IRepo<LibraryEntity> _libraryRepo = null!;
    private IRepo<FileEntity> _fileRepo = null!;

    public LamashareContext DbContext => context;

    #region Repos
    public IRepo<SystemSettingEntity> SystemSettingRepo
    {
        get { return _systemSettingRepo ??= new Repo<SystemSettingEntity>(context); }
    }

    public IRepo<LibraryEntity> LibraryRepo
    {
        get { return _libraryRepo ??= new Repo<LibraryEntity>(context); }
    }

    public IRepo<FileEntity> FileRepo
    {
        get { return _fileRepo ??= new Repo<FileEntity>(context); }
    }
    #endregion
}