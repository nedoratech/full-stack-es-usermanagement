using System.ComponentModel.DataAnnotations;

namespace UserRegistration.Storage.Contracts;

public sealed class PostgresStorageConfigurationSettings
{
    public const string SectionName = "Storage:Postgres";
    
    [Required]
    [MinLength(1)]
    public string ConnectionString { get; init; } = string.Empty;
    
    [MinLength(1)]
    public string MigrationsTableName { get; init; } = "__EFMigrationsHistory";
    
    [MinLength(1)]
    public string MigrationsSchemaName { get; init; } = "public";
    
    [Range(1, int.MaxValue)]
    public int? CommandTimeout { get; init; }
    
    public bool EnableSensitiveDataLogging { get; init; } = false;
}
