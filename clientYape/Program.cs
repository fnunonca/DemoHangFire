using System;
using System.Threading.Tasks;
using clientYape;
using Grpc.Net.Client;

namespace ClientYape
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Crear el canal gRPC
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");

            // Crear el cliente gRPC
            var client = new Yape.YapeClient(channel);

            // Crear la solicitud
            var request = new YapeBodyRequest
            {
                MerchantCode = "somecode",
                OrderNumber = "someorder",
                Language = "somelanguage"
            };

            // Enviar la solicitud y obtener la respuesta
            var response = await client.SendRequestAsync(request);

            // Imprimir la respuesta
            Console.WriteLine($"Code: {response.Code}, Message: {response.Message}");
        }
    }
}
