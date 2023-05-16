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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Effects;
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

        // 利用 win32 接口实现窗体鼠标事件穿透
        const int WS_EX_TRANSPARENT = 0x20;
        const int GWL_EXSTYLE = -20;

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            if (App.StartArgs != null && App.StartArgs.Contains("-k"))
            {

                // Get this window's handle
                IntPtr hwnd = new WindowInteropHelper(this).Handle;

                // Change the extended window style to include WS_EX_TRANSPARENT
                int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
                SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
            }
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        static extern bool IsZoomed(IntPtr hWnd);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        public static bool IsForegroundFullScreen()
        {
            IntPtr handle = GetForegroundWindow();
            RECT rect;
            GetWindowRect(handle, out rect);
            return rect.Left <= 0 && rect.Top <= 0 && rect.Right >= System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width && rect.Bottom >= System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
        }

        public static bool IsMaximized() {
            return IsZoomed(GetForegroundWindow());
        }


        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            // int year = getYear();
            TimeSpan timeSpan = targetTime - DateTime.Now;
            // if (isJFMode)
            // {
            //     timeSpan = new DateTime(year, 6, 8, 17, 0, 0) - DateTime.Now;
            // }
            double detailNum = (timeSpan.Hours * 3600000 + timeSpan.Minutes * 60000 + timeSpan.Seconds * 1000 + timeSpan.Milliseconds) / 86400000.0;
            TextBlockDays.Text = ((timeSpan.Days < 0 || detailNum < 0.0) && neg ? "-" + Math.Abs(timeSpan.Days).ToString() : Math.Abs(timeSpan.Days).ToString());
            if (!reded && (redText || (!greenText && (timeSpan.Days < 100))))
            {
                reded = true;
                TextBlockDays.Foreground = new SolidColorBrush(Colors.Red) { Opacity = alpha ? 0.5 : 1 };
            }
            string detailStr = Math.Abs(detailNum).ToString(StringFormat);
            if (detailStr.StartsWith("1."))
            {
                try
                {
                    detailStr = ".";
                    int n = int.Parse(App.StartArgs[7]);
                    while (n-- > 0)
                    {
                        detailStr += "9";
                    }
                }
                catch
                {
                    detailStr = ".999";
                }
            }
            TextBlockDaysDetails.Text = detailStr;

            if (Topmost) {
                if (!isFullScreen && (IsForegroundFullScreen() || IsMaximized())) {
                    isFullScreen = true;
                    transState = 0.0;
                }
                if (isFullScreen) {
                    if (IsForegroundFullScreen() || IsMaximized()) {
                        if ((transState += 0.01) > 3.14)
                            transState = -3.14;
                        Foreground = new SolidColorBrush(light ? Colors.White : Colors.Black) { Opacity = (Math.Cos(transState) + 1.0) / (alpha ? 4.0 : 2.0) };
                        TextBlockDays.Foreground = new SolidColorBrush(((SolidColorBrush)TextBlockDays.Foreground).Color) { Opacity = (Math.Cos(transState) + 1.0) / (alpha ? 4.0 : 2.0) };
                    } else {
                        isFullScreen = false;
                        Foreground = new SolidColorBrush(light ? Colors.White : Colors.Black) { Opacity = alpha ? 0.5 : 1.0 };
                        TextBlockDays.Foreground = new SolidColorBrush(((SolidColorBrush)TextBlockDays.Foreground).Color) { Opacity = alpha ? 0.5 : 1.0 };
                    }
                }
            }
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
                    if (TextBlockTitle.Text == "") {
                        TextBlockTitle.Text = "距离高考还有";
                    }
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

                if (App.StartArgs.Contains("-l"))
                {
                    light = true;
                    Foreground = Brushes.White;
                }

                if (App.StartArgs.Contains("-a")) {
                    alpha = true;
                    Foreground = new SolidColorBrush(light ? Colors.White : Colors.Black) { Opacity = 0.5 };
                    TextBlockDays.Foreground = new SolidColorBrush(((SolidColorBrush)TextBlockDays.Foreground).Color) { Opacity = 0.5 };
                }

                if (App.StartArgs.Contains("-r")) {
                    redText = true;
                } else if (!App.StartArgs.Contains("-g")) {
                    greenText = false;
                }

                if (App.StartArgs.Contains("-n"))
                {
                    neg = true;
                }

                if (App.StartArgs.Contains("-b"))
                {
                    TextBlockTitle.Effect = AfterText.Effect = TextBlockCopyright.Effect = new DropShadowEffect { Color = light ? Colors.Black : Colors.White, Direction = 320, ShadowDepth = 0, BlurRadius = alpha ? 15 : 5 };
                    TextBlockDays.Effect = TextBlockDaysDetails.Effect = new DropShadowEffect { Color = light ? Colors.White : Colors.Black, Direction = 320, ShadowDepth = 0, BlurRadius = alpha ? 15 : 5 };
                }

                if (App.StartArgs.Contains("-p"))
                {
                    Topmost = true;
                }

                // if (App.StartArgs.Contains("-jf"))
                // {
                //     TextBlockTitle.Text = "距离解放还有";
                //     isJFMode = true;
                // }
            } else {
                greenText = false;
            }
        }

        bool redText = false;
        bool greenText = true;
        bool neg = false;
        bool light = false;
        bool alpha = false;
        bool reded = false;
        bool isFullScreen = false;
        double transState = 0.0;
        // bool isJFMode = false;
        DateTime targetTime = new DateTime(2000, 1, 1, 0, 0, 0);

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                WindowState = WindowState.Normal;
            }
        }
    }
}
