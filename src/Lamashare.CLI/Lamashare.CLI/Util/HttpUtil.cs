namespace Lamashare.CLI.Util;

public abstract class HttpUtil
{
    public static HttpClient GetClient()
    {
        var client = new HttpClient();
        return client;
    }
}