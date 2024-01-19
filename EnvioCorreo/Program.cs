using System;
using System.Net.Mail;
using System.Threading.Tasks;  // Asegúrate de incluir esta línea para usar Task

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            Console.WriteLine("Ingrese el correo electrónico del destinatario:");
            string toEmail = Console.ReadLine();

            Console.WriteLine("Ingrese el asunto del correo:");
            string subject = Console.ReadLine();

            Console.WriteLine("Ingrese el cuerpo del correo:");
            string body = Console.ReadLine();

            await SendEmail(toEmail, subject, body);
            Console.WriteLine("Correo enviado con éxito.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al enviar el correo: " + ex.Message);
        }

        Console.WriteLine("Presione cualquier tecla para salir.");
        Console.ReadKey();
    }

    static async Task SendEmail(string toEmail, string subject, string body)
    {
        string fromEmail = "notificaciones@izipay.pe";

        MailMessage message = new MailMessage(fromEmail, toEmail, subject, body);

        using (var smtp = new SmtpClient("172.16.28.14"))
        {
            smtp.UseDefaultCredentials = false;
            await smtp.SendMailAsync(message);
        }
    }
}
