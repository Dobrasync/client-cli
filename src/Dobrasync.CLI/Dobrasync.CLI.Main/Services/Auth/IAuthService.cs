namespace Lamashare.CLI.Services.Auth;

public interface IAuthService
{
    public Task Authenticate();
    public Task Logout();
}