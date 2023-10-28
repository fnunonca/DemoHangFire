using System.Data.SqlClient;
using System.Net;
using System.Security.Cryptography.X509Certificates;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Seleccione una opción:");
        Console.WriteLine("1. Cargar certificado desde archivo.");
        Console.WriteLine("2. Cargar certificado desde base de datos.");
        var option = Console.ReadLine();

        X509Certificate2 certificate = null;

        try
        {
            switch (option)
            {
                case "1":
                    var certFilePath = @"D:\tmp\izipay_keyAndCertBundle.p12";
                    var certPassword = "123456";
                    certificate = new X509Certificate2(certFilePath, certPassword);
                    break;

                case "2":
                    certificate = await LoadCertificateFromDatabase();
                    break;

                default:
                    Console.WriteLine("Opción inválida.");
                    return;
            }

            if (certificate == null)
            {
                Console.WriteLine("No se pudo cargar el certificado.");
                return;
            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;

            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(certificate);

            using (var httpClient = new HttpClient(handler))
            {
                var response = await httpClient.GetAsync("https://ye.cert.yape.com.pe/yap-tok/security/v1/token");
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

    static async Task<X509Certificate2> LoadCertificateFromDatabase()
    {
        string connectionString = "Server=MC0780;Database=PUNTOWEB;Uid=PUNTOUSER; Pwd=!908@N61@D0#;Min Pool Size=10; Max Pool Size=100;"; // Debes reemplazar con la cadena de conexión a tu base de datos.
        string sqlQuery = "select CERTIFICATE_CONTENT from [key].MerchantCertificate where id = 14";

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    var certificateContent = (string)await command.ExecuteScalarAsync();
                    byte[] certificateBytes = Convert.FromBase64String(certificateContent);
                    return new X509Certificate2(certificateBytes, "izipay123456");
                }
            }
        }
        catch
        {
            Console.WriteLine("Error al cargar el certificado desde la base de datos.");
            return null;
        }
    }
}
