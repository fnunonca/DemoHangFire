using Renci.SshNet;
using System;
using System.Collections.Generic;
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
            string password = "L1m@2023.";

            string[] baseRemoteFilePaths =
            {
                "SVPRDMW365_WebSiteApiController_444/notification.dispatcher1/nlog-ApiNotication-dispatcherUno-controller-yyyy-MM-dd.log",
                "SVPRDMW365_WebSiteApiController_443/notification.dispatcher1/nlog-ApiNotication-dispatcherUno-controller-yyyy-MM-dd.log",
                "SVPRDLW366_WebSiteApiController_444/notification.dispatcher1/nlog-ApiNotication-dispatcherUno-controller-yyyy-MM-dd.log",
                "SVPRDLW366_WebSiteApiController_443/notification.dispatcher1/nlog-ApiNotication-dispatcherUno-controller-yyyy-MM-dd.log"
            };

            List<string> dates = GenerateDates(DateTime.Now.AddDays(-14), DateTime.Now);

            foreach (var basePath in baseRemoteFilePaths)
            {
                var pathsForBase = GeneratePaths(basePath, dates);

                foreach (var path in pathsForBase)
                {
                    DownloadFromSFTP(host, port, username, password, path);
                }
            }
        }

        static List<string> GenerateDates(DateTime start, DateTime end)
        {
            var dates = new List<string>();

            for (DateTime date = start; date <= end; date = date.AddDays(1))
            {
                dates.Add(date.ToString("yyyy-MM-dd"));
            }

            return dates;
        }

        static List<string> GeneratePaths(string basePath, List<string> dates)
        {
            var paths = new List<string>();

            foreach (var date in dates)
            {
                paths.Add(basePath.Replace("yyyy-MM-dd", date));
            }

            return paths;
        }

        static void DownloadFromSFTP(string host, int port, string username, string password, string remoteFilePath)
        {
            string localBaseDirectory = @"D:\Log\prd\";

            using (var sftpClient = new SftpClient(host, port, username, password))
            {
                sftpClient.Connect();

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

                sftpClient.Disconnect();
            }
        }
    }
}
