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
    public partial class contri_graph : UserControl
    {

        public contri_graph()
        {
            InitializeComponent();

            Init();
        }

        private void Init()
        {
            Graph_Init();
            Source_Init();
        }

        private Dictionary<DateTime, Rectangle> source = new();

        public void Graph_Init()
        {
            graph.Children.Clear();
            source.Clear();
        }

        public void Source_Init()
        {
            // 按总天数生成
            //int days = getDays(DateTime.Now.Year);
            //for(int i = 1; i <= days; ++ i)
            //{
            //    source.Add(DateTime.Now.AddDays(1 - i), new Rectangle()
            //    {
            //        Width = 10, Height = 10, RadiusX = 5, RadiusY = 5
            //    });
            //}

            // 按标尺生成 (日标尺: 日/二/四/六)
            int today = DateTime.Now.DayOfWeek
        }

        private int getDays(int year)
        {
            bool run = (year % 4 == 0 && year % 100 != 0) || year % 400 == 0 ||
                (year % 3200 == 0 && year % 172800 == 0);
            return run ? 366 : 365;
        }
    }
}
