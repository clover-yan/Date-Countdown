using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GaokaoCountdown
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private DispatcherTimer dispatcherTimer = null;
        string StringFormat = ".000";

        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer_Tick(null, null);

            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            dispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan timeSpan = new DateTime(2022, 6, 7, 9, 0, 0) - DateTime.Now;
            TextBlockDays.Text = timeSpan.Days.ToString();
            if (timeSpan.Days < 100) TextBlockDays.Foreground = Brushes.Red;
            string detailStr = ((timeSpan.Hours * 3600000 + timeSpan.Minutes * 60000 + timeSpan.Seconds * 1000 + timeSpan.Milliseconds) / 86400000.0).ToString(StringFormat);
            TextBlockDaysDetails.Text = detailStr;
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Top = 150;
            Left = 0;
            Width = SystemParameters.WorkArea.Width;

            if (App.StartArgs != null)
            {
                StringFormat = ".";
                try
                {
                    int n = int.Parse(App.StartArgs[0]);
                    while (n-- > 0)
                    {
                        StringFormat += "0";
                    }
                }
                catch
                {
                    StringFormat = ".0";
                }

                if (App.StartArgs.Contains("-c"))
                {
                    TextBlockCopyright.Visibility = Visibility.Collapsed;
                }

                if (App.StartArgs.Contains("-t"))
                {
                    ShowInTaskbar = true;
                }
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                WindowState = WindowState.Normal;
            }
        }
    }
}