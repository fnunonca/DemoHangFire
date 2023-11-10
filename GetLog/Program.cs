using Renci.SshNet;

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

            // Lista de rutas de archivos remotos
            //string[] remoteFilePaths =
            //{
            //    "svprdmw365_websiteapicontroller_444/authorization/nlog-ApiControllerAuthorization-all-2023-11-09.log",
            //    "svprdmw365_websiteapicontroller_443/authorization/nlog-ApiControllerAuthorization-all-2023-11-09.log",
            //    "svprdlw366_websiteapicontroller_444/authorization/nlog-ApiControllerAuthorization-all-2023-11-09.log",
            //    "svprdlw366_websiteapicontroller_443/authorization/nlog-ApiControllerAuthorization-all-2023-11-09.log"
            //};
            //string[] remoteFilePaths =
            //{
            //    "SVPRDLW364_WebSiteApi_443/paymentlink/APIPublicaLink444-all-2023-10-14.log",
            //    "SVPRDLW364_WebSiteApi_444/paymentlink/APIPublicaLink444-all-2023-10-14.log",
            //    "SRVPRDLW363_WebSiteApi_444/paymentlink/APIPublicaLink444-all-2023-10-14.log",
            //    "SRVPRDLW363_WebSiteApi_443/paymentlink/APIPublicaLink444-all-2023-10-14.log"
            //};
            //string[] remoteFilePaths =
            //{
            //    "svprdmw365_websiteapicontroller_444/paymentlink/APIControllerLink444-all-2023-10-30.log",
            //    "svprdmw365_websiteapicontroller_443/paymentlink/APIControllerLink444-all-2023-10-30.log",
            //    "svprdlw366_websiteapicontroller_444/paymentlink/APIControllerLink444-all-2023-10-30.log",
            //    "svprdlw366_websiteapicontroller_443/paymentlink/APIControllerLink444-all-2023-10-30.log"
            //};
            //string[] remoteFilePaths =
            //{
            //    "svprdmw365_websiteapicontroller_444/qr/nlog-Controller-all-2023-10-15.log",
            //    "svprdmw365_websiteapicontroller_443/qr/nlog-Controller-all-2023-10-15.log",
            //    "svprdlw366_websiteapicontroller_444/qr/nlog-Controller-all-2023-10-15.log",
            //    "svprdlw366_websiteapicontroller_443/qr/nlog-Controller-all-2023-10-15.log"
            //};
            //string[] remoteFilePaths =
            //{
            //    "svprdmw365_websiteapicontroller_444/payments/nlog-apicontrollerpayments-2023-11-08.log",
            //    "svprdmw365_websiteapicontroller_443/payments/nlog-apicontrollerpayments-2023-11-08.log",
            //    "svprdlw366_websiteapicontroller_444/payments/nlog-apicontrollerpayments-2023-11-08.log",
            //    "svprdlw366_websiteapicontroller_443/payments/nlog-apicontrollerpayments-2023-11-08.log"
            //};
            //string[] remoteFilePaths =
            //{
            //    "SVPRDLW363_WebSiteForm_444/api/nlog-FormApiPublic-2023-11-06.log",
            //    "SVPRDLW363_WebSiteForm_443/api/nlog-FormApiPublic-2023-11-06.log",
            //    "SVPRDLW364_WebSiteForm_444/api/nlog-FormApiPublic-2023-11-06.log",
            //    "SVPRDLW364_WebSiteForm_443/api/nlog-FormApiPublic-2023-11-06.log"
            //};

            string[] remoteFilePaths =
            {
                "svprdmw365_websiteapicontroller_444/qr/nlog-Controller-all-2023-11-09.log",
                "svprdmw365_websiteapicontroller_443/qr/nlog-Controller-all-2023-11-09.log",
                "svprdlw366_websiteapicontroller_444/qr/nlog-Controller-all-2023-11-09.log",
                "svprdlw366_websiteapicontroller_443/qr/nlog-Controller-all-2023-11-09.log"
            };
            //string[] remoteFilePaths =
            //{
            //    "SVRPRDLW366_WebSiteApiInternal_444/entity.ecommerce/nlog-entity-ecommerce-2023-10-18.log",
            //    "SVRPRDLW366_WebSiteApiInternal_443/entity.ecommerce/nlog-entity-ecommerce-2023-10-18.log",
            //    "SVPRDLW365_WebSiteApiInternal_444/entity.ecommerce/nlog-entity-ecommerce-2023-10-18.log",
            //    "SVPRDLW365_WebSiteApiInternal_443/entity.ecommerce/nlog-entity-ecommerce-2023-10-18.log"
            //};
            //string[] remoteFilePaths =
            //{
            //    "svprdmw365_websiteapicontroller_444/security/nlog-Security-2023-10-18.log",
            //    "svprdmw365_websiteapicontroller_443/security/nlog-Security-2023-10-18.log",
            //    "svprdlw366_websiteapicontroller_444/security/nlog-Security-2023-10-18.log",
            //    "svprdlw366_websiteapicontroller_443/security/nlog-Security-2023-10-18.log"
            //};
            //string[] remoteFilePaths =
            //{
            //    "SVRPRDLW366_WebSiteApiInternal_444/scoring.cybersource/nlog-ApiInternalScoringCybersource-2023-10-06.log",
            //    "SVRPRDLW366_WebSiteApiInternal_443/scoring.cybersource/nlog-ApiInternalScoringCybersource-2023-10-06.log",
            //    "SVPRDLW365_WebSiteApiInternal_444/scoring.cybersource/nlog-ApiInternalScoringCybersource-2023-10-06.log",
            //    "SVPRDLW365_WebSiteApiInternal_444/scoring.cybersource/nlog-ApiInternalScoringCybersource-2023-10-06.log"
            //};

            //string[] remoteFilePaths =
            //{
            //    "SVPRDLW368_WebSiteInterno_444/authorization/nlog-ApiInternoAuthorization-all-2023-10-13.log",
            //    "SVPRDLW368_WebSiteInterno_443/authorization/nlog-ApiInternoAuthorization-all-2023-10-13.log",
            //    "SVPRDMW367_WebSiteInterno_444/authorization/nlog-ApiInternoAuthorization-all-2023-10-13.log",
            //    "SVPRDMW367_WebSiteInterno_443/authorization/nlog-ApiInternoAuthorization-all-2023-10-13.log"
            //};
            //string[] remoteFilePaths =
            //{
            //    "SVPRDLW364_WebSiteApi_444/event/nlog-ApiPublicEvent-2023-10-15.log",
            //    "SVPRDLW364_WebSiteApi_443/event/nlog-ApiPublicEvent-2023-10-15.log",
            //    "SRVPRDLW363_WebSiteApi_444/event/nlog-ApiPublicEvent-2023-10-15.log",
            //    "SRVPRDLW363_WebSiteApi_443/event/nlog-ApiPublicEvent-2023-10-15.log"
            //};
            //string[] remoteFilePaths =
            //{
            //    "SVPRDLW364_WebSiteForm_444/api/nlog-FormApiPublic-2023-10-18.log",
            //    "SVPRDLW364_WebSiteForm_443/api/nlog-FormApiPublic-2023-10-18.log",
            //    "SRVPRDLW363_WebSiteForm_444/api/nlog-FormApiPublic-2023-10-18.log",
            //    "SRVPRDLW363_WebSiteForm_443/api/nlog-FormApiPublic-2023-10-18.log"
            //};
            //string[] remoteFilePaths =
            //{
            //    "SVPRDLW364_WebSiteApi_444/notification.dispatcher1/nlog-ApiNotication-Dispatcher1-PUBLIC-controller-2023-11-08.log",
            //    "SVPRDLW364_WebSiteApi_443/notification.dispatcher1/nlog-ApiNotication-Dispatcher1-PUBLIC-controller-2023-11-08.log",
            //    "SRVPRDLW363_WebSiteApi_444/notification.dispatcher1/nlog-ApiNotication-Dispatcher1-PUBLIC-controller-2023-11-08.log",
            //    "SRVPRDLW363_WebSiteApi_443/notification.dispatcher1/nlog-ApiNotication-Dispatcher1-PUBLIC-controller-2023-11-08.log"
            //};
            //string[] remoteFilePaths =
            //{
            //    "SVPRDLW364_WebSiteApi_444/notification.dispatcher1/nlog-ApiNotication-Dispatcher1-PUBLIC-controller-2023-10-15.log",
            //    "SVPRDLW364_WebSiteApi_443/notification.dispatcher1/nlog-ApiNotication-Dispatcher1-PUBLIC-controller-2023-10-15.log",
            //    "SRVPRDLW363_WebSiteApi_444/notification.dispatcher1/nlog-ApiNotication-Dispatcher1-PUBLIC-controller-2023-10-15.log",
            //    "SRVPRDLW363_WebSiteApi_443/notification.dispatcher1/nlog-ApiNotication-Dispatcher1-PUBLIC-controller-2023-10-15.log"
            //};
            //string[] remoteFilePaths =
            //{
            //    "SVPRDLW364_WebSiteApi_444/qrnotification/nlog-ApiQRNotification-all-2023-10-15.log",
            //    "SVPRDLW364_WebSiteApi_443/qrnotification/nlog-ApiQRNotification-all-2023-10-15.log",
            //    "SRVPRDLW363_WebSiteApi_444/qrnotification/nlog-ApiQRNotification-all-2023-10-15.log",
            //    "SRVPRDLW363_WebSiteApi_443/qrnotification/nlog-ApiQRNotification-all-2023-10-15.log"
            //};
            //string[] remoteFilePaths =
            //{
            //    "SVPRDMW365_WebSiteApiController_444/cancel/nlog-ApiControllerCancel-all-2023-10-23.log",
            //    "SVPRDMW365_WebSiteApiController_443/cancel/nlog-ApiControllerCancel-all-2023-10-23.log",
            //    "SVPRDLW366_WebSiteApiController_444/cancel/nlog-ApiControllerCancel-all-2023-10-23.log",
            //    "SVPRDLW366_WebSiteApiController_443/cancel/nlog-ApiControllerCancel-all-2023-10-23.log"
            //};
            //string[] remoteFilePaths =
            //{
            //    "SVPRDMW365_WebSiteApiController_444/mile/nlog-Service.ApiMileController-all-2023-10-13.log",
            //    "SVPRDMW365_WebSiteApiController_443/mile/nlog-Service.ApiMileController-all-2023-10-13.log",
            //    "SVPRDLW366_WebSiteApiController_444/mile/nlog-Service.ApiMileController-all-2023-10-13.log",
            //    "SVPRDLW366_WebSiteApiController_443/mile/nlog-Service.ApiMileController-all-2023-10-13.log"
            //};
            //string[] remoteFilePaths =
            //{
            //    "SVPRDMW367_WebSiteInterno_444/cybprocessor/nlog-ApiInternalCybprocessor-all-2023-10-06.log",
            //    "SVPRDMW367_WebSiteInterno_443/cybprocessor/nlog-ApiInternalCybprocessor-all-2023-10-06.log",
            //    "SVPRDLW368_WebSiteInterno_444/cybprocessor/nlog-ApiInternalCybprocessor-all-2023-10-06.log",
            //    "SVPRDLW368_WebSiteInterno_443/cybprocessor/nlog-ApiInternalCybprocessor-all-2023-10-06.log"
            //};
            //string[] remoteFilePaths =
            //{
            //    "SVPRDMW365_WebSiteApiBusiness_444/cancel/nlog-ApiBusinessCancel-all-2023-09-25.log",
            //    "SVPRDMW365_WebSiteApiBusiness_443/cancel/nlog-ApiBusinessCancel-all-2023-09-25.log",
            //    "SVPRDLW366_WebSiteApiBusiness_444/cancel/nlog-ApiBusinessCancel-all-2023-09-25.log",
            //    "SVPRDLW366_WebSiteApiBusiness_443/cancel/nlog-ApiBusinessCancel-all-2023-09-25.log",
            //};

            //string[] remoteFilePaths =
            //{
            //    "SVPRDLW368_WebSiteInterno_444/cancel/nlog-ApiInternoCancel-all-2023-09-25.log",
            //    "SVPRDLW368_WebSiteInterno_443/cancel/nlog-ApiInternoCancel-all-2023-09-25.log",
            //    "SVPRDMW367_WebSiteInterno_444/cancel/nlog-ApiInternoCancel-all-2023-09-25.log",
            //    "SVPRDMW367_WebSiteInterno_443/cancel/nlog-ApiInternoCancel-all-2023-09-25.log",
            //};

            //string[] remoteFilePaths =
            //{
            //    "SVPRDMW365_WebSiteApiController_444/refund/nlog-ApiRefundController-all-2023-09-27.log",
            //    "SVPRDMW365_WebSiteApiController_443/refund/nlog-ApiRefundController-all-2023-09-27.log",
            //    "SVPRDLW366_WebSiteApiController_444/refund/nlog-ApiRefundController-all-2023-09-27.log",
            //    "SVPRDLW366_WebSiteApiController_443/refund/nlog-ApiRefundController-all-2023-09-27.log"
            //};
            string localBaseDirectory = @"D:\Log\Jueves\";

            using (var sftpClient = new SftpClient(host, port, username, password))
            {
                sftpClient.Connect();

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

                sftpClient.Disconnect();
            }
        }
    }
}
