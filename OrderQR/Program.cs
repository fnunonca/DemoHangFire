using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        string sourceFilePath = @"D:\Log\TARJETAW\reporteTarjeta.log";
        string destinationDirectory = @"D:\Log\TARJETAW\";
        string pattern = "\"merchantCode\":\"(\\d+)\"";

        if (!File.Exists(sourceFilePath))
        {
            Console.WriteLine("El archivo de origen no existe.");
            return;
        }

        // Leer las líneas del archivo de origen
        var lines = File.ReadAllLines(sourceFilePath);

        // Diccionario para agrupar las líneas por merchantCode
        var groupedLines = new Dictionary<string, List<string>>();

        foreach (var line in lines)
        {
            // Ignorar registros que contienen {"code":"RA2"
            if (line.Contains("{\"code\":\"RA2\"")) continue;

            // Buscar merchantCode en la línea actual
            Match match = Regex.Match(line, pattern);
            if (match.Success)
            {
                string merchantCode = match.Groups[1].Value;

                // Agregar línea al diccionario agrupado por merchantCode
                if (!groupedLines.ContainsKey(merchantCode))
                {
                    groupedLines[merchantCode] = new List<string>();
                }
                groupedLines[merchantCode].Add(line);
            }
        }

        // Escribir cada grupo en un archivo independiente
        foreach (var group in groupedLines)
        {
            string destinationFilePath = $"{destinationDirectory}CARD_{group.Key}.log";
            File.WriteAllLines(destinationFilePath, group.Value);
        }

        Console.WriteLine("Proceso finalizado.");
    }
}
