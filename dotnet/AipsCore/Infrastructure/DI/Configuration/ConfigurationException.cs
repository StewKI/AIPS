namespace AipsCore.Infrastructure.DI.Configuration;

public class ConfigurationException : Exception
{
    public ConfigurationException(string key)
        : base($"Env configuration error, key '{key}' not set.")
    {
    }
}