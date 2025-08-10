using System.Text.Json;
using ShardingStorage.Client.Configurations;
using ShardingStorage.Client.Exceptions;

namespace ShardingStorage.Client.Tools;

internal static class ConfigurationReader
{
    public static Configuration ReadFromFile(string fileName)
    {
        using var stream = File.OpenRead(fileName);

        var configuration = JsonSerializer.Deserialize<Configuration>(stream);

        if (configuration is null)
            throw new ConfigurationReaderException();

        return configuration;
    }

    public static Configuration ReadFromEnvironment()
    {
        var coreBaseAddress = Environment.GetEnvironmentVariable("CORE_BASE_ADDRESS");

        if (coreBaseAddress is null)
            throw new ConfigurationReaderException();

        return new Configuration
        {
            CoreBaseAddress = coreBaseAddress
        };
    }
}