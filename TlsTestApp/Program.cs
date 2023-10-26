using System.Net;
using System.Security.Cryptography.X509Certificates;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;

            var certFilePath = @"D:\tmp\izipay_keyAndCertBundle.p12";
            var certPassword = "123456";
            var certificate = new X509Certificate2(certFilePath, certPassword);

            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(certificate);

            using (var httpClient = new HttpClient(handler))
            {
                var response = await httpClient.GetAsync("https://ye.cert.yape.com.pe/yap-tok/security/v1/token"); // Reemplaza con tu URL
                var content = await response.Content.ReadAsStringAsync();

                Console.WriteLine("Respuesta recibida:");
                Console.WriteLine(content);
                Console.ReadKey();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ocurrió un error:");
            Console.WriteLine(ex.Message);
        }
    }
}
