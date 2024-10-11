using System.ComponentModel.DataAnnotations;

namespace Lamashare.CLI.Db.Entities;

public class Library : BaseEntity
{
    /// <summary>
    /// Path to the directory in which the library is stored locally. 
    /// </summary>
    [Required, MinLength(1), MaxLength(4096)]
    public string LocalPath { get; set; } = default!;

    [Required]
    public Guid RemoteId { get; set; }

    public List<File> Files { get; set; } = new();
}