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
    /// contri_graph.xaml 的交互逻辑
    /// </summary>
    public partial class ContriGraph : UserControl
    {
        private int today, hadDay, totalDays;
        private double radius = 2, size = 15, padding = 2;
        private Dictionary<DateTime, Rectangle> source = new();
        private Dictionary<DateTime, int> commits = new();
        //private DateTime[][] graph_map = new DateTime[8][];

        public ContriGraph()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            GraphInit();
            SourceInit();
            //for (int i = 1; i <= 7; ++i) graph_map[i] = new DateTime[54];
            DrawGraph();
        }

        /// <summary>
        /// 初始化 canvas 控件, 清空已有小方块
        /// </summary>
        public void GraphInit()
        {
            graph.Children.Clear();
            source.Clear();
            //graph_map = new DateTime[8][];
        }

        /// <summary>
        /// 以今日日期生成图表
        /// </summary>
        public void SourceInit()
        {
            // 按标尺生成 (日标尺: 日/二/四/六)
            DateTime now = DateTime.Now;
            today = GetDayInWeek(now.DayOfWeek);
            hadDay = TurnWeekDayToIndex(today);
            totalDays = (52 * 7) + hadDay;
            for (int i = 1; i <= totalDays; ++i)
            {
                DateTime rt_dt = now.AddDays(-totalDays + i); // 当日日期
                DataTrigger(rt_dt); // 设置值监视器
                commits.Add(rt_dt, 0); // 添加当日提交数
                Rectangle rt = new Rectangle()
                {
                    Width = size, Height = size, RadiusX = radius, RadiusY = radius, // 大小圆角
                    Stroke = new SolidColorBrush()
                    {
                        Color = Colors.WhiteSmoke // 边框
                    },
                    Fill = new SolidColorBrush()
                    {
                        Color = Color.FromArgb(0, 0, 0, 0)
                    },
                    ToolTip = $"{rt_dt:yyyy-MM-dd} : {commits[rt_dt]} commits",
                    StrokeThickness = 1
                };
                Block_EventInit(rt);
                source.Add(rt_dt, rt);
            }
        }

        /// <summary>
        /// 画图到 canvas
        /// </summary>
        public void DrawGraph()
        {
            int weekPast = 0, // 已添加几周
                toweekAdded = 0; // 本周已添加几天
            int[] dayAdded = new int[8]; // 按周几统计已添加几天
            foreach(DateTime dt in source.Keys)
            {
                int wd = GetDayInWeek(dt.DayOfWeek); // 今天是周几
                ++dayAdded[wd]; // 按周几统计加一
                Rectangle rt = source[dt]; // 从资源中按日期取出 block
                graph.Children.Add(rt);
                Canvas.SetTop(rt, (TurnWeekDayToIndex(wd) - 1) * size +
                    toweekAdded * padding); // 设置距顶
                Canvas.SetLeft(rt, weekPast * size + dayAdded[wd] * padding); // 设置距左
                ++toweekAdded; // 本周已添加加一
                //graph_map[wd][dayAdded[wd]] = dt; // 记录到二维数组
                if (dt.Day == 7)
                {
                    TextBlock tb_tmp = new TextBlock()
                    {
                        Text = dt.ToString("MMMM").Replace("月", ""),
                        Foreground = new SolidColorBrush()
                        {
                            Color = Colors.WhiteSmoke
                        }
                    };
                    ruler_top.Children.Add(tb_tmp);
                    Canvas.SetLeft(tb_tmp, weekPast * size + dayAdded[wd] * padding);
                }
                if (toweekAdded == 7)
                {
                    ++weekPast; toweekAdded = 0;
                }
            }
            Canvas.SetTop(r_2, 2 * size + 2 * padding);
            Canvas.SetTop(r_4, 4 * size + 4 * padding);
            Canvas.SetTop(r_6, 6 * size + 6 * padding);
        }

        /// <summary>
        /// 数据监视器
        /// </summary>
        /// <param name="dt">日期</param>
        private void DataTrigger(DateTime dt)
        {

        }

        /// <summary>
        /// 初始化方块事件
        /// </summary>
        /// <param name="rt">方块</param>
        private void Block_EventInit(Rectangle rt)
        {
            
        }

        /// <summary>
        /// 转换周日期到序列
        /// </summary>
        /// <param name="wd">周日期</param>
        /// <returns>序列</returns>
        private static int TurnWeekDayToIndex(int wd) => wd == 7 ? 1 : wd + 1;

        /// <summary>
        /// DayOfWeek 转 int
        /// </summary>
        /// <param name="dow">DayOfWeek</param>
        /// <returns>周几</returns>
        private static int GetDayInWeek(DayOfWeek dow)
        {
            return dow.ToString() switch
            {
                "Monday" => 1,
                "Tuesday" => 2,
                "Wednesday" => 3,
                "Thursday" => 4,
                "Friday" => 5,
                "Saturday" => 6,
                "Sunday" => 7,
                _ => -1,
            };
        }

        /// <summary>
        /// 获取一年天数
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns>天数</returns>
        private static int GetDays(int year) => IsLeapYear(year) ? 366 : 365;

        /// <summary>
        /// 判断某年是否是闰年
        /// </summary>
        /// <param name="year">某年</param>
        /// <returns>是否</returns>
        private static bool IsLeapYear(int year)
        {
            return (year % 4 == 0 && year % 100 != 0) || year % 400 == 0 ||
                (year % 3200 == 0 && year % 172800 == 0);
        }
    }
}
