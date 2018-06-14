using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ITTD
{
    class Bullet
    {
        double bulletX;
        bool canShoot = true;
        int counterTimer;
        public Rectangle bullet;

        public Bullet(Canvas c, bool facingLeft, double gunBarrelX, double gunBarrelY)
        {
            //create bullet
            bullet = new Rectangle();
            bullet.Width = 5;
            bullet.Height = 5;
            bullet.Fill = Brushes.Green;
            //check which side of player to spawn the bullet on
            if (facingLeft == false)
            {
                bulletX = gunBarrelX + 35;
            }
            if (facingLeft == true)
            {
                bulletX = gunBarrelX - 5;
            }
            c.Children.Add(bullet);
            Canvas.SetLeft(bullet, bulletX);
            Canvas.SetBottom(bullet, gunBarrelY + 15);
        }
    }
}
