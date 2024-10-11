using Lamashare.CLI.Db.Entities;
using Lamashare.CLI.Db.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using File = Lamashare.CLI.Db.Entities.File;

namespace Lamashare.CLI.Db;

public class LamashareContext : DbContext
{
    public LamashareContext(DbContextOptions<LamashareContext> options) : base(options) {}
    
    public virtual DbSet<SystemSetting> SystemSettings { get; set; }
    public virtual DbSet<Library> Libraries { get; set; }
    public virtual DbSet<File> Files { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        Seed(builder);
    }

    private void Seed(ModelBuilder builder)
    {
        foreach (var e in Enum.GetValues<ESystemSetting>())
        {
            builder.Entity<SystemSetting>().HasData(
                new SystemSetting()
                {
                    Id = e.ToString(),
                    Value = null
                }    
            );
            
        }
    }
}