using System.Data.SqlClient;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;

public class CertificateWorker
{
    private readonly ILogger<CertificateWorker> _logger;
    private CancellationTokenSource _cancellationTokenSource;


    public CertificateWorker()
    {
        // Si utilizas algún DI, deberás adaptar esta parte.
        _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<CertificateWorker>();
    }

    public void Start()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        ExecuteAsync(_cancellationTokenSource.Token).ConfigureAwait(false);
    }

    public void Stop()
    {
        _cancellationTokenSource.Cancel();
    }

    protected async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            try
            {
                var certificate = await LoadCertificateFromDatabase();

                if (certificate == null)
                {
                    _logger.LogError("No se pudo cargar el certificado.");
                    continue;
                }

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;

                var handler = new HttpClientHandler();
                handler.ClientCertificates.Add(certificate);

                string jsonParameters = "asdasd";
                var _content = new StringContent(jsonParameters, Encoding.UTF8, "application/json");


                using (var httpClient = new HttpClient(handler))
                {
                    var response = await httpClient
                          .PostAsync("https://ye.cert.yape.com.pe/yap-tok/security/v1/token",
                          _content);
                    var content = await response.Content.ReadAsStringAsync();

                    File.WriteAllText("respuesta.txt", content);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrió un error: {ex.Message}");
            }

            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken); // Aquí determinas cada cuánto tiempo quieres que se realice el proceso.
        }
    }

    private async Task<X509Certificate2> LoadCertificateFromDatabase()
    {
        string connectionString = "Server=MC0780;Database=PUNTOWEB;Uid=PUNTOUSER; Pwd=!908@N61@D0#;Min Pool Size=10; Max Pool Size=100;";
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
        catch (Exception ex)
        {
            _logger.LogError($"Error al cargar el certificado desde la base de datos: {ex.Message}");
            return null;
        }
    }
}
