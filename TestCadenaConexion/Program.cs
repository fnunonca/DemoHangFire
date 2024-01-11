
using Microsoft.Data.SqlClient;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = "Server=MC0780;Database=PUNTOWEB;Uid=PUNTOUSER;TrustServerCertificate=True;Pwd=!908@N61@D0#;Min Pool Size=10;Max Pool Size=100;";

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Conexión exitosa a la base de datos.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al conectar a la base de datos: " + ex.Message);
        }

        Console.WriteLine("Presione cualquier tecla para salir.");
        Console.ReadKey();
    }
}