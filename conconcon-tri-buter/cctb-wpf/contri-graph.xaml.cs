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
        /// <summary>
        /// 构造函数
        /// </summary>
        public ContriGraph()
        {
            InitializeComponent();
            Init();
        }

        #region 事件定义
        /// <summary>
        /// 选择事件
        /// </summary>
        /// <param name="dt"><日期/param>
        public delegate void SelectDelegate(DateTime dt);
        public event SelectDelegate SelectEvent;
        public delegate void UnSelectDelegate(DateTime dt);
        public event UnSelectDelegate UnSelectEvent;
        private delegate void DataDelegate();
        private event DataDelegate DataChanged;
        #endregion

        #region 预定义
        private int today, hadDay, totalDays;
        private double radius = 2, size = 15, padding = 2;
        public Dictionary<DateTime, Rectangle> source = new(); // 每个 dt 对应的 block
        public Dictionary<DateTime, int> commits = new(); // 每个 dt 的 commits 有多少
        public Dictionary<DateTime, bool> selected = new(); // 所有 dt 的选择情况
        public Dictionary<string, List<DateTime>> month_dts = new(); // 按月收集的 dt
        public Dictionary<string, bool> month_allin = new(); // 按月是否全部选择
        public List<DateTime> selected_dts = new(); // 已经选择的 dt 
        #endregion

        #region 颜色预定义
        /// <summary>
        /// 普通边框
        /// </summary>
        private static SolidColorBrush Block_Stroke_Normal = new()
        {
            Color = Colors.WhiteSmoke
        };
        /// <summary>
        /// 选择边框
        /// </summary>
        private static SolidColorBrush Block_Stroke_Selected = new()
        {
            Color = Colors.DarkRed
        };
        /// <summary>
        /// 高亮边框
        /// </summary>
        private static SolidColorBrush Block_Stroke_Highlight = new()
        {
            Color = Colors.Cyan
        };
        /// <summary>
        /// 普通背景
        /// </summary>
        private static SolidColorBrush Block_Fill_Normal = new()
        {
            Color = Colors.Transparent
        }; 
        #endregion

        #region 初始化
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
            commits.Clear();
            selected.Clear();
            month_dts.Clear();
            month_allin.Clear();
            selected_dts.Clear();
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
                selected.Add(rt_dt, false); // 设置为未选中
                Rectangle rt = new Rectangle()
                {
                    Width = size,
                    Height = size,
                    RadiusX = radius,
                    RadiusY = radius, // 大小圆角
                    Stroke = Block_Stroke_Normal,
                    Fill = Block_Fill_Normal,
                    ToolTip = $"{rt_dt:yyyy-MM-dd} : {commits[rt_dt]} commits",
                    StrokeThickness = 1
                };
                source.Add(rt_dt, rt);
                Block_EventInit(rt_dt);
            }
        } 
        #endregion

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
                string month_auth = dt.ToString("yyyy-MM");
                if (!month_dts.ContainsKey(month_auth))
                {
                    month_dts.Add(month_auth, new List<DateTime>());
                    month_allin.Add(month_auth, false);
                }
                month_dts[month_auth].Add(dt);
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
                        },
                        Tag = dt.ToString("yyyy-MM")
                    };
                    TextBlock_Ruler_Month_EventInit(tb_tmp);
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
            Canvas.SetTop(rr_1, 1 * size + 1 * padding);
            Canvas.SetTop(rr_3, 3 * size + 3 * padding);
            Canvas.SetTop(rr_5, 5 * size + 5 * padding);
        }

        /// <summary>
        /// 数据监视器
        /// </summary>
        /// <param name="dt">日期</param>
        private void DataTrigger(DateTime dt)
        {
            DataChanged += () =>
            {
                source[dt].ToolTip = $"{dt:yyyy-MM-dd} : {commits[dt]} commits";
            };
        }

        public void AddCommit(DateTime dt, int num)
        {
            commits[dt] += num;
            DataChanged.Invoke();
        }

        #region 初始化事件
        /// <summary>
        /// 初始化方块事件
        /// </summary>
        /// <param name="rt">方块</param>
        private void Block_EventInit(DateTime dt)
        {
            Rectangle rt = source[dt];
            rt.MouseDown += (_, _) =>
            {
                if (selected[dt]) UnSelect(dt);
                else Select(dt);
            };
        }

        /// <summary>
        /// 初始化月份标尺事件
        /// </summary>
        /// <param name="tb">月份标尺</param>
        private void TextBlock_Ruler_Month_EventInit(TextBlock tb)
        {
            tb.MouseEnter += (_, _) =>
            {
                foreach (DateTime dt in source.Keys)
                    if (dt.ToString("yyyy-MM").Equals(tb.Tag.ToString()))
                        source[dt].Stroke = Block_Stroke_Highlight;
            };
            tb.MouseLeave += (_, _) =>
            {
                foreach (DateTime dt in source.Keys)
                {
                    if (dt.ToString("yyyy-MM").Equals(tb.Tag.ToString()))
                        source[dt].Stroke = Block_Stroke_Normal;
                    if (selected[dt])
                        source[dt].Stroke = Block_Stroke_Selected;
                }
            };
            tb.MouseDown += (_, _) =>
            {
                string m_auth = tb.Tag.ToString();
                if (!month_allin[m_auth])
                {
                    foreach (DateTime dt in month_dts[m_auth]) Select(dt);
                    month_allin[m_auth] = true;
                }
                else
                {
                    foreach (DateTime dt in month_dts[m_auth]) UnSelect(dt);
                    month_allin[m_auth] = false;
                }
            };
        }

        /// <summary>
        /// 初始化周几标尺事件
        /// </summary>
        /// <param name="tb">周几标尺</param>
        private void TextBlock_Ruler_Day_EventInit(TextBlock tb)
        {
            tb.MouseEnter += (_, _) =>
            {

            };
        }
        #endregion

        #region 选择与取消选择
        /// <summary>
        /// 选择
        /// </summary>
        /// <param name="dt">日期</param>
        public void Select(DateTime dt)
        {
            if (!selected[dt])
            {
                Rectangle rt = source[dt];
                rt.Stroke = Block_Stroke_Selected;
                selected[dt] = true;
                selected_dts.Add(dt);
                SelectEvent.Invoke(dt);
            }
        }

        /// <summary>
        /// 选择全部
        /// </summary>
        public void SelectAll()
        {
            foreach (DateTime dt in source.Keys) Select(dt);
        }

        /// <summary>
        /// 取消选择
        /// </summary>
        /// <param name="dt">日期</param>
        public void UnSelect(DateTime dt)
        {
            if (selected[dt])
            {
                Rectangle rt = source[dt];
                rt.Stroke = Block_Stroke_Normal;
                selected[dt] = false;
                selected_dts.Remove(dt);
                UnSelectEvent.Invoke(dt);
            }
        }

        /// <summary>
        /// 取消选择全部
        /// </summary>
        public void UnSelectAll()
        {
            List<DateTime> tmp = new();
            foreach (DateTime dt in selected_dts) tmp.Add(dt);
            foreach (DateTime dt in tmp) UnSelect(dt);
        }
        #endregion

        #region 帮助函数
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
        #endregion
    }
}
