using Lamashare.CLI.Db.Entities;
using Lamashare.CLI.Db.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Lamashare.CLI.Db;

public class LamashareContext : DbContext
{
    public LamashareContext(DbContextOptions<LamashareContext> options) : base(options) {}
    
    public virtual DbSet<SystemSettingEntity> SystemSettings { get; set; }
    public virtual DbSet<LibraryEntity> Libraries { get; set; }
    public virtual DbSet<FileEntity> Files { get; set; }
    public virtual DbSet<BlockEntity> Blocks { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        Seed(builder);
    }

    private void Seed(ModelBuilder builder)
    {
        foreach (var e in Enum.GetValues<ESystemSetting>())
        {
            builder.Entity<SystemSettingEntity>().HasData(
                new SystemSettingEntity()
                {
                    Id = e.ToString(),
                    Value = null
                }    
            );
            
        }
    }
}