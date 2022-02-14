using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace cctb_wpf
{
    /// <summary>
    /// StartCommitWin.xaml 的交互逻辑
    /// </summary>
    public partial class StartCommitWin : Window
    {
        public StartCommitWin()
        {
            InitializeComponent();
        }

        public void operation(ContriGraph cg)
        {
            TextBox tb = new()
            {
                IsReadOnly = true,
                Margin = new Thickness(10),
                Foreground = new SolidColorBrush()
                {
                    Color = Colors.WhiteSmoke
                },
                Background = new SolidColorBrush()
                {
                    Color = Colors.Transparent
                }
            };
            ScrollViewer sv = new()
            {
                Content = tb,
                Margin = new Thickness(10)
            };
            Window log_win = new()
            {
                Title = "提交日志",
                Left = 10,
                Top = 10,
                Width = 400,
                Height = 225,
                Background = new SolidColorBrush()
                {
                    Color = Colors.Black
                },
                Content = sv
            };
            LogDelegate log = delegate (string content)
            {
                tb.Text += $"{content}";
            };
            log_win.Show();
            log("开始装逼!\n");
            int index = 1;
            foreach (DateTime item in cg.source.Keys)
            {
                if (cg.commits[item] != 0)
                {
                    log($"\n{index}. 开始生成: {item:yyyy-MM-dd}\n");
                    for(int i = 1; i <= cg.commits[item]; ++ i)
                    {
                        log($"\t{i}. fn: {conconcon_tri_buter.Program.randomname()}\n" +
                            $"\tmessage: {conconcon_tri_buter.LivelyMessage.GetLivelyMessage()}\n");
                        // {Pro_space(i.ToString().Length)}
                    }
                    ++index;
                }
            }
        }

        private string Pro_space(int num)
        {
            string rst = "";
            for (int i = 0; i < num; ++i) rst += ' ';
            return rst;
        }

        private delegate void LogDelegate(string content);
    }
}
