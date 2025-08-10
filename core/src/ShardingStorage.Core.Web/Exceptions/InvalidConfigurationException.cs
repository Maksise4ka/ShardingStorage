namespace ShardingStorage.Core.Web.Exceptions;

internal class InvalidConfigurationException : Exception
{
    public InvalidConfigurationException()
        : base("Invalid configuration")
    {
    }
}