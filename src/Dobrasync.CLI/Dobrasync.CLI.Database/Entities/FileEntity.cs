using Lamashare.CLI.Db.Entities.Base;

namespace Lamashare.CLI.Db.Entities;

public class FileEntity : BaseEntity
{
    public LibraryEntity LibraryEntity { get; set; } = default!;
    public string FileLibraryPath { get; set; } = default!;
    public string TotalChecksum { get; set; } = default!;
    public DateTimeOffset DateModified { get; set; } = default!;
    public DateTimeOffset DateCreated { get; set; } = default!;
    public List<BlockEntity> Blocks { get; set; } = new();
}