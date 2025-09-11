using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using HealthCheckerCLI.Helpers;

namespace HealthCheckerCLI.Services
{
    public class ConfigurationService
    {
        private const string CONFIG_PATH = "./health-cli.yaml";

        private readonly IDeserializer _deserializer;

        public ConfigurationService()
        {
            _deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .IgnoreUnmatchedProperties()
                .Build();
        }

       public async Task<HealthCheckConfiguration?> GetDeserializeConfig()
       {
            try
            {
                if(!CheckConfigurationExsist()) return null;

                var yamlFile = await File.ReadAllTextAsync(CONFIG_PATH);
                var config = _deserializer.Deserialize<HealthCheckConfiguration>(yamlFile);

                return config;
            } 
            catch
            {
                return null;
            }
            
       }

        private bool CheckConfigurationExsist()
        {
            FileInfo fileInfo = new(CONFIG_PATH);

            return fileInfo.Exists;
        }
    }
}
