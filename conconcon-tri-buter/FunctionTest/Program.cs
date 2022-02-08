using System;

namespace FunctionTest
{
    class Program
    {
        static void Main(string[] args)
        {
            for(int i = 1; i <= 7; ++ i)
            {
                Console.WriteLine(DateTime.Now.AddDays(i).DayOfWeek);
            }
            Console.WriteLine();
            for (int i = 1; i <= 7; ++i)
                Console.WriteLine(TurnWeekDayToIndex(i));
        }

        /// <summary>
        /// 转换周日期到序列
        /// </summary>
        /// <param name="wd">周日期</param>
        /// <returns>序列</returns>
        private static int TurnWeekDayToIndex(int wd) => wd == 7 ? 1 : wd + 1;
    }
}
