using System.ComponentModel.DataAnnotations;

namespace UserRegistration.Storage.Contracts;

public sealed class StorageConfigurationSettings
{
    [Required]
    [MinLength(1)]
    public string ConnectionString { get; init; } = string.Empty;
}