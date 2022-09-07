using System;
using System.Diagnostics;

namespace procmon
{
    class Program
    {
        static void Help(string[] args)
        {
            Console.WriteLine("\nprocmon [processName] [lifetime] [frequency]:\n processName - (obligatory, string) name of the process \n lifetime - (obligatory, int) maximum process lifetime in minutes \n frequency - (obligatory, int) monitoring frequency for this process\n");
        }

        // todo: in order to use this as an utility that could be better to get command and run each monitor in a separate thread inside the single instance
        // todo: maybe log information into file
        static void Main(string[] args)
        {
            // validate input arguments, show help message if input is incorrect
            // todo: parse arguments lifetime and frequency as a double and then round them when counting lifetime and interval 
            if (args.Length < 3 ||
                args[0] == "--h" ||
                args[0] == "--help" ||
                !int.TryParse(args[1], out int maxLifeTime) ||
                !int.TryParse(args[2], out int monFrequency)
                )
            {
                Help(args);
                return;
            }

            Console.WriteLine($"Monitor for process '{args[0]}' is starting.\n It would check for '{args[0]}' every {monFrequency} minutes. \nAfter {maxLifeTime} minutes of lifetime all '{args[0]}' processed will be killed. \nPress ESC to stop");

            // process is running until key ESC pressed
            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            {
                // set interval in minutes
                int intInterval = monFrequency * 60000;
                // find process by name
                Process[] procs = Process.GetProcessesByName(args[0]);

                // if there are no processes with this name log it to console
                if (procs.Length == 0)
                {
                    Console.WriteLine($"{DateTime.Now} - Can't find processes named '{args[0]}'");
                }
                // if monitor find any processes log their info to console
                else if (procs.Length > 0)
                {
                    foreach (var proc in procs)
                    {
                        var lifetime = DateTime.Now - proc.StartTime;

                        Console.WriteLine($"{DateTime.Now} - {proc.ProcessName} #{proc.Id} - {lifetime.TotalMinutes:0.000}m");

                        // kill the process if it takes too much time
                        if (lifetime.TotalMinutes > maxLifeTime)
                        {
                            proc.Kill();
                            Console.WriteLine($"{DateTime.Now} - Process '{proc.ProcessName}' #{proc.Id} has been killed.");
                        }
                    }
                }
                Thread.Sleep(intInterval);
            }

            Console.WriteLine($"Monitor for process '{args[0]}' has been stopped...");
        }
    }
}
