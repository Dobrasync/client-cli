namespace Lamashare.CLI.Services.Sync;

public interface ISyncService
{
    public void Login();

    public void Logout();

    public void CloneLibrary(Guid libraryId);

    public void RemoveLibrary(Guid libraryId);
}