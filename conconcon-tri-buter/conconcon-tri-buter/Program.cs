using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using ch = conconcon_tri_buter.ConsoleHelper;

namespace conconcon_tri_buter
{
    public class Program
    {
        /// <summary>
        /// 主函数
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            start: Console.Write("Switch one Mode:\r\n" +
                "\t1. Simply contribute every selected day\r\n" +
                "\t2. Simply contribute with random lively commit message\r\n" +
                "\t3. Lively contribute with lively commit message\r\n" +
                "\t4. Lively contribute with lively commit message with density\r\n" +
                "\t0. Exit without anything...\r\n" +
                "Switch one Mode: "
            );
            bool anyException = false;
            try
            {
                int choose = int.Parse(Console.ReadLine());
                if (choose == 0)
                {
                    ch.Output("See you.", ConsoleColor.DarkBlue);
                    Environment.Exit(0);
                }
                bool push_afterAll = ch.GetInput("Push after commit ? (y/n) : ") == "y";
                switch (choose)
                {
                    case 1: Simply_Contribute(false, false); break;
                    case 2: Simply_Contribute(true, false); break;
                    case 3: Simply_Contribute(true, true); break;
                    case 4: Density_Contribute(true); break;
                    default: goto start;
                }
                ch.Output("\nStart Pushing ...", ConsoleColor.DarkMagenta);
                if (push_afterAll) runGit(" push");
                ch.Output("\nPush finished !", ConsoleColor.DarkMagenta);
            }
            catch (Exception e)
            {
                ch.Output($"\n\n\n``` ERROR ```\n\n{e.Message}\n\n{e}\n\n" +
                    $"``` ERROR ```\n\n\n", ConsoleColor.Red);
                anyException = true;
            }
            if (anyException) goto start;
            ch.GetInput("\n\nFinished! Go to your GitHub to see 13 !");
        }

        /// <summary>
        /// 简单消息评论
        /// </summary>
        /// <param name="lively_message">是否启用拟真消息</param>
        private static void Simply_Contribute(bool lively_message, bool customDate)
        {
            Console.WriteLine($"Now Directory : {Environment.CurrentDirectory}\n");
            int days = int.Parse(ch.GetInput("How much days you want to contribute? : "));
            DateTime cusD = DateTime.Now;
            if (customDate)
                cusD = ch.GetDate("Input end date (format: yyyy-MM-dd) : ", '-');
            string rst = ch.GetInput("How many contribution one day pushed ('r' - random) : ");
            bool random = rst == "r";
            int cons = random ? -1 : int.Parse(rst);
            Console.WriteLine("\nPress any key to continue ...\n");
            Console.ReadLine(); Console.WriteLine();
            DateTime dt = customDate ? cusD : DateTime.Now,
                now = customDate ? cusD : DateTime.Now;
            dt -= new TimeSpan(days, 0, 0, 0);
            int finishedDays = 1;
            while (dt.Year != now.Year || dt.Month != now.Month || dt.Day != now.Day)
            {
                ch.Output($"{finishedDays}.\t| date: {dt:yyyy-MM-dd} | start commit!\n", ConsoleColor.Blue);
                for (int i = 1; i <= (random ? rand.Next(3, 41) : cons); ++i)
                    generatefile(dt, lively_message, false);
                dt = dt.AddDays(1);
                ++finishedDays;
            }
        }

