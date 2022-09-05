// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

namespace procmon
{
    class Program
    {
        static void Main(string[] args)

        {
            if (args.Length < 3)
            {
                System.Console.WriteLine("Invalid input. Please, enter proccess name, its maximum lifetime (in minutes) and a monitoring frequency (in minutes).");
                return;
            }

            if (!double.TryParse(args[1], out double maxLifeTime))
            {
                System.Console.WriteLine("Invalid input. Please, enter the proccess maximum lifetime in minutes as a second argument");
                return;
            }

            if (!double.TryParse(args[2], out double monFrequency))
            {
                System.Console.WriteLine("Invalid input. Please, enter monitoring frequency in minutes as a third argument");
                return;
            }


            Process[] procs = Process.GetProcessesByName(args[0]);
            foreach (var proc in procs)
            {
                var lifetime = DateTime.Now - proc.StartTime;
                System.Console.WriteLine($"{proc.ProcessName} #{proc.Id} - {lifetime.TotalMinutes}");

                if (lifetime.TotalMinutes > maxLifeTime)
                {
                    proc.Kill();
                }

            }

        }


    }
}
