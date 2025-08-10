namespace ShardingStorage.Client.Exceptions;

internal class ConfigurationReaderException : Exception
{
    public ConfigurationReaderException()
        : base("Unable to read configuration")
    {
    }
}