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
        }
    }
}
