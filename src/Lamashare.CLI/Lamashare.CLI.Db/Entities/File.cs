namespace Lamashare.CLI.Db.Entities;

public class File : BaseEntity
{
    public Library Library { get; set; } = default!;
    public string FileLibraryPath { get; set; } = default!;
    public string TotalChecksum { get; set; } = default!;
}