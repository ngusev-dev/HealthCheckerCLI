using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using HealthCheckerCLI.Helpers;

namespace HealthCheckerCLI.Services
{
    public class ConfigurationService
    {
        private const string CONFIG_PATH = "./health-cli.yaml";
        private readonly IDeserializer _deserializer;
        private HealthCheckConfiguration? _configurationFile;

        public HealthCheckConfiguration? configurationFile
        {
            get { return this._configurationFile; }
        }

        public ConfigurationService()
        {
            _deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .IgnoreUnmatchedProperties()
                .Build();

            GetDeserializeConfig();
        }

       private void GetDeserializeConfig()
       {
            if (!CheckConfigurationExsist()) throw new Exception("The configuration file was not found");

            try
            {
                var yamlFile = File.ReadAllText(CONFIG_PATH);
                _configurationFile = _deserializer.Deserialize<HealthCheckConfiguration>(yamlFile);
                if (_configurationFile is null) throw new Exception("Empty file");
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
