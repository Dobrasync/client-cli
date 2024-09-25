namespace Lamashare.CLI.Storage.Service;

public interface ISyncService
{
    public void Login();

    public void Logout();

    public void CloneLibrary(Guid libraryId);

    public void RemoveLibrary(Guid libraryId);
}