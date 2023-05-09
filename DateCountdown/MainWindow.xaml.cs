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

namespace DateCountdown
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
            // int year = getYear();
            TimeSpan timeSpan = targetTime - DateTime.Now;
            // if (isJFMode)
            // {
            //     timeSpan = new DateTime(year, 6, 8, 17, 0, 0) - DateTime.Now;
            // }
            TextBlockDays.Text = (neg ? timeSpan.Days : Math.Abs(timeSpan.Days)).ToString();
            if (redText || (!greenText && timeSpan.Days < 100)) TextBlockDays.Foreground = Brushes.Red;
            double detailNum = (timeSpan.Hours * 3600000 + timeSpan.Minutes * 60000 + timeSpan.Seconds * 1000 + timeSpan.Milliseconds) / 86400000.0);
            if (detailNum < 0.0) TextBlockDays.Text = "-" + TextBlockDays.Text;
            string detailStr = Math.Abs(detailNum).ToString(StringFormat);
            if (detailStr.StartsWith("1."))
            {
                try
                {
                    detailStr = ".";
                    int n = int.Parse(App.StartArgs[7]);
                    while (n-- > 0)
                    {
                        StringFormat += "9";
                    }
                }
                catch
                {
                    detailStr = ".999";
                }
            }
            TextBlockDaysDetails.Text = detailStr;
        }

        private int getYear()
        {
            TimeSpan timeSpan = new DateTime(DateTime.Now.Year, 6, 7, 9, 0, 0) - DateTime.Now;
            if (timeSpan.TotalSeconds < 0) return DateTime.Now.Year + 1;
            else return DateTime.Now.Year;
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
            Top = 20;
            Left = 0;
            Width = SystemParameters.WorkArea.Width;

            try
            {
                targetTime = new DateTime(int.Parse(App.StartArgs[1]), int.Parse(App.StartArgs[2]), int.Parse(App.StartArgs[3]), int.Parse(App.StartArgs[4]), int.Parse(App.StartArgs[5]), int.Parse(App.StartArgs[6]));
            }
            catch
            {
                targetTime = new DateTime(getYear(), 6, 7, 9, 0, 0);
            }
            
            if (App.StartArgs != null)
            {
                StringFormat = ".";
                try
                {
                    TextBlockTitle.Text = App.StartArgs[0];
                }
                catch
                {
                    TextBlockTitle.Text = "距离高考还有";
                }
                
                try
                {
                    int n = int.Parse(App.StartArgs[7]);
                    while (n-- > 0)
                    {
                        StringFormat += "0";
                    }
                }
                catch
                {
                    StringFormat = ".000";
                }

                if (App.StartArgs.Contains("-c"))
                {
                    TextBlockCopyright.Visibility = Visibility.Collapsed;
                }

                if (App.StartArgs.Contains("-t"))
                {
                    ShowInTaskbar = true;
                }

                if (App.StartArgs.Contains("-r"))
                {
                    redText = true;
                }
                
                if (App.StartArgs.Contains("-g"))
                {
                    greenText = true;
                }

                if (App.StartArgs.Contains("-l"))
                {
                    Foreground = Brushes.White;
                }

                if (App.StartArgs.Contains("-n"))
                {
                    neg = false;
                }

                // if (App.StartArgs.Contains("-jf"))
                // {
                //     TextBlockTitle.Text = "距离解放还有";
                //     isJFMode = true;
                // }
            }
        }

        bool redText = false;
        bool greenText = false;
        bool neg = true;
        // bool isJFMode = false;
        DateTime targetTime;

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                WindowState = WindowState.Normal;
            }
        }
    }
}