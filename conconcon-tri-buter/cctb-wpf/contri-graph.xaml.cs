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

        public ContriGraph()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            GraphInit();
            SourceInit();
        }

        private Dictionary<DateTime, Rectangle> source = new();

        public void GraphInit()
        {
            graph.Children.Clear();
            source.Clear();
        }

        public void SourceInit()
        {
            // 按标尺生成 (日标尺: 日/二/四/六)
            DateTime now = DateTime.Now;
            int today = GetDayInWeek(now.DayOfWeek);
            int hadDay = today == 7 ? 1 : today + 1;
            int totalDays = (52 * 7) + hadDay;
            for(int i = 0; i <= totalDays; ++ i)
            {
                source.Add(now.AddDays(-totalDays + i), new Rectangle()
                {
                    Width = 10, Height = 10, RadiusX = 5, RadiusY = 5
                });
            }
            //string test = "";
            //foreach (DateTime dt in source.Keys)
            //    test += dt.ToString("yyyy-MM-dd") + " ";
            //MessageBox.Show(test);
        }

        private static int GetDayInWeek(DayOfWeek dow)
        {
            return dow.ToString() switch
            {
                "Monday" => 1,
                "Tuesday" => 2,
                "Wednesday" => 2,
                "Thursday" => 2,
                "Friday" => 2,
                "Saturday" => 2,
                "Sunday" => 2,
                _ => -1,
            };
        }

        private static int GetDays(int year)
        {
            bool run = (year % 4 == 0 && year % 100 != 0) || year % 400 == 0 ||
                (year % 3200 == 0 && year % 172800 == 0);
            return run ? 366 : 365;
        }
    }
}
