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

            var request = new PingRequest
            {
                Code = "00"
            };


            // Enviar la solicitud y obtener la respuesta
            var response = await client.PingAsync(request);

            // Imprimir la respuesta
            Console.WriteLine($"Code: {response.Code}, Message: {response.Message}");
            Console.ReadKey();
        }
    }
}
