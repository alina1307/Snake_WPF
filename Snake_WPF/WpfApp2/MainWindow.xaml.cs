using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        private List<Point> Food = new List<Point>();
        private List<Point> Snake = new List<Point>();
        private Brush snakeColor = Brushes.Gray;
        private Point startingPoint = new Point(300, 200);
        private Point currentPosition = new Point();
        private int direction = 0;
        private int previousDirection = 0;
        private int headSize = 6;
        private int length = 50;
        private int score = 0;
        private Random rnd = new Random();

        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(10000);
            timer.Start();
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
            paintSnake(startingPoint);
            currentPosition = startingPoint;
            for (int n = 0; n < 10; n++)
            {
                GeterateFood(n);
            }
        }

        private void Start()
        {
            Snake.Clear();
            paintCanvas.Children.Clear();
            paintSnake(startingPoint);
            currentPosition = startingPoint;
            for (int n = 0; n < 10; n++)
            {
                GeterateFood(n);
            }
        }

        private void paintSnake(Point currentPosition)
        {
            Ellipse snake = new Ellipse();
            snake.Fill = snakeColor;
            snake.Width = headSize;
            snake.Height = headSize;
            Canvas.SetTop(snake, currentPosition.Y);
            Canvas.SetLeft(snake, currentPosition.X);
            int Count = paintCanvas.Children.Count;
            paintCanvas.Children.Add(snake);
            Snake.Add(currentPosition);
            if (Count > length)
            {
                paintCanvas.Children.RemoveAt(Count - length + 9);
                Snake.RemoveAt(Count - length);
            }
        }

        private void GeterateFood(int index)
        {
            Point bonusPoint = new Point(rnd.Next(5, 620), rnd.Next(5, 380));
            Ellipse food = new Ellipse();
            food.Fill = Brushes.Red;
            food.Width = headSize;
            food.Height = headSize;
            Canvas.SetTop(food, bonusPoint.Y);
            Canvas.SetLeft(food, bonusPoint.X);
            paintCanvas.Children.Insert(index, food);
            Food.Insert(index, bonusPoint);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            switch (direction)
            {
                case (int)Direction.DIRECTION.DOWN:
                    currentPosition.Y ++;
                    break;
                case (int)Direction.DIRECTION.UP:
                    currentPosition.Y --;
                    break;
                case (int)Direction.DIRECTION.LEFT:
                    currentPosition.X --;            
                    break;
                case (int)Direction.DIRECTION.RIGHT:
                    currentPosition.X ++;
                    break;
            }
            paintSnake(currentPosition);
            if ((currentPosition.X < 5) || (currentPosition.X > 620) ||
                (currentPosition.Y < 5) || (currentPosition.Y > 350))
                GameOver();
            int n = 0;
            foreach (Point point in Food)
            {
                if ((Math.Abs(point.X - currentPosition.X) < headSize) &&
                    (Math.Abs(point.Y - currentPosition.Y) < headSize))
                {
                    length += 10;
                    score += 10;
                    Food.RemoveAt(n);
                    paintCanvas.Children.RemoveAt(n);
                    GeterateFood(n);
                    break;
                }
                n++;
            }
            for (int q = 0; q < (Snake.Count - headSize * 2); q++)
            {
                Point point = new Point(Snake[q].X, Snake[q].Y);
                if ((Math.Abs(point.X - currentPosition.X) < (headSize)) &&
                     (Math.Abs(point.Y - currentPosition.Y) < (headSize)))
                {
                    GameOver();
                    break;
                }
            }
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    if (previousDirection != (int)Direction.DIRECTION.UP)
                        direction = (int)Direction.DIRECTION.DOWN;
                    break;
                case Key.Up:
                    if (previousDirection != (int)Direction.DIRECTION.DOWN)
                        direction = (int)Direction.DIRECTION.UP;
                    break;
                case Key.Left:
                    if (previousDirection != (int)Direction.DIRECTION.RIGHT)
                        direction = (int)Direction.DIRECTION.LEFT;
                    break;
                case Key.Right:
                    if (previousDirection != (int)Direction.DIRECTION.LEFT)
                        direction = (int)Direction.DIRECTION.RIGHT;
                    break;
            }
            previousDirection = direction;
        }

        private void GameOver()
        {
            MessageBox.Show("You Lose! Your score is " + score.ToString(), "Game Over", MessageBoxButton.OK);
            this.Close();
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            Start();
        }
    }
}

