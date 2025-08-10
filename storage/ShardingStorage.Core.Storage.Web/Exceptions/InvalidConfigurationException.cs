namespace ShardingStorage.Core.Storage.Web.Exceptions;

internal class InvalidConfigurationException : Exception
{
    public InvalidConfigurationException()
        : base("Invalid configuration")
    {
    }
}