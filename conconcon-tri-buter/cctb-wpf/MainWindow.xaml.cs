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
        private Dictionary<DateTime, string> selections = new();

        public MainWindow()
        {
            InitializeComponent();
            contriGraph.SelectEvent += (x) =>
            {
                SelectedDates.Items.Add(x.ToString("yyyy-MM-dd tt"));
            };
            contriGraph.UnSelectEvent += (x) =>
            {
                SelectedDates.Items.Remove(x.ToString("yyyy-MM-dd tt"));
            };
            KeyDown += (_, y) =>
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.A))
                    contriGraph.SelectAll();
            };
        }

        private void Button_Click_Init(object sender, RoutedEventArgs e)
        {
            contriGraph.Init();
            SelectedDates.Items.Clear();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }
    }
}
