namespace Lamashare.CLI.Const;

public abstract class Constants
{
    public static readonly string AppHomeFolderName = ".lamashare";
    public static readonly string AppHomeFolderPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), AppHomeFolderName);
    public static readonly string AppSqliteFileName = "database.sqlite";
    public static readonly string AppSqliteFilePath = Path.Join(AppHomeFolderPath, AppSqliteFileName);
}