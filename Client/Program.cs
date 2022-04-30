using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static async Task  Main(string[] args)
        {
            //descobrir os endpoints através dos metadados
            var client = new HttpClient();
            var discovery = await client.GetDiscoveryDocumentAsync("https://localhost:5001");

            if (discovery.IsError)
            {
                Console.WriteLine(discovery.Error);
                return;
            }

            //request token
            var tokenResponse = await client.RequestClientCredentialsTokenAsync
                                (
                                    new ClientCredentialsTokenRequest
                                    {
                                        Address = discovery.TokenEndpoint,
                                        ClientId = "client",
                                        ClientSecret = "secret",
                                        Scope = "api1"
                                    }
                                );

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
            }

            Console.WriteLine(tokenResponse.Json);

            Console.ReadLine();
        }
    }
}
