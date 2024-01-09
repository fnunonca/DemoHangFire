using System;
using System.IO;

namespace LogSearcher
{
    class Program
    {
        static void Main(string[] args)
        {
            string logDirectory = @"D:\Log\Interbank_NOV\";
            string[] orderNumbers =
            {
            "IBKd31162d4OQfP",
            "IBK74a75521nAuR",
            "IBK369dda40xBqt",
            "IBKd3a486765zzj",
            "IBK0bedb464ax2X",
            "IBK632d944aj2Kw",
            "IBK858cd81eDFM6",
            "IBK685c4c12bHqy",
            "IBK9eef12e6pOyW",
            "IBKe53196d8aNaV",
            "IBK15bb7274Y2x2",
            "IBKca37caacZmzn",
            "IBK3e3b8f9fhRsf",
            "IBK4a41972ag4DZ",
            "IBK7faf4ca4FDit",
            "IBK26c83172vKCG",
            "IBKad1108b2Fymg",
            "IBKff3b5eacBN3v",
            "IBK1cc19e52SItm",
            "IBKa2051a012Hvc",
            "IBKd26356baWMoJ",
            "IBKc94b4c3b6c7U",
            "IBK58e9785aSlzG",
            "IBK2aa0bac6cDns",
            "IBKe9d1dfd5BI60",
            "IBK644b9890FXc8",
            "IBK795b0d34lr3L",
            "IBK63011629v5bv",
            "IBK090e6a30RDtS",
            "IBKd7a17c5dFwsB",
            "IBK03b88f30IkUo",
            "IBK2fc0075cPRQv",
            "IBK24da9341lTCU",
            "IBK2225a216p2Kr",
            "IBK875952af0QDq",
            "IBK66374839m22Z",
            "IBK78841e6cu4FG",
            "IBKf0ec7726KoTn",
            "IBK3db12f36subi",
            "IBKcdedaaf5CmqY",
            "IBK2c404a53hf8q",
            "IBKe1392e59IGOM",
            "IBKfdaa2370LEV1",
            "IBKcd11fc0a2yGe",
            "IBK7d57603bC5Wu",
            "IBK32d2d852QWFE",
            "IBK11811fafnhTA",
            "IBK879cf364A7vY",
            "IBKdb10d3a2bae0",
            "IBKe708f02ad1ty",
            "IBKfdd9d400IBlV"
            };

            string outputFile = @"D:\Log\Interbank_NOV_response\response.log";

            try
            {
                Console.WriteLine("Starting to process log files...");

                var files = Directory.EnumerateFiles(logDirectory, "*.log", SearchOption.AllDirectories);
                if (!files.Any())
                {
                    Console.WriteLine("No log files found in the directory or its subdirectories.");
                    return;
                }

                using (StreamWriter writer = new StreamWriter(outputFile))
                {
                    foreach (string file in files)
                    {
                        Console.WriteLine($"Processing file: {file}");
                        var logContent = File.ReadAllLines(file);
                        foreach (var line in logContent)
                        {
                            foreach (var orderNumber in orderNumbers)
                            {
                                if (line.Contains(orderNumber) && line.Contains("|Payment|End|response:"))
                                {
                                    writer.WriteLine(line);
                                    Console.WriteLine($"Found and wrote line for order {orderNumber}");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) // Capturando excepciones más generales
            {
                Console.WriteLine($"An exception occurred: {ex.Message}");
            }

            Console.WriteLine("Search and write process complete.");
        }
    }
}
