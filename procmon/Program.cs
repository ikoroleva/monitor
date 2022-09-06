//todo: several instances
//todo: logging information to file


using System.Diagnostics;

namespace procmon
{
    class Program
    {
        static void Help(string[] args)
        {

            System.Console.WriteLine("\nprocmon [processName, lifetime, frequency]:\n processName - (obligatory, string) name of the proccess \n lifetime - (obligatory, int) maximum process lifetime in minutes \n frequency - (obligatory, int) monitoring frequency for this process\n");

        }
        static void Main(string[] args)

        {
            //validate input arguments, show help message if input is incorrect
            //todo: parse arguments lifetime and frequency as a double and then round them when counting lifetime and interval 
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


            Console.WriteLine($"Monitor for proccess '{args[0]}' is starting.\n It would check for '{args[0]}' every {monFrequency} minutes. \nAfter {maxLifeTime} minutes of lifetime all '{args[0]}' processed will be killed. \nPress ESC to stop");


            //proccess is running until key ESC woudln't pressed
            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            {
                int intInterval = monFrequency * 60000; //set interval in minutes
                Process[] procs = Process.GetProcessesByName(args[0]); //find proccess by name

                //if there are no proccesses with this name log it in console
                if (procs.Length == 0)
                {
                    Console.WriteLine($"{DateTime.Now} - Can't find processes named '{args[0]}'");
                }
                //if monitor find any proccesses log their info in console
                else if (procs.Length > 0)
                {
                    foreach (var proc in procs)
                    {
                        var lifetime = DateTime.Now - proc.StartTime;

                        System.Console.WriteLine($"{DateTime.Now} - {proc.ProcessName} #{proc.Id} - {lifetime.TotalMinutes}");

                        if (lifetime.TotalMinutes > maxLifeTime) //kill the process if it takes too much time
                        {
                            proc.Kill();
                            Console.WriteLine($"{DateTime.Now} - Proccess '{proc.ProcessName}' #{proc.Id} was killed.");
                        }
                    }
                }
                Thread.Sleep(intInterval);
            }

            System.Console.WriteLine($"Monitor for proccess '{args[0]}' was stopped...");
        }
    }
}
