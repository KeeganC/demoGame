using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace demoGame
{
    class Bullet
    {
        public Rectangle rectanble = new Rectangle();
        MainWindow mainWindow;
        Canvas canvas;
        double rleft = 0;

        public Bullet(MainWindow mw, Canvas c) {
            mainWindow = mw;
            canvas = c;
            rectanble.Height = 50;
            rectanble.Width = 50;
            rectanble.Fill = Brushes.Blue;
            canvas.Children.Add(rectanble);
            Canvas.SetLeft(rectanble, rleft);
        }

        public void update() {
            rleft += 60.0/ 4.0;
            Canvas.SetLeft(rectanble, rleft);
        }

        public void destroy() {
            canvas.Children.Remove(rectanble);
        }
    }
}

