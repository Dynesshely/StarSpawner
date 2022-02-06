using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable IDE0059 // 不需要赋值

namespace conconcon_tri_buter
{
    public class ConsoleHelper
    {
        /// <summary>
        /// 简化输入的获取
        /// </summary>
        /// <param name="message">提示消息</param>
        /// <returns>用户输入</returns>
        public static string GetInput(string message)
        {
            Console.Write(message);
            return Console.ReadLine();
        }

        /// <summary>
        /// 彩色输出带复原
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="color">颜色</param>
        public static void Output(string content, ConsoleColor color)
        {
            ConsoleColor cc = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(content);
            Console.ForegroundColor = cc;
        }

        /// <summary>
        /// 从控制台获取日期 (yyyy-MM-dd)
        /// </summary>
        /// <param name="message">提示消息</param>
        /// <param name="sep">分隔符</param>
        /// <returns>输入的日期</returns>
        public static DateTime GetDate(string message, char sep)
        {
            int yyyy = DateTime.Now.Year, MM = DateTime.Now.Month, dd = DateTime.Now.Day;
            string[] dateArr = GetInput(message).Split(sep);
            yyyy = int.Parse(dateArr[0]); MM = int.Parse(dateArr[1]); dd = int.Parse(dateArr[2]);
            return new DateTime(yyyy, MM, dd);
        }
    }
}

#pragma warning restore IDE0059 // 不需要赋值