        /// <summary>
        /// 带密度的提交
        /// </summary>
        /// <param name="lively_message">是否使用拟真消息</param>
        private static void Density_Contribute(bool lively_message)
        {
            DateTime sdt = ch.GetDate("Input start date (yyyy-MM-dd): ", '-'),
                edt = ch.GetDate("Input end date (yyyy-MM-dd): ", '-');
            double density = double.Parse(ch.GetInput("Input density (0 - 1): "));
            int maxi = int.Parse(ch.GetInput("Input maxi commit per day : "));
            bool flexible = ch.GetInput("Flexible commits per day ? (y/n) : ").Equals("y");
            int totalDay = (int)(edt - sdt).TotalDays, needDay = (int)(totalDay * density);
            ArrayList existed = new()
            {
                Capacity = totalDay
            };
            Math math = new();
            for (int i = 0; i < totalDay; ++i) existed.Add(false);
            for (int i = 1; i <= needDay; ++i)
                existed[(int)math.AverageRandom(0, totalDay)] = true;
            List<DateTime> targets = new();
            for (int i = 0; i < totalDay; ++i)
                if ((bool)existed[i]) targets.Add(sdt.AddDays(i));
            string tar_dates = "\ntarget dates:\n";
            foreach(DateTime dt in targets)
                tar_dates += $"{dt:yyyy-MM-dd} ";
            ch.Output($"{tar_dates}\n\n", ConsoleColor.Green);
            foreach (DateTime dt in targets)
            {
                ch.Output($"date: {dt:yyyy-MM-dd} | start commit!\n", ConsoleColor.Cyan);
                int this_turn_max = rand.Next(1, 9) > 7 ?
                    rand.Next(maxi / 2, maxi) : rand.Next(1, maxi / 2);
                for(int i = 1; i <= (flexible ? this_turn_max : maxi); ++ i)
                    generatefile(dt.AddHours(rand.Next(1, 23)), lively_message, false);
            }
        }

        /// <summary>
        /// 生成文件并提交, 删除文件并提交
        /// </summary>
        /// <param name="dt">日期</param>
        /// <param name="lively_message">是否启用拟真消息</param>
        private static void generatefile(DateTime dt, bool lively_message, bool commit_when_delete)
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

            if (commit_when_delete)
            {
                if (lively_message) specialCommit(dt, message);
                else normalCommit(dt);
            }

            Console.WriteLine($"delete: {Path.GetFileName(fn)}");
        }

        /// <summary>
        /// 生成文件并提交, 删除文件并提交
        /// </summary>
        /// <param name="dt">日期</param>
        /// <param name="lively_message">是否启用拟真消息</param>
        private static void generatefile(DateTime dt, bool lively_message, bool commit_when_delete, string rootDir)
        {
            string fn = $"{rootDir}\\{randomname()}.txt";

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

            if (commit_when_delete)
            {
                if (lively_message) specialCommit(dt, message);
                else normalCommit(dt);
            }

            Console.WriteLine($"delete: {Path.GetFileName(fn)}");
        }

        /// <summary>
        /// 获取一条拟真消息
        /// </summary>
        /// <returns>拟真消息</returns>
        private static string get_lively_message() => LivelyMessage.GetLivelyMessage();

        /// <summary>
        /// 普通提交
        /// </summary>
        /// <param name="dt">提交日期</param>
        private static void normalCommit(DateTime dt)
        {
            runGit(" add .");
            runGit($" commit -m \"{randomname()}\" --date {dt:yyyy/MM/dd}");
        }

        /// <summary>
        /// 特殊提交
        /// </summary>
        /// <param name="dt">提交日期</param>
        /// <param name="msg">提交消息</param>
        private static void specialCommit(DateTime dt, string msg)
        {
            runGit(" add .");
            runGit($" commit -m \"{msg}\" --date {dt:yyyy/MM/dd}");
        }

        /// <summary>
        /// 启动 Git
        /// </summary>
        /// <param name="args">启动参数</param>
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

        /// <summary>
        /// 随机名称
        /// </summary>
        /// <returns>名称字符串</returns>
        public static string randomname()
        {
            int length = 8;
            string rst = "";
            for (int i = 1; i <= length; ++i)
                rst += rand.Next(0, 2) == 0 ? ((char)('A' + rand.Next(0, 25))).ToString() : ((char)('a' + rand.Next(0, 25))).ToString();
            return rst;
        }
    }
}
