using System;
using System.Data.SqlClient;
using System.IO;

namespace LogToSql
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=MC0780;Database=PUNTOWEB;Uid=PUNTOUSER; Pwd=!908@N61@D0#;Min Pool Size=10; Max Pool Size=100;";
            string logFilePath = @"D:\Log\Interbank_NOV_response\response.log";

            try
            {
                var lines = File.ReadAllLines(logFilePath);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (var line in lines)
                    {
                        var columns = line.Split('|');
                        if (columns.Length >= 6)
                        {
                            var cmdText = "INSERT INTO Payment.FVN_TEMP (FECHA, ID_CERO, LEVEL_LOG, API, STEP, RESPONSE) VALUES (@fecha, @idCero, @levelLog, @api, @step, @response)";

                            using (SqlCommand command = new SqlCommand(cmdText, connection))
                            {
                                command.Parameters.AddWithValue("@fecha", columns[0]);
                                command.Parameters.AddWithValue("@idCero", columns[1]);
                                command.Parameters.AddWithValue("@levelLog", columns[2]);
                                command.Parameters.AddWithValue("@api", columns[3]);
                                command.Parameters.AddWithValue("@step", columns[4]);
                                command.Parameters.AddWithValue("@response", columns.Length > 6 ? columns[5] + "|" + columns[6] : columns[5]);

                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }

                Console.WriteLine("Data inserted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
