using Topshelf;

namespace MyCertificateService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<CertificateWorker>(s =>
                {
                    s.ConstructUsing(name => new CertificateWorker());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("Servicio Windows Yape Test");
                x.SetDisplayName("Servicio Windows Yape Test");
                x.SetServiceName("Servicio Windows Yape Test");
            });
        }
    }


}