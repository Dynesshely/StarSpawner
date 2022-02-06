using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
