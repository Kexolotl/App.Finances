using System;
using System.IO;
using Newtonsoft.Json;

namespace MyFinances
{
    public static class ServiceConfigurationReader
    {
        public static T GetServiceConfiguration<T>(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new Exception("Service configuration file does not exist. Cannot run application.");
            }

            var content = File.ReadAllText(filename);
            var serviceConfiguration = JsonConvert.DeserializeObject<T>(content);

            return serviceConfiguration;
        }
    }

    public class MyFinancesServiceConfiguration
    {
        public string BaseDirectory { get; set; }
        public string StorageFolder { get; set; }
        public string Logging { get; set; }
        public string ServiceName { get; set; }
    }
}
