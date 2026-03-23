using LabiryntMatematyczny.Models;
using LabiryntMatematyczny.Services;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace LabiryntMatematyczny
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer gameLoopTimer;
        private DispatcherTimer levelTimer;
        private Player playerModel;
        private Rectangle playerShape;
        private Rectangle endZoneShape;
        private LevelManager levelManager;
        private QuizManager quizManager;
        private MathQuestion currentQuestion;
        private bool goUp, goDown, goLeft, goRight;
        private int timeLeft;
        private double viewRadius = 150; 

        public MainWindow()
        {
            InitializeComponent();
        }

       
        //menu
        private void BtnResume_Click(object sender, RoutedEventArgs e)
        {
            if (menuGrid.Visibility == Visibility.Hidden && quizPanel.Visibility == Visibility.Hidden)
            {
                gameLoopTimer.Start();
                levelTimer.Start();
                this.Focus(); 
            }
        }

        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            if (gameLoopTimer.IsEnabled) gameLoopTimer.Stop();
            if (levelTimer.IsEnabled) levelTimer.Stop();
        }

        private void BtnRestart_Click(object sender, RoutedEventArgs e)
        {
            if (menuGrid.Visibility == Visibility.Hidden)
            {
                gameLoopTimer.Stop();
                levelTimer.Stop();
                quizPanel.Visibility = Visibility.Hidden;

                levelManager.LoadLevel(levelManager.CurrentLevelIndex);

                gameCanvas.Children.Clear();
                playerShape = null;
                endZoneShape = null;

                DrawLevel();
                    
                StartLevelTimer(); 
                gameLoopTimer.Start(); 
                this.Focus();
            }
        }

        private void BtnToStart_Click(object sender, RoutedEventArgs e)
        {
            // Powrót do startu
            ShowMainMenu("LABIRYNT MATEMATYCZNY");
            btnMenuStart.Content = "GRAJ";
            btnBackToMenu.Visibility = Visibility.Collapsed;
        }

        //menu glowne
          private void BtnStartGame_Click(object sender, RoutedEventArgs e)
        {
            menuGrid.Visibility = Visibility.Hidden;
            this.Focus();
            InitializeGame();
        }

      
        private void ShowMainMenu(string message)
        {
            if (gameLoopTimer != null) gameLoopTimer.Stop();
            if (levelTimer != null) levelTimer.Stop();

            gameCanvas.Children.Clear();
            playerShape = null;
            endZoneShape = null;

            lblMenuTitle.Content = message;
            btnMenuStart.Content = "ZAGRAJ PONOWNIE";
            btnBackToMenu.Visibility = Visibility.Visible;
            menuGrid.Visibility = Visibility.Visible;
            quizPanel.Visibility = Visibility.Hidden;
        }

        //logika gry
        private void InitializeGame()
        {
            levelManager = new LevelManager();
            quizManager = new QuizManager();

            if (!levelManager.LoadLevel(0))
            {
                MessageBox.Show("Błąd wczytywania poziomu!");
                return;
            }
            lblLevel.Content = (levelManager.CurrentLevelIndex + 1).ToString();

            playerModel = new Player((int)levelManager.StartPosition.X, (int)levelManager.StartPosition.Y);
            playerShape = null;

            gameCanvas.Children.Clear();
            DrawLevel();

            // Timer pętli gry (ruch)
            if (gameLoopTimer == null)
            {
                gameLoopTimer = new DispatcherTimer();
                gameLoopTimer.Interval = TimeSpan.FromMilliseconds(16);
                gameLoopTimer.Tick += GameLoopTimer_Tick;
            }
            gameLoopTimer.Start();

            // Timer czasu (sekundy)
            if (levelTimer == null)
            {
                levelTimer = new DispatcherTimer();
                levelTimer.Interval = TimeSpan.FromSeconds(1);
                levelTimer.Tick += LevelTimer_Tick;
            }
            StartLevelTimer();
        }

        private void DrawLevel()
        {
            playerModel.X = (int)levelManager.StartPosition.X;
            playerModel.Y = (int)levelManager.StartPosition.Y;

            //graf
            ImageBrush playerTexture = LoadImage("player.png");
            ImageBrush wallTexture = LoadImage("wall.png");
            ImageBrush metaTexture = LoadImage("meta.png");

            //Rysowanie gracza
            if (playerShape == null)
            {
                playerShape = new Rectangle
                {
                    Width = playerModel.Size,
                    Height = playerModel.Size
                };

                if (playerTexture != null) playerShape.Fill = playerTexture;
                else playerShape.Fill = Brushes.Blue;
            }

            if (!gameCanvas.Children.Contains(playerShape))
            {
                gameCanvas.Children.Add(playerShape);
            }
            Canvas.SetLeft(playerShape, playerModel.X);
            Canvas.SetTop(playerShape, playerModel.Y);

            // walls
            foreach (var wall in levelManager.Walls)
            {
                if (!gameCanvas.Children.Contains(wall.Shape))
                {
                    if (wallTexture != null) wall.Shape.Fill = wallTexture;
                    else wall.Shape.Fill = Brushes.DarkGray;

                    // Mgła, Domyślnie ukryta
                    wall.Shape.Visibility = Visibility.Hidden;

                    Canvas.SetLeft(wall.Shape, wall.Bounds.X);
                    Canvas.SetTop(wall.Shape, wall.Bounds.Y);
                    gameCanvas.Children.Add(wall.Shape);
                }
            }

            //meta
            if (endZoneShape == null || !gameCanvas.Children.Contains(endZoneShape))
            {
                endZoneShape = new Rectangle
                {
                    Width = levelManager.EndZone.Width,
                    Height = levelManager.EndZone.Height,
                    Visibility = Visibility.Hidden
                };

                if (metaTexture != null) endZoneShape.Fill = metaTexture;
                else endZoneShape.Fill = Brushes.Green;

                Canvas.SetLeft(endZoneShape, levelManager.EndZone.X);
                Canvas.SetTop(endZoneShape, levelManager.EndZone.Y);
                gameCanvas.Children.Add(endZoneShape);
            }

            UpdateFogOfWar();
        }

        //mgla
        private void UpdateFogOfWar()
        {
            // Środek gracza
            double playerCenterX = playerModel.X + (playerModel.Size / 2);
            double playerCenterY = playerModel.Y + (playerModel.Size / 2);

            //Sprawdź i odkryj ściany
            foreach (var wall in levelManager.Walls)
            {
                double wallCenterX = wall.Bounds.X + (wall.Bounds.Width / 2);
                double wallCenterY = wall.Bounds.Y + (wall.Bounds.Height / 2);

                double dist = GetDistance(playerCenterX, playerCenterY, wallCenterX, wallCenterY);

                if (dist < viewRadius)
                    wall.Shape.Visibility = Visibility.Visible;
                else
                    wall.Shape.Visibility = Visibility.Hidden;
            }

            //Sprawdź i odkryj metę
            if (endZoneShape != null)
            {
                double endX = levelManager.EndZone.X + (levelManager.EndZone.Width / 2);
                double endY = levelManager.EndZone.Y + (levelManager.EndZone.Height / 2);

                double distEnd = GetDistance(playerCenterX, playerCenterY, endX, endY);

                if (distEnd < viewRadius)
                    endZoneShape.Visibility = Visibility.Visible;
                else
                    endZoneShape.Visibility = Visibility.Hidden;
            }
        }

        private double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

       //petla gry i kolizje
        private void StartLevelTimer()
        {
            timeLeft = 60; 
            lblTimer.Content = "Czas: " + timeLeft;
            levelTimer.Start();
        }

        private void LevelTimer_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            lblTimer.Content = "Czas: " + timeLeft;

            if (timeLeft <= 0)
            {
                ShowMainMenu("KONIEC CZASU!");
            }
        }

        private void GameLoopTimer_Tick(object sender, EventArgs e)
        {
            int oldX = playerModel.X;
            int oldY = playerModel.Y;

            if (goLeft) playerModel.X -= playerModel.Speed;
            if (goRight) playerModel.X += playerModel.Speed;
            if (goUp) playerModel.Y -= playerModel.Speed;
            if (goDown) playerModel.Y += playerModel.Speed;

            Rect playerRect = new Rect(playerModel.X, playerModel.Y, playerModel.Size, playerModel.Size);

            // Kolizje ze ścianami
            foreach (var wall in levelManager.Walls)
            {
                if (playerRect.IntersectsWith(wall.Bounds))
                {
                    playerModel.X = oldX;
                    playerModel.Y = oldY;
                    break;
                }
            }

            // Aktualizacja pozycji na ekranie
            Canvas.SetLeft(playerShape, playerModel.X);
            Canvas.SetTop(playerShape, playerModel.Y);

            // Sprawdzenie mety
            playerRect = new Rect(playerModel.X, playerModel.Y, playerModel.Size, playerModel.Size);
            if (playerRect.IntersectsWith(levelManager.EndZone))
            {
                ShowQuiz();
            }
            UpdateFogOfWar();
        }

       // quiz
        private void ShowQuiz()
        {
            gameLoopTimer.Stop();
            levelTimer.Stop();

            currentQuestion = quizManager.GenerateQuestion(levelManager.CurrentLevelIndex);
            lblQuestion.Content = currentQuestion.QuestionText;
            txtAnswer.Text = "";

            quizPanel.Visibility = Visibility.Visible;

            Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() => {
                txtAnswer.Focus();
            }));
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtAnswer.Text, out int playerAnswer))
            {
                if (playerAnswer == currentQuestion.CorrectAnswer)
                {
                    ProceedToNextLevel();
                }
                else
                {
                    MessageBox.Show("Zła odpowiedź! Spróbuj ponownie.");
                    txtAnswer.Text = "";
                    txtAnswer.Focus();
                }
            }
            else
            {
                MessageBox.Show("Wpisz liczbę!");
            }
        }

        private void ProceedToNextLevel()
        {
            quizPanel.Visibility = Visibility.Hidden;

            if (levelManager.IsLastLevel())
            {
                ShowMainMenu("GRATULACJE! WYGRANA!");
            }
            else
            {
                levelManager.LoadNextLevel();
                lblLevel.Content = (levelManager.CurrentLevelIndex + 1).ToString();
                gameCanvas.Children.Clear();
                playerShape = null;
                endZoneShape = null;

                DrawLevel();

                gameLoopTimer.Start();
                StartLevelTimer();
            }
        }

        //obsluga klawiatury
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W) goUp = true;
            if (e.Key == Key.S) goDown = true;
            if (e.Key == Key.A) goLeft = true;
            if (e.Key == Key.D) goRight = true;

            if (e.Key == Key.Escape)
            {
                if (menuGrid.Visibility == Visibility.Hidden)
                {
                    ShowMainMenu("PAUZA");
                }
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W) goUp = false;
            if (e.Key == Key.S) goDown = false;
            if (e.Key == Key.A) goLeft = false;
            if (e.Key == Key.D) goRight = false;
        }
        private ImageBrush LoadImage(string filename)
        {
            try
            {
                return new ImageBrush(new BitmapImage(new Uri($"pack://application:,,,/Assets/Images/{filename}")));
            }
            catch
            {
                return null;
            }
        }
    }
}