
using Catalog.API.Entities;
using Elasticsearch.Net;
using Nest;

namespace Catalog.API.Extensions
{
    public static class ElasticSearchExtensions
    {
        public static void AddElasticSearch (
            this IServiceCollection services, IConfiguration configuration
            )
        {
            var url = configuration["ELKConfiguration:Uri"];
            var Index = configuration["ELKConfiguration: index"];

            var settings = new ConnectionSettings(new Uri(url)).PrettyJson()
                                                               .DefaultIndex("Products");

            AddDefaultMappings(settings);
            var client = new ElasticClient(settings);
            services.AddSingleton<IElasticClient>(client);

            CreateIndex(client, "Products");
        }
        private static void AddDefaultMappings (ConnectionSettings settings)
        {
            settings.DefaultMappingFor<Product>(p =>
            p.Ignore(x => x.Price)
            .Ignore(x => x.Summary)
            .Ignore(x => x.ImageFile)
            .Ignore(x => x.ItemsInStock));
        }

        private static void CreateIndex(IElasticClient client, string IndexName)
        {
            client.Indices.Create(IndexName, i => i.Map<Product>(x => x.AutoMap()));
        }
    }
}
