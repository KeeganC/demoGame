/*Keegan Chan and Ethan Shipston
 * 6 6 2018
 * ITTD
 * A multiplayer side shooter game*/
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

namespace ITTD
{
    enum GameState { SplashScreen, GameOn, GameOver }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Global variables
        GameState gameState;
        Background.Maps maps;
        int counterTimer = 0;
        int bulletCounterTimer = 0;
        double playerMomentum = 0;
        double playerMoving = 0;
        double playerMovementX = 0;
        double playerMomentumUp = 0;
        double playerMovingUp = 0;
        double playerMovementY = 0;
        Point lastPos = new Point();
        bool canShoot = true;
        bool facingLeft = true;
        List<Bullet> bullets = new List<Bullet>();


        Point P1Start;
        Player Player = new Player();


        System.Windows.Threading.DispatcherTimer gameTimer = new System.Windows.Threading.DispatcherTimer();
        MediaPlayer musicPlayer = new MediaPlayer();

        public MainWindow()
        {
            InitializeComponent();
            //splash screen
            //canvas.Background = new ImageBrush(new BitmapImage(new Uri("TroonSplash.png", UriKind.Relative)));

            //start music
            //musicPlayer.Open(new Uri("TRON Legacy R3CONF1GUR3D - 06 - C.L.U. (Paul Oakenfold Remix) Daft Punk.mp3", UriKind.Relative));
            //musicPlayer.Play();

            //starts the game timer thingy
            gameTimer.Tick += gameTimer_Tick;
            gameTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / 60);//fps
            gameTimer.Start();
            gameState = GameState.SplashScreen;

            //place character
            P1Start.X = 0;
            P1Start.Y = 0;
            Player.createPlayer(canvas, P1Start, 1);

        }

        public void setupGame()
        {
            Background map = new Background();
            map.drawMap1(canvas);
            maps = ITTD.Background.Maps.Map1;
            gameState = GameState.GameOn;
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            counterTimer++;

            if (gameState == GameState.SplashScreen)
            {
                setupGame();
            }

            if (gameState == GameState.GameOn)
            {
                //slow down player when not moving
                if (counterTimer % 2 == 0)
                {
                    if (playerMomentum < 0)
                    {
                        playerMomentum++;
                    }
                    if (playerMomentum > 0)
                    {
                        playerMomentum--;
                    }

                    if (playerMovementY > 10)
                    {
                        playerMomentumUp--;
                    }
                    else if (playerMomentumUp <= 0)
                    {
                        playerMomentumUp = 0;
                    }
                }
                if (maps == ITTD.Background.Maps.Map1)
                {
                    playerMomentum = Player.addMomentum(playerMomentum);
                    playerMomentumUp = Player.addMomentumUp(playerMomentumUp);

                    playerMoving += playerMomentum;
                    if (playerMovementX < 0) //wall cycle to oposite wall
                    {
                        playerMoving = 800;
                    }
                    if (playerMoving > 800)
                    {
                        playerMoving = 0;
                    }

                    //adjusts player's location based on momentum
                    playerMoving += playerMomentum;
                    playerMovementX = P1Start.X + playerMoving;

                    playerMovingUp += playerMomentumUp;
                    playerMovementY = P1Start.Y + playerMovingUp;
                    if (playerMovementY < 20) //floor collision
                    {
                        playerMovementY = 20;
                        playerMomentumUp = 1;
                    }

                    passThroughPlatform(100, 200, 100, 120);
                    passThroughPlatform(600, 700, 100, 120);
                    solidPlatform(250, 550, 170, 190);

                    lastPos.X = playerMovementX;
                    lastPos.Y = playerMovementY;
                    Player.update(canvas, playerMovementX, playerMovementY);

                   
                }
                //shoot a bullet
                if (canShoot == true)
                {
                    if (Keyboard.IsKeyDown(Key.Enter))
                    {
                        bullets.Add(new Bullet(canvas, facingLeft, playerMovementX, playerMovementY));
                        canShoot = false;
                    }
                }

                //set delay in shots
                if (canShoot == false)
                {
                    bulletCounterTimer++;
                    if (bulletCounterTimer == 60)
                    {
                        canShoot = true;
                        bulletCounterTimer = 0;
                    }
                }
            }

            if (gameState == GameState.GameOver)
            {

            }
        }

        private void passThroughPlatform(int platformLeftSide, int platformRightSide, int platformBottom, int platformTop)
        {
            if (playerMovementX >= platformLeftSide - 30 && playerMovementX <= platformRightSide && playerMovementY > platformBottom && playerMovementY < platformTop) //platform player can move through
            {
                if (playerMomentumUp <= 0)
                {
                    playerMovementY = platformTop;
                    playerMomentumUp = 0;
                    playerMovingUp = platformTop;
                }

            }
        }
        private void solidPlatform(int platformLeftSide, int platformRightSide, int platformBottom, int platformTop)
        {
            if (playerMovementX >= platformLeftSide - 30 &&
                playerMovementX <= platformRightSide &&
                playerMovementY > platformBottom &&
                playerMovementY < platformTop) //platform player can't move through (top)
            {
                if (playerMomentumUp <= 0)
                {
                    playerMovementY = platformTop;
                    playerMomentumUp = 0;
                    playerMovingUp = platformTop;
                }
            }
            if (playerMovementX >= platformLeftSide - 30 &&
                playerMovementX <= platformRightSide &&
                playerMovementY > platformBottom - 35 &&
                playerMovementY < platformTop &&
                lastPos.Y + 35 <= platformBottom) //platform player can't move through (bottom)
            {
                if (playerMomentumUp > 0)
                {
                    playerMovementY = platformBottom - 35;
                    playerMomentumUp = 0;
                    playerMovingUp = platformBottom - 35;
                }
            }
            if (playerMovementX >= platformLeftSide - 30 &&
                playerMovementX <= platformRightSide - 10 &&
                playerMovementY > platformBottom &&
                playerMovementY < platformTop &&
                lastPos.Y + 35 > platformBottom &&
                lastPos.X + 30 > platformLeftSide) //platform player can't move through (left)
            {
                if (playerMomentum > 0)
                {
                    playerMovementX = platformLeftSide - 30;
                    playerMomentum = 0;
                    playerMoving = platformLeftSide - 30;
                }
            }
            if (playerMovementX >= platformLeftSide - 30 &&
                playerMovementX <= platformRightSide - 10 &&
                playerMovementY > platformBottom &&
                playerMovementY < platformTop &&
                lastPos.Y + 35 > platformBottom &&
                lastPos.X + 30 > platformLeftSide) //platform player can't move through (right)
            {
                if (playerMomentum < 0)
                {
                    playerMovementX = platformRightSide;
                    playerMomentum = 0;
                    playerMoving = platformRightSide;
                }
            }
        }
    }
}