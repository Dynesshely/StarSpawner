using System;
using System.Diagnostics;
using System.IO;

namespace conconcon_tri_buter
{
    class Program
    {
        static void Main(string[] args)
        {

            start: Console.Write("Switch one Mode:\r\n" +
                "\t1. Simply contribute every selected day\r\n" +
                "\t2. Simply contribute with random lively commit message\r\n" +
                "\t3. Lively contribute with lively commit message\r\n" +
                "\t0. Exit without anything...\r\n" +
                "Switch one Mode: "
            );

            bool anyException = false;
            try
            {
                int choose = int.Parse(Console.ReadLine());
                switch (choose)
                {
                    case 1:
                        Simply_Contribute(false);
                        break;
                    case 2:
                        Simply_Contribute(true);
                        break;
                    case 3:
                        break;
                    case 0:
                        Console.WriteLine("See you.");
                        Environment.Exit(0);
                        break;
                    default:
                        goto start;
                }
            }
            catch (Exception e)
            {
                ConsoleColor beforeColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n\n\n``` ERROR ```\n\n{e.Message}\n\n{e}\n\n" +
                    $"``` ERROR ```\n\n\n");
                Console.ForegroundColor = beforeColor;
                anyException = true;
            }

            if (anyException) goto start;

            Console.WriteLine("\n\nFinished! Go to your GitHub to see 13 !");
        }

        private static void Simply_Contribute(bool lively_message)
        {
            Console.WriteLine($"Now Directory : {Environment.CurrentDirectory}\n");
            Console.Write("How much days you want to contribute? : ");
            int days = int.Parse(Console.ReadLine());
            Console.Write("How many contribution one day pushed ('r' - random) : ");
            string rst = Console.ReadLine();
            bool random = rst == "r";
            int cons = random ? -1 : int.Parse(rst);

            Console.WriteLine("\nPress any key to continue ...\n");
            Console.ReadLine(); Console.WriteLine();

            DateTime dt = DateTime.Now, now = DateTime.Now;
            dt -= new TimeSpan(days, 0, 0, 0);
            while (dt.Year != now.Year || dt.Month != now.Month || dt.Day != now.Day)
            {
                ConsoleColor color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"date: {dt:yyyy-MM-dd} | start commit!");
                Console.ForegroundColor = color;
                for (int i = 1; i <= (random ? rand.Next(3, 26) : cons); ++i)
                    generatefile(dt, lively_message);
                dt = dt.AddDays(1);
            }
        }

        private static void generatefile(DateTime dt, bool lively_message)
        {
            string fn = $"{Environment.CurrentDirectory}\\{randomname()}.txt";

            Console.WriteLine($"generate: {Path.GetFileName(fn)}");

            if (File.Exists(fn))
                File.Delete(fn);
            FileStream fs = File.Create(fn);
            StreamWriter sw = new(fs);
            sw.WriteLine(randomname());
            sw.Flush();
            sw.Close(); fs.Close();

            string message = get_lively_message();

            if (lively_message) specialCommit(dt, message);
            else normalCommit(dt);

            File.Delete(fn);

            if (lively_message) specialCommit(dt, message);
            else normalCommit(dt);

            Console.WriteLine($"delete: {Path.GetFileName(fn)}");
        }

        private static string get_lively_message() => LivelyMessage.GetLivelyMessage();

        private static void normalCommit(DateTime dt)
        {
            runGit(" add .");
            runGit($" commit -m \"{randomname()}\" --date {dt:yyyy/MM/dd}");
        }

        private static void specialCommit(DateTime dt, string msg)
        {
            runGit(" add .");
            runGit($" commit -m \"{msg}\" --date {dt:yyyy/MM/dd}");
        }

        private static void runGit(string args)
        {
            using (Process myProcess = new Process())
            {
                myProcess.StartInfo.UseShellExecute = false;
                myProcess.StartInfo.FileName = "git.exe";
                myProcess.StartInfo.Arguments = args;
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.Start();
                myProcess.WaitForExit();
            }
        }

        static Random rand = new();

        private static string randomname()
        {
            int length = 8;
            string rst = "";
            for (int i = 1; i <= length; ++i)
                rst += rand.Next(0, 1) == 0 ? ((char)('A' + rand.Next(0, 25))).ToString() : ((char)('a' + rand.Next(0, 25))).ToString();
            return rst;
        }
    }
}
