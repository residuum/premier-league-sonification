using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using PremierLeagueTable;
using PremierLeagueTable.WebData;

namespace ConsoleImplementation
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Controller output = Controller.GetInstance(AssetsFolder, UseJack))
            {
                output.Downloaded += ((sender, eventargs) =>
                {
                    output.Sonify(eventargs.Table);
                });
                output.TeamStarting += ((sender, eventargs) =>
                {
                    Team team = eventargs.Team;
                    Console.WriteLine("{0:00} | {1} | {2:00} | {3:00} | {4:+00;-00;} | {5:00}", team.Position, team.Name.PadRight(25), team.GoalsFor, team.GoalsAgainst, team.GoalDifference, team.Points);
                });
                output.Download();
                Console.WriteLine("   | Team                      | GF | GA |  GD | Pts");
                Console.WriteLine("----------------------------------------------------");
                Console.Read();
            }
        }

        static string AssetsFolder
        {
            get {
                string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                return Path.GetFullPath(assemblyFolder + ConfigurationManager.AppSettings["baseFolder"]);
            }
        }

        static bool UseJack
        {
            get {
                return ConfigurationManager.AppSettings["jack"].ToLower() == "true";
            }
        }
    }
}
