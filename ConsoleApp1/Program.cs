namespace ConsoleApp1
{
    internal class Program
    {
        public static void Main()
        {
            long unixTimeMilliseconds = 1695150633915;
            DateTimeOffset dateTime = DateTimeOffset.FromUnixTimeMilliseconds(unixTimeMilliseconds);
            Console.WriteLine(dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            Console.ReadKey();
        }
    }
}