using Renci.SshNet;
using System;
using System.IO;

namespace GetLog
{
    internal class Program
    {
        static void Main()
        {
            string host = "MC0305";
            int port = 22;
            string username = "MC1767";
            string password = "Cusc02024.";

            string localBaseDirectory = @"D:\Log\Interbank_NOV\";

            DateTime startDate = new DateTime(2023, 10, 30);
            DateTime endDate = new DateTime(2023, 11, 9);

            using (var sftpClient = new SftpClient(host, port, username, password))
            {
                sftpClient.Connect();

                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    string dateString = date.ToString("yyyy-MM-dd");
                    string[] remoteFilePaths =
                    {
                        $"svprdmw365_websiteapicontroller_444/payments/nlog-apicontrollerpayments-{dateString}.log",
                        $"svprdmw365_websiteapicontroller_443/payments/nlog-apicontrollerpayments-{dateString}.log",
                        $"svprdlw366_websiteapicontroller_444/payments/nlog-apicontrollerpayments-{dateString}.log",
                        $"svprdlw366_websiteapicontroller_443/payments/nlog-apicontrollerpayments-{dateString}.log"
                    };

                    foreach (var remoteFilePath in remoteFilePaths)
                    {
                        string localDirectory = Path.Combine(localBaseDirectory, Path.GetDirectoryName(remoteFilePath));
                        string localFilePath = Path.Combine(localDirectory, Path.GetFileName(remoteFilePath));

                        // Crear el directorio local si no existe
                        Directory.CreateDirectory(localDirectory);

                        if (sftpClient.Exists(remoteFilePath))
                        {
                            using (var fileStream = new FileStream(localFilePath, FileMode.Create))
                            {
                                sftpClient.DownloadFile(remoteFilePath, fileStream);
                                Console.WriteLine($"Archivo descargado exitosamente en {localFilePath}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"El archivo remoto no existe: {remoteFilePath}");
                        }
                    }
                }

                sftpClient.Disconnect();
            }
        }
    }
}
