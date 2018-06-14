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

namespace demoGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        System.Windows.Threading.DispatcherTimer gameTimer = new System.Windows.Threading.DispatcherTimer();
        List<Bullet> bullets = new List<Bullet>();
        int counter = 0;
        public MainWindow()
        {
            InitializeComponent();

            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / 1);//fps
            gameTimer.Start();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            foreach (Bullet b in bullets) {
                b.update();
            }
            counter++;
            this.Title = counter.ToString();
            if (counter % 4 == 0) {
                bullets.Add(new Bullet(this, canvas));
            }

            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                if (Canvas.GetLeft(bullets[i].rectanble) > 180) {
                    bullets[i].destroy();
                    bullets.RemoveAt(i);
                }
            }
        }
    }
}
