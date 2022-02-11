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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace cctb_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 已经选择的日期
        /// </summary>
        private List<DateTime> selections = new();

        /// <summary>
        /// 主窗口构造函数
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            contriGraph.SelectEvent += (x) =>
            {
                SelectedDates.Items.Add(x.ToString("yyyy-MM-dd"));
                selections.Add(x);
            };
            contriGraph.UnSelectEvent += (x) =>
            {
                SelectedDates.Items.Remove(x.ToString("yyyy-MM-dd"));
                selections.Remove(x);
            };
            KeyDown += (_, y) =>
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.A))
                    contriGraph.SelectAll();
            };
        }

        /// <summary>
        /// 初始化按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Init(object sender, RoutedEventArgs e)
        {
            contriGraph.Init();
            SelectedDates.Items.Clear();
        }

        /// <summary>
        /// 拖动窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }

        /// <summary>
        /// 取消全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_UnSelectAll(object sender, RoutedEventArgs e)
        {
            contriGraph.UnSelectAll();
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_SetValue(object sender, RoutedEventArgs e)
        {
            foreach (DateTime dt in selections)
            {
                if (int.TryParse(commits_num.Text, out int cnum))
                    contriGraph.SetCommit(dt, cnum);
                else MessageBox.Show("非法的提交数", "错误",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 开始提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_StartCommit(object sender, RoutedEventArgs e)
        {
            ContriGraph cgtmp = new(contriGraph.BaseDT)
            {
                VerticalAlignment = VerticalAlignment.Top,
                IsEnabled = false
            };
            foreach (DateTime dt in contriGraph.source.Keys)
                if(contriGraph.commits[dt] != 0)
                    cgtmp.SetCommit(dt, contriGraph.commits[dt]);
            StartCommitWin scw = new();
            scw.rootGrid.Children.Add(cgtmp);
            scw.ShowDialog();
        }
    }
}
