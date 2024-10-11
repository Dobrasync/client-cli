using Lamashare.CLI.Db;
using Lamashare.CLI.Db.Entities;
using File = Lamashare.CLI.Db.Entities.File;

namespace LamashareApi.Database.Repos;

public class RepoWrapper(LamashareContext context) : IRepoWrapper
{
    private IRepo<SystemSetting> _systemSettingRepo = null!;
    private IRepo<Library> _libraryRepo = null!;
    private IRepo<File> _fileRepo = null!;

    public LamashareContext DbContext => context;

    #region Repos
    public IRepo<SystemSetting> SystemSettingRepo
    {
        get { return _systemSettingRepo ??= new Repo<SystemSetting>(context); }
    }

    public IRepo<Library> LibraryRepo
    {
        get { return _libraryRepo ??= new Repo<Library>(context); }
    }

    public IRepo<File> FileRepo
    {
        get { return _fileRepo ??= new Repo<File>(context); }
    }
    #endregion
}