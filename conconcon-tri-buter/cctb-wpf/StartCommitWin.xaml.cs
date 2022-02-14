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
            TextBox tb = new()
            {
                IsReadOnly = true,
                Margin = new Thickness(10),
                Foreground = new SolidColorBrush()
                {
                    Color = Colors.WhiteSmoke
                }
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
                Content = tb
            };
            LogDelegate log = delegate(string content)
            {
                tb.Text += $"\n{content}";
            };
            
            Activated += (_, _) =>
            {
                log_win.Show();
                log("开始装逼!");

            };
        }

        private delegate void LogDelegate(string content);
    }
}
