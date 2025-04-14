using Microsoft.Extensions.Configuration;

namespace Business.Helpers;

public static class ConfigurationHelper
{
    public static IConfiguration Configuration;
    public static void Initialize(IConfiguration config)
    {
        Configuration = config;
    }
}