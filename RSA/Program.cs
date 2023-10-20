using System.Data.SqlClient;
namespace RSA
{
    class Program
    {
        static void Main()
        {
            string connectionString = "Server=MC0780;Database=PUNTOWEB;Uid=PUNTOUSER; Pwd=!908@N61@D0#;Min Pool Size=10; Max Pool Size=100;";
            string publicKey = GetPublicKey(connectionString);

            if (!string.IsNullOrEmpty(publicKey))
            {
                File.WriteAllText("publicKey.txt", publicKey);
                Console.WriteLine("Llave pública RSA exportada con éxito a publicKey.txt.");
            }
            else
            {
                Console.WriteLine("No se pudo obtener la llave pública.");
            }
        }

        static string GetPublicKey(string connectionString)
        {
            string publicKey = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT LlavePublica FROM MerchantPublicKey WHERE CuentaMerchant = '4500055'", connection))
                {
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        publicKey = result.ToString();
                    }
                }
            }

            return publicKey;
        }
    }
}

