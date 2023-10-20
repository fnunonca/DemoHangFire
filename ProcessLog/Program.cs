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

        static void Main(string[] args)
        {
            var logFiles = GetLogFiles(RootDirectory, SearchPattern);

            foreach (var logFile in logFiles)
            {
                var transactionIds = ExtractTransactionIds(logFile, LogPattern);
                foreach (var id in transactionIds)
                {
                    Console.WriteLine($"Transaction ID from {logFile}: {id}");
                }
            }

            Console.WriteLine("Finished processing. Press any key to exit.");
            Console.ReadKey();
        }

        private static List<string> GetLogFiles(string rootDirectory, string searchPattern)
        {
            return Directory.GetFiles(rootDirectory, searchPattern, SearchOption.AllDirectories).ToList();
        }

        private static IEnumerable<string> ExtractTransactionIds(string filePath, string pattern)
        {
            var transactionIds = new List<string>();
            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                if (Regex.IsMatch(line, pattern))
                {
                    var parts = line.Split('|');
                    if (parts.Length >= 4)
                    {
                        transactionIds.Add(parts[3]);
                    }
                }
            }

            return transactionIds;
        }
    }
}
