using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace cctb_wpf
{
    /// <summary>
    /// contri_graph.xaml 的交互逻辑
    /// </summary>
    public partial class ContriGraph : UserControl
    {
        /// <summary>
        /// 是否拥有基准日期
        /// </summary>
        public bool hadBDT = false;
        /// <summary>
        /// 基准日期
        /// </summary>
        public DateTime BaseDT;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ContriGraph()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ContriGraph(DateTime baseDt)
        {
            InitializeComponent();
            hadBDT = true;
            BaseDT = baseDt;
            Init();
        }

        #region 事件定义
        /// <summary>
        /// 选择事件
        /// </summary>
        /// <param name="dt"><日期/param>
        public delegate void SelectDelegate(DateTime dt);
        public event SelectDelegate SelectEvent;
        /// <summary>
        /// 取消选择事件
        /// </summary>
        /// <param name="dt">日期</param>
        public delegate void UnSelectDelegate(DateTime dt);
        public event UnSelectDelegate UnSelectEvent;
        /// <summary>
        /// 数据相关事件
        /// </summary>
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
        public DateTime[][] day_dts = new DateTime[8][]; // 按天收集的 dt
        public bool[][] day_allin = new bool[8][]; // 按天是否全部选择
        #endregion

        #region 颜色预定义
        /// <summary>
        /// 普通边框
        /// </summary>
        public static SolidColorBrush Block_Stroke_Normal = new()
        {
            Color = Colors.WhiteSmoke
        };
        /// <summary>
        /// 选择边框
        /// </summary>
        public static SolidColorBrush Block_Stroke_Selected = new()
        {
            Color = Colors.DarkRed
        };
        /// <summary>
        /// 高亮边框
        /// </summary>
        public static SolidColorBrush Block_Stroke_Highlight = new()
        {
            Color = Colors.Cyan
        };
        /// <summary>
        /// 普通背景
        /// </summary>
        public static SolidColorBrush Block_Fill_Normal = new()
        {
            Color = Colors.Transparent
        };
        /// <summary>
        /// 提交背景 - 少
        /// </summary>
        public static SolidColorBrush Block_Fill_Less = new()
        {
            Color = Color.FromRgb(14, 68, 41)
        };
        /// <summary>
        /// 提交背景 - 中下
        /// </summary>
        public static SolidColorBrush Block_Fill_Medium_Less = new()
        {
            Color = Color.FromRgb(0, 109, 50)
        };
        /// <summary>
        /// 提交背景 - 中上
        /// </summary>
        public static SolidColorBrush Block_Fill_Medium_More = new()
        {
            Color = Color.FromRgb(38, 166, 65)
        };
        /// <summary>
        /// 提交背景 - 多
        /// </summary>
        public static SolidColorBrush Block_Fill_More = new()
        {
            Color = Color.FromRgb(57, 211, 83)
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
            DrawGraph();
        }

        /// <summary>
        /// 初始化 canvas 控件, 清空已有小方块
        /// </summary>
        public void GraphInit()
        {
            graph.Children.Clear();
            source.Clear();
            source = new();
            commits.Clear();
            commits = new();
            selected.Clear();
            selected = new();
            month_dts.Clear();
            month_dts = new();
            month_allin.Clear();
            month_allin = new();
            selected_dts.Clear();
            selected_dts = new();
            day_dts = new DateTime[8][];
            day_allin = new bool[8][];
        }

        /// <summary>
        /// 以今日日期生成图表
        /// </summary>
        public void SourceInit()
        {
            // 按标尺生成 (日标尺: 日/二/四/六)
            DateTime now = hadBDT ? BaseDT : DateTime.Now; // 是否按照基准生成
            if (!hadBDT) BaseDT = now;
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
                    Width = size, // 宽度
                    Height = size, // 高度
                    RadiusX = radius, // 水平圆角
                    RadiusY = radius, // 垂直圆角
                    Stroke = Block_Stroke_Normal, // 边框
                    Fill = Block_Fill_Normal, // 填充
                    ToolTip = $"{rt_dt:yyyy-MM-dd} : {commits[rt_dt]} commits", // 提示
                    StrokeThickness = 1 // 边框粗细
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

        #region 数据相关操作
        /// <summary>
        /// 按提交数进行颜色分级
        /// </summary>
        /// <param name="cnum">提交次数</param>
        private static SolidColorBrush CommitColorGrade(int cnum)
        {
            if (cnum == 0) return Block_Fill_Normal;
            else if (cnum > 0 && cnum < 10) return Block_Fill_Less;
            else if (cnum < 30) return Block_Fill_Medium_Less;
            else if (cnum < 70) return Block_Fill_Medium_More;
            else return Block_Fill_More;
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
                source[dt].Fill = CommitColorGrade(commits[dt]);
            };
        }

        /// <summary>
        /// 增加提交数
        /// </summary>
        /// <param name="dt">日期</param>
        /// <param name="num">数量</param>
        public void AddCommit(DateTime dt, int num)
        {
            commits[dt] += num;
            DataChanged.Invoke();
        }

        /// <summary>
        /// 设置提交数
        /// </summary>
        /// <param name="dt">日期</param>
        /// <param name="num">数量</param>
        public void SetCommit(DateTime dt, int num)
        {
            if (num < 0) throw new InvalidOperationException("提交数不能小于零");
            commits[dt] = num;
            DataChanged.Invoke();
        } 
        #endregion

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
                if (IsEnabled)
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
                if(IsEnabled)
                    foreach (DateTime dt in source.Keys)
                        if (dt.ToString("yyyy-MM").Equals(tb.Tag.ToString()))
                            source[dt].Stroke = Block_Stroke_Highlight;
            };
            tb.MouseLeave += (_, _) =>
            {
                if (IsEnabled)
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
                if (IsEnabled)
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
                SelectEvent?.Invoke(dt);
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
                UnSelectEvent?.Invoke(dt);
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
