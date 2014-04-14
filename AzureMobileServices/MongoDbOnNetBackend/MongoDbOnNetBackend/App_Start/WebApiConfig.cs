using Microsoft.WindowsAzure.Mobile.Service;

namespace MongoDbOnNetBackend
{
    public static class WebApiConfig
    {
        public static void Register()
        {
            var config = ServiceConfig.Initialize(new ConfigBuilder());
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
        }
    }
}