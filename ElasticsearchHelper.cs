using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkOperationDemo
{
    public static class ElasticsearchHelper
    {
        public static ElasticClient GetESClient()
        {
            ConnectionSettings connectionSettings;
            ElasticClient elasticClient;
            StaticConnectionPool connectionPool;
            var nodes = new Uri[] {
                new Uri("http://da6591de6c48.ngrok.io") //Provide ES cluster addresses)
            };
            connectionPool = new StaticConnectionPool(nodes);
            connectionSettings = new ConnectionSettings(connectionPool);
            elasticClient = new ElasticClient(connectionSettings);

            return elasticClient;
        }

        public static async Task CreateBulkDocument(ElasticClient elasticClient, string indexName, List<Product> products)
        {
            var bulkResponse = await elasticClient.BulkAsync(b => b
                                       .Index(indexName)
                                       .IndexMany(products));


            if (bulkResponse.ApiCall.Success && bulkResponse.IsValid)
            {
                // success fully inserted..
                Console.WriteLine("Bulk Document Inserted.");
            }
            else
            {
                Console.WriteLine(bulkResponse.OriginalException.ToString());
            }
        }

    }
}
