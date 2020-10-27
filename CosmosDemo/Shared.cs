using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace CosmosDemo
{
    public static class Shared
    {
        public static CosmosClient Client { get; private set; }
        public static string DbName { get; set; } = "Facebook2.0";
        public static string ContainerName { get; set; } = "Users";

        static Shared()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var endpoint = configuration.GetConnectionString("CosmosEndpoint");
            var masterKey = configuration.GetConnectionString("CosmosPrimaryKey");

            var client = new CosmosClient(endpoint, masterKey);
            Client = client;
        }
    }
}
