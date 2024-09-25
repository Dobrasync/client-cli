using System.ComponentModel.DataAnnotations;

namespace Lamashare.CLI.Db.Entities;

public class Library : BaseEntity
{
    /// <summary>
    /// Path to the directory in which the library is stored locally. 
    /// </summary>
    [Required, MinLength(1)]
    public string LocalPath { get; set; } = default!;
}