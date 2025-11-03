using Microsoft.EntityFrameworkCore;
using UserRegistration.Storage.Contracts;
using UserRegistration.Storage.Postgres.Entities;

namespace UserRegistration.Storage.Postgres;

internal sealed class EventStoreDbContext(
    DbContextOptions<EventStoreDbContext> options, 
    PostgresStorageConfigurationSettings settings) : DbContext(options)
{
    public DbSet<EventStoreEntity> Events { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(settings.ConnectionString, npgsql =>
        {
            npgsql.MigrationsAssembly(typeof(EventStoreDbContext).Assembly.GetName().Name);
            npgsql.MigrationsHistoryTable(settings.MigrationsTableName, settings.MigrationsSchemaName);
            
            if (settings.CommandTimeout.HasValue)
            {
                npgsql.CommandTimeout(settings.CommandTimeout.Value);
            }
        });
        
        if (settings.EnableSensitiveDataLogging)
        {
            options.EnableSensitiveDataLogging();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EventStoreEntity>(entity =>
        {
            entity.ToTable("events");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName(nameof(EventStoreEntity.Id))
                .UseIdentityAlwaysColumn();

            entity.Property(e => e.AggregateId)
                .HasColumnName(nameof(EventStoreEntity.AggregateId))
                .IsRequired();

            entity.Property(e => e.Type)
                .HasColumnName(nameof(EventStoreEntity.Type))
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(e => e.Body)
                .HasColumnName(nameof(EventStoreEntity.Body))
                .HasColumnType("jsonb")
                .IsRequired();

            entity.Property(e => e.Version)
                .HasColumnName(nameof(EventStoreEntity.Version))
                .IsRequired();

            entity.Property(e => e.OccurredAt)
                .HasColumnName(nameof(EventStoreEntity.OccurredAt))
                .IsRequired();

            entity.HasIndex(e => e.AggregateId)
                .HasDatabaseName("IxEventsAggregateId");

            entity.HasIndex(e => new { e.AggregateId, e.Version })
                .IsUnique()
                .HasDatabaseName("IxEventsAggregateIdVersion");
        });
    }
}
