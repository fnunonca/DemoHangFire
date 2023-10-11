using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SignatureApp
{
    internal class Program
    {
        public class Transaction
        {
            public string DateTimeTransaction { get; set; }
            public double Amount { get; set; }
            public string Currency { get; set; }
            public string CodeAuth { get; set; }
        }

        public class Merchant
        {
            public string MerchantCode { get; set; }
            public string PhoneNumber { get; set; }
            public string Address { get; set; }
            public string Email { get; set; }
            public string Name { get; set; }
        }

        public class JsonData
        {
            public string OrderNumber { get; set; }
            public string OrderStatus { get; set; }
            public string EndPointTransaction { get; set; }
            public Transaction Transaction { get; set; }
            public Merchant Merchant { get; set; }
        }

        static async Task Main()
        {
            // Instanciar el objeto con los valores proporcionados
            var myObject = new JsonData
            {
                OrderNumber = "11805",
                OrderStatus = "Pago-Aceptado",
                EndPointTransaction = "https://www.norton.pe/notificacion/notificacion-izp-v2.php?send_email=1",
                Transaction = new Transaction
                {
                    DateTimeTransaction = "20230928",
                    Amount = 1,
                    Currency = "PEN",
                    CodeAuth = "424672"
                },
                Merchant = new Merchant
                {
                    MerchantCode = "4078037",
                    PhoneNumber = "936580859",
                    Address = "AV CAMINO REAL 1278",
                    Email = "",
                    Name = "NORTON"
                }
            };

            // Convertir el objeto a una cadena JSON
            string jsonToSign = JsonConvert.SerializeObject(myObject);

            Console.WriteLine("JSON a firmar:");
            Console.WriteLine(jsonToSign);

            string secretKey = "JzWCKMEKILfcbAMBUO4rD8R5sgM9Mii4";

            string signature = await Sign(jsonToSign, secretKey);

            if (!string.IsNullOrEmpty(signature))
            {
                Console.WriteLine($"Firma generada: {signature}");
            }
            else
            {
                Console.WriteLine("Error al generar la firma.");
            }

            Console.ReadLine(); // Mantén la ventana abierta
        }

        public static async Task<string> Sign(string inputSign, string secretKey)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                byte[] requestBytes = Encoding.UTF8.GetBytes(inputSign);
                byte[] hash = hmac.ComputeHash(requestBytes);
                return await Task.Run(() => Convert.ToBase64String(hash));
            }
        }
    }
}
