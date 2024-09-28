namespace Lamashare.CLI.Services.Sync;

public interface ISyncService
{
    public Task<int> Login();

    public Task<int> Logout();

    public Task<int> CloneLibrary(Guid libraryId, string localLibraryPath);

    public Task<int> RemoveLibrary(Guid libraryId, bool deleteDirectory);

    public Task<int> SyncLibrary(Guid localLibId);
}