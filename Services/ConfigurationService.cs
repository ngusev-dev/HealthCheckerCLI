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
            if (!CheckConfigurationExsist()) throw new Exception("The configuration file was not found");

            try
            {
                var yamlFile = await File.ReadAllTextAsync(CONFIG_PATH);
                var config = _deserializer.Deserialize<HealthCheckConfiguration>(yamlFile);

                if (config is null) throw new Exception("Empty file");

                return config;
            } 
            catch
            {
                throw new Exception("Error when processing the configuration file");
            }
            
       }

        private bool CheckConfigurationExsist()
        {
            FileInfo fileInfo = new(CONFIG_PATH);

            return fileInfo.Exists;
        }
    }
}
