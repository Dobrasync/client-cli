using System.ComponentModel.DataAnnotations;

namespace Lamashare.CLI.Db.Entities;

public class Remote : BaseEntity
{
    [Required, MinLength(1), MaxLength(256)]
    public string Url { get; set; } = default!;
}