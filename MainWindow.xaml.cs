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
using System.Drawing;
using System.Timers;
using System.Windows.Threading;
using System.Diagnostics;
using System.Threading;

namespace sprinter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    
    public partial class MainWindow : Window
    {
        bool count1 = false;
        bool count2 = false;
        bool count3 = false;
        bool count4 = false;
        bool positionC = false;
        double x1, x2;
        DispatcherTimer _timer;
        TimeSpan _time;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        Stopwatch stopWatch = new Stopwatch();
        string currentTime = string.Empty;
        public MainWindow()
        {
            InitializeComponent();
            dispatcherTimer.Tick += new EventHandler(dt_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            _time = TimeSpan.FromSeconds(3);

        }

        /*--------------------------   WINNER   -------------------------------*/
        bool win = true;
        private void Winner(System.Windows.Shapes.Rectangle player,string name)
        {
            double x = Canvas.GetLeft(player);
            if (x >= MyCanvas1.ActualWidth && win == false)
            {
                position();
                stopWatch.Stop();
                MessageBox.Show(name + " win! with " + currentTime.ToString() + " seconds");
                win = true;             
            }
        }

        private void Canvas_KeyDown1(object sender, KeyEventArgs e)
        {
            double x1 = Canvas.GetLeft(player_1);
            void move(System.Windows.Shapes.Rectangle player, Key letter1, Key letter2, bool win)
            {
                if (e.Key == letter1 && !win && count1 == false)
                {
                    Canvas.SetLeft(player, Canvas.GetLeft(player) + 5);
                    Winner(player, player11.Text);
                    count1 = true;
                    count2 = false;                   
                }
                else if (count1 == true && e.Key == letter1 && !win && x1 > 54)
                {
                    Canvas.SetLeft(player, Canvas.GetLeft(player) - 5);
                    count2 = false;
                }
                if (e.Key == letter2 && !win && count2 == false)
                {
                    Canvas.SetLeft(player, Canvas.GetLeft(player) + 5);
                    Winner(player, player11.Text);
                    count2 = true;
                    count1 = false;
                }
                else if (count2 == true && e.Key == letter2 && !win && x1 > 54)
                {
                    Canvas.SetLeft(player, Canvas.GetLeft(player) - 5);
                    count1 = false;
                }
            }

            /*--------------------------    PLAYER 1    -------------------------------*/
            move(player_1, Key.Left, Key.Right, win);

            /*--------------------------    PLAYER 2    -------------------------------*/

            double x2 = Canvas.GetLeft(player_2);
            if (e.Key == Key.A && !win && !count3)
            {
                Canvas.SetLeft(player_2, Canvas.GetLeft(player_2) + 5);
                Winner(player_2, player22.Text);
                count3 = true;
                count4 = false;
            }
            else if (count3 && e.Key == Key.A && !win && x2 > 54)
            {
                Canvas.SetLeft(player_2, Canvas.GetLeft(player_2) - 5);
                count4 = false;
            }
            if (e.Key == Key.D && !win && !count4)
            {
                Canvas.SetLeft(player_2, Canvas.GetLeft(player_2) + 5);
                Winner(player_2, player22.Text);
                count4 = true;
                count3 = false;
            }
            else if (count4 && e.Key == Key.D && !win && x2 > 54)
            {
                Canvas.SetLeft(player_2, Canvas.GetLeft(player_2) - 5);
                count3 = false;
            }
        }
        /*--------------------------    PLAY    -------------------------------*/
        private void Play(object sender, RoutedEventArgs e)
        {
            player11.Focusable = false;
            player22.Focusable = false;
            MyCanvas1.Focus();
            position();
            positionC = true;
            _timer = new DispatcherTimer(new TimeSpan(0,0,1), DispatcherPriority.Normal, delegate
            {               
                if (_time == TimeSpan.Zero) {
                    _timer.Stop();
                    stopWatch.Start();
                    dispatcherTimer.Start(); 
                }
                Countdown.Text = _time.ToString("c"); 
                _time = _time.Add(TimeSpan.FromSeconds(-1)); 
            }, Application.Current.Dispatcher); 
            _timer.Start();
            btn_play.IsEnabled = false;
        }

        /*--------------------------    CRONOMETRO    -------------------------------*/

         void dt_Tick(object sender, EventArgs e)
         {
            if (stopWatch.IsRunning)
            {     
                TimeSpan ts = stopWatch.Elapsed;
                currentTime = String.Format("{0:00}:{1:00}:{2:00}",
                ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                txtTimer.Text = currentTime; 
                Countdown.Text = _time.ToString();
                Countdown.Text = "___Go!___";
                win = false;
            }
         }

        private void Leaderboard(object sender, RoutedEventArgs e)
        {
            x1 = Canvas.GetLeft((System.Windows.Shapes.Rectangle)this.FindName("player_1"));
            x2 = Canvas.GetLeft((System.Windows.Shapes.Rectangle)this.FindName("player_2"));
            if (x1 > x2) { 
            var name = player11.Text;
            LeaderBoard viewWindow = new LeaderBoard(name,currentTime);
            viewWindow.Show();
            }
            else
            {
                var name = player22.Text;
                LeaderBoard viewWindow = new LeaderBoard(name,currentTime);
                viewWindow.Show();
            }
        }

        public void position()
        {
            var myRect1 = (System.Windows.Shapes.Rectangle)this.FindName("player_1");
            var myRect2 = (System.Windows.Shapes.Rectangle)this.FindName("player_2");
            if (positionC == false) { 
            x1 = Canvas.GetLeft(myRect1);   
            x2 = Canvas.GetLeft(myRect2);
            }
            else { 
            Canvas.SetLeft(myRect1, x1);
            Canvas.SetLeft(myRect2, x2);
            }
        }

        private void Resume(object sender, RoutedEventArgs e)
        {
            positionC = false;
            position();
            btn_play.IsEnabled = true;
            txtTimer.Text = "tempo: 00:00";
            Countdown.Text = "CountDown";
            _timer.Stop();
            stopWatch.Stop();
            stopWatch.Reset();
            dispatcherTimer.Tick += new EventHandler(dt_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            _time = TimeSpan.FromSeconds(3);
            btn_play.Focus();
            win = true;
            count1 = false;
            count2 = false;
            count3 = false;
            count4 = false;
        }
    }
}
