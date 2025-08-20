using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using HealthCheckerCLI.Interfaces;

namespace HealthCheckerCLI.Services
{
    public class YamlService
    {
        private const string CONFIG_PATH = "./config/config.yaml";

        private readonly IDeserializer _deserializer;

        public YamlService()
        {
            _deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
        }

       public async Task<Dictionary<string, HealthCheckEntry>?> GetDeserializeConfig()
        {
            try
            {
                var yamlFile = await File.ReadAllTextAsync(CONFIG_PATH);

                var config = _deserializer.Deserialize<Dictionary<string, HealthCheckEntry>>(yamlFile);

                return config;
            } 
            catch
            {
                return null;
            }
            
        }
    }
}
