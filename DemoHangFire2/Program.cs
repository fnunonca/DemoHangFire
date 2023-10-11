using Hangfire;
using Hangfire.MemoryStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Threading;
using Quartz;
using Quartz.Impl;
using System.Threading.Tasks;

namespace DemoHangFire2
{
    internal class Program
    {
        static List<string> numeros = Enumerable.Range(1, 100).Select(n => n.ToString()).ToList();
        static int currentIndex = 0;
        static int cpuMax = 10;
        static List<string> enqueuedNumbers = new List<string>();

        static void Main(string[] args)
        {
            ScheduleJobWithQuartz();

            Console.ReadLine();
        }
        //static void Main(string[] args)
        //{
        //    GlobalConfiguration.Configuration.UseMemoryStorage();

        //    using (var server = new BackgroundJobServer())
        //    {
        //        for (currentIndex = 0; currentIndex < numeros.Count; currentIndex++)
        //        {
        //            ProcessNumber(numeros[currentIndex]);
        //        }
        //    }

        //    // Mostrar números encolados
        //    if (enqueuedNumbers.Any())
        //    {
        //        Console.WriteLine($"Números encolados: {enqueuedNumbers.Count}");
        //        Console.WriteLine($"Lista de números encolados: {string.Join(", ", enqueuedNumbers)}");
        //    }
        //    else
        //    {
        //        Console.WriteLine("No se encoló ningún número.");
        //    }

        //    Console.ReadLine();
        //}
        private static async void ScheduleJobWithQuartz()
        {
            // Obtiene una instancia del programador
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = await schedulerFactory.GetScheduler();

            // Inicia el programador
            await scheduler.Start();

            // Define el trabajo y lo asocia a nuestro método ExecuteJob
            var job = JobBuilder.Create<JobRunner>()
                .WithIdentity("myJob", "group1")
                .Build();

            // Define el gatillo (trigger) para las 5 de la tarde todos los días
            var trigger = TriggerBuilder.Create()
                .WithIdentity("myTrigger", "group1")
                .WithDailyTimeIntervalSchedule(x => x
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(17, 0))
                    .OnEveryDay())
                .StartNow()
                .Build();

            // Programa el trabajo con el gatillo
            await scheduler.ScheduleJob(job, trigger);
        }

        public class JobRunner : IJob
        {
            public async Task Execute(IJobExecutionContext context)
            {
                // Aquí va el código que quieres que se ejecute diariamente a las 5 de la tarde
                // En este caso, tu código original para procesar los números y monitorear el uso de la CPU

                GlobalConfiguration.Configuration.UseMemoryStorage();

                using (var server = new BackgroundJobServer())
                {
                    for (currentIndex = 0; currentIndex < numeros.Count; currentIndex++)
                    {
                        ProcessNumber(numeros[currentIndex]);
                    }
                }

                // ... [resto de tu código]

            }
        }
        private static float GetCpuUsage()
        {
            var searcher = new ManagementObjectSearcher("select * from Win32_PerfFormattedData_PerfOS_Processor");
            var collection = searcher.Get();

            foreach (var obj in collection)
            {
                if (obj["Name"].ToString() == "_Total")
                {
                    return float.Parse(obj["PercentProcessorTime"].ToString());
                }
            }
            return 0;
        }
        public static void CheckCpuAndWait(string numero)
        {
            Thread.Sleep(TimeSpan.FromMinutes(1));

            float cpuUsage = GetCpuUsage();
            Console.WriteLine($"Uso del CPU tras esperar: {cpuUsage}%.");
            if (cpuUsage <= cpuMax)
            {
                // Reanuda el proceso
                Console.WriteLine($"{numero} reanudado");
                ContinueProcessing();
            }
            else
            {
                ConsoleColor originalColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Blue;
                // Si el CPU todavía está por encima del límite, encola el número nuevamente
                Console.WriteLine($"El uso de CPU sigue siendo mayor del {cpuMax}%, re-encolando el número {numero}...");
                Console.ForegroundColor = originalColor;
                BackgroundJob.Enqueue(() => CheckCpuAndWait(numero));
            }
        }

        public static void ContinueProcessing()
        {
            // Aquí, recuerda que debes llevar un registro del índice actual para continuar desde donde lo dejaste
            // Suponiendo que tienes una variable global llamada currentIndex
            for (int i = currentIndex; i < numeros.Count; i++)
            {
                Console.WriteLine($"{numeros[i]}");
                ProcessNumber(numeros[i]);
            }
        }
        public static void ProcessNumber(string numero)
        {
            float cpuUsage = GetCpuUsage();
            Console.WriteLine($"Número: {numero}. Uso del CPU: {cpuUsage}%.");

            if (cpuUsage > cpuMax)
            {
                ConsoleColor originalColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"El uso de CPU es mayor del {cpuMax}%, encolando tarea...");
                Console.ForegroundColor = originalColor;
                enqueuedNumbers.Add(numero); // Agregar el número a la lista de encolados
                BackgroundJob.Enqueue(() => CheckCpuAndWait(numero));
            }
        }
    }
}