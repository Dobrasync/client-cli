using Lamashare.CLI.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Lamashare.CLI.Db;

public class LamashareContext : DbContext
{
    public LamashareContext(DbContextOptions<LamashareContext> options) : base(options) {}
    
    public virtual DbSet<Remote> Remotes { get; set; }
    public virtual DbSet<Library> Libraries { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
    }
}