using Microsoft.Extensions.Configuration;

namespace AipsCore.Infrastructure.DI.Configuration;

public static class ConfigurationEnvExtensions
{
    private const string DbConnStringKey = "DB_CONN_STRING";

    public static string GetEnvConnectionString(this IConfiguration configuration)
    {
        return configuration.GetEnvForSure(DbConnStringKey);
    }

    private static string GetEnvForSure(this IConfiguration configuration, string key)
    {
        var value = configuration[key];

        if (value is null)
        {
            throw new ConfigurationException(key);
        }
        
        return value;
    }
}