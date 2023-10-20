using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace LogAnalyzer
{
    class Program
    {
        private const string RootDirectory = @"D:\Log\TARJETAW";
        private const string SearchPattern = "nlog-apicontrollerpayments-*.log";
        private const string LogPattern = @"\|Payment\|Notification\| Status Code Response 404";
        private const string LogEndPattern = @"\|Payment\|End\|response:";
        private const string OutputFile = @"D:\OutputLog.txt"; // ruta donde se guardará el archivo

        static void Main(string[] args)
        {
            var logFiles = GetLogFiles(RootDirectory, SearchPattern);

            var allResponses = new List<string>();

            foreach (var logFile in logFiles)
            {
                var responses = ExtractResponseLines(logFile, LogEndPattern);
                allResponses.AddRange(responses);
            }

            File.WriteAllLines(OutputFile, allResponses);

            Console.WriteLine($"Finished processing. Responses written to {OutputFile}. Press any key to exit.");
            Console.ReadKey();
        }

        private static List<string> GetLogFiles(string rootDirectory, string searchPattern)
        {
            return Directory.GetFiles(rootDirectory, searchPattern, SearchOption.AllDirectories).ToList();
        }

        private static IEnumerable<string> ExtractResponseLines(string filePath, string endPattern)
        {
            var responseLines = new List<string>();
            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                if (Regex.IsMatch(line, endPattern))
                {
                    responseLines.Add(line);
                }
            }

            return responseLines;
        }
    }
}
