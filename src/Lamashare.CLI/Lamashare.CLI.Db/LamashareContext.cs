using Lamashare.CLI.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lamashare.CLI.Db;

public class LamashareContext : DbContext
{
    
    public virtual DbSet<Remote> Remotes { get; set; }
    public virtual DbSet<Library> Libraries { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
    }
}