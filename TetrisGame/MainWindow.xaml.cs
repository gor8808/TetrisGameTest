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
using System.Windows.Threading;
namespace TetrisGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int GAMESPEED = 700;// millisecond
        //List<System.Media.SoundPlayer> soundList=new List<System.Media.SoundPlayer>();
        private DispatcherTimer _timer;
        private Random _r;
        private int _rowCount = 0;
        private int _columnCount = 0;
        private int _leftPos = 0;
        private int _downPos = 0;
        private int _currentTetrisItemWidth;
        private int _currentTetrisItemHeigth;
        private int _currentShapeNumber;
        private int _nextShapeNumber;
        private int _tetrisGridColumn;
        private int _tetrisGridRow;
        private int _rotationAngle = 0;
        private bool _isGameActive = false;
        private bool _isNextShapeDrawed = false;
        private int[,] currentTetrisItem = null;
        private bool _isRotated = false;
        private bool _isBottomCollided = false;
        private bool _isLeftCollided = false;
        private bool _isRightCollided = false;
        private bool _isGameOver = false;
        private int _gameSpeed;
        private int _levelScale = 60; // every 60 second level -= 10 
        private double _gameSpeedCounter = 0;
        private int _gameLevel = 1;
        private int _gameScore = 0;
        
        private List<int> _currentTetrisItemRow = null;
        private List<int> _currentTetrisItemColumn = null;

        public MainWindow()
        {
            InitializeComponent();
            _gameSpeed = GAMESPEED;
            //created event for key press
            KeyDown += MainWindow_KeyDown;
            // init timer
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 0, 0, _gameSpeed); // 700 millisecond
            _timer.Tick += Timer_Tick;
            _tetrisGridColumn = tetrisGrid.ColumnDefinitions.Count;
            _tetrisGridRow = tetrisGrid.RowDefinitions.Count;
            DrawGrid();
            _r = new Random();
            _currentShapeNumber = _r.Next(1, 8);
            _nextShapeNumber = _r.Next(1, 8);
            nextTxt.Visibility = levelTxt.Visibility = GameOverTxt.Visibility = Visibility.Collapsed;

        }

        private void DrawGrid()
        {
            for (int i = 0; i < _tetrisGridRow; i++)
            {
                for (int j = 0; j < _tetrisGridColumn; j++)
                {
                    Rectangle rect = new Rectangle();
                    if ((j % 2 == 0 && i % 2 == 0) || (j % 2 != 0 && i % 2 != 0))
                    {
                        rect.Fill = new SolidColorBrush(Color.FromArgb(100,48,48,48));
                    }
                    else
                    {
                        rect.Fill = new SolidColorBrush(Color.FromArgb(100, 78, 75, 75));
                    }
                    GridToDraw.Children.Add(rect);
                    rect.SetValue(Grid.RowProperty, i);
                    rect.SetValue(Grid.ColumnProperty, j);
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _downPos++;
            MoveShape();
            if (_gameSpeedCounter >= _levelScale)
            {
                if (_gameSpeed >= 50)
                {
                    _gameSpeed -= 50;
                    _gameLevel++;
                    levelTxt.Text = "Level: " + _gameLevel.ToString();
                }
                else { _gameSpeed = 50; }
                _timer.Stop();
                _timer.Interval = new TimeSpan(0, 0, 0, 0, _gameSpeed);
                _timer.Start();
                _gameSpeedCounter = 0;
            }
            _gameSpeedCounter += (_gameSpeed / 1000f);
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (_isGameOver)
            {
                tetrisGrid.Children.Clear();
                nextShapeCanvas.Children.Clear();
                GameOverTxt.Visibility = Visibility.Collapsed;
                _isGameOver = false;
            }
            if (!_timer.IsEnabled)
            {
                if (!_isGameActive) { scoreTxt.Text = "0"; _leftPos = 3; AddShape(_currentShapeNumber, _leftPos); }
                nextTxt.Visibility = levelTxt.Visibility = Visibility.Visible;
                levelTxt.Text = "Level: " + _gameLevel.ToString();
                _timer.Start();
                startStopBtn.Content = "Stop Game";
                _isGameActive = true;
            }
            else
            {
                _timer.Stop();
                startStopBtn.Content = "Start Game";
            }
        }
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (!_timer.IsEnabled) { return; }
            switch (e.Key.ToString())
            {
                case "Up":
                    _rotationAngle += 90;
                    if (_rotationAngle > 270) { _rotationAngle = 0; }
                    SapeRotation(_rotationAngle);
                    break;
                case "Down":
                    _downPos += 2;
                    break;
                case "Right":
                    TetroCollided();
                    if (!_isRightCollided) { _leftPos++; }
                    _isRightCollided = false;
                    break;
                case "Left":
                    TetroCollided();
                    if (!_isLeftCollided) { _leftPos--; }
                    _isLeftCollided = false;
                    break;
            }
            MoveShape();
        }
        private void MoveShape()
        {
            _isLeftCollided = false;
            _isRightCollided = false;

            TetroCollided();
            if (_leftPos > (_tetrisGridColumn - _currentTetrisItemWidth))
            {
                _leftPos = (_tetrisGridColumn - _currentTetrisItemWidth);
            }
            else if (_leftPos < 0) { _leftPos = 0; }

            if (_isBottomCollided)
            {
                ShapeStoped();
                return;
            }
            AddShape(_currentShapeNumber, _leftPos, _downPos);
        }
        private void ShapeStoped()
        {
            _timer.Stop();
            if (_downPos <= 2)
            {
                GameOver();
                return;
            }

            int index = 0;
            while (index < tetrisGrid.Children.Count)
            {
                UIElement element = tetrisGrid.Children[index];
                if (element is Rectangle)
                {
                    Rectangle square = (Rectangle)element;
                    if (square.Name.IndexOf("moving_") == 0)
                    {
                        string newName = square.Name.Replace("moving_", "arrived_");
                        square.Name = newName;
                    }
                }
                index++;
            }
            CheckComplete();
            Reset();
            _timer.Start();

        }
        private void Reset()
        {
            _downPos = 0;
            _leftPos = 3;
            _isRotated = false;
            _rotationAngle = 0;
            _currentShapeNumber = _nextShapeNumber;
            if (!_isGameOver) { AddShape(_currentShapeNumber, _leftPos); }
            _isNextShapeDrawed = false;
            _r = new Random();
            _nextShapeNumber = _r.Next(1, 8);
            _isBottomCollided = false;
            _isLeftCollided = false;
            _isRightCollided = false;
        }
        private void GameOver()
        {
            _isGameOver = true;
            Reset();
            startStopBtn.Content = "Start Game";
            GameOverTxt.Visibility = Visibility.Visible;
            _rowCount = 0;
            _columnCount = 0;
            _leftPos = 0;
            _gameSpeedCounter = 0;
            _gameSpeed = GAMESPEED;
            _gameLevel = 1;
            _isGameActive = false;
            _gameScore = 0;
            _isNextShapeDrawed = false;
            currentTetrisItem = null;
            _currentShapeNumber = _r.Next(1, 8);
            _nextShapeNumber = _r.Next(1, 8);
            _timer.Interval = new TimeSpan(0, 0, 0, 0, _gameSpeed);

        }
        private void CheckComplete()
        {
            int gridRow = tetrisGrid.RowDefinitions.Count;
            int gridColumn = tetrisGrid.ColumnDefinitions.Count;
            int squareCount = 0;
            for (int row = gridRow; row >= 0; row--)
            {
                squareCount = 0;
                for (int column = gridColumn; column >= 0; column--)
                {
                    Rectangle square;
                    square = (Rectangle)tetrisGrid.Children
                   .Cast<UIElement>()
                   .FirstOrDefault(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column);
                    if (square != null)
                    {
                        if (square.Name.IndexOf("arrived") == 0)
                        {
                            squareCount++;
                        }
                    }
                }

                if (squareCount == gridColumn)
                {
                    DeleteLine(row);
                    scoreTxt.Text = GetScore().ToString();
                    CheckComplete();
                }
            }
        }
        private void DeleteLine(int row)
        {
            for (int i = 0; i < tetrisGrid.ColumnDefinitions.Count; i++)
            {
                Rectangle square;
                try
                {
                    square = (Rectangle)tetrisGrid.Children
                   .Cast<UIElement>()
                   .FirstOrDefault(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == i);
                    tetrisGrid.Children.Remove(square);
                }
                catch { }

            }
            foreach (UIElement element in tetrisGrid.Children)
            {
                Rectangle square = (Rectangle)element;
                if (square.Name.IndexOf("arrived") == 0 && Grid.GetRow(square) <= row)
                {
                    Grid.SetRow(square, Grid.GetRow(square) + 1);
                }
            }
        }
        private int GetScore()
        {
            _gameScore += 50 * _gameLevel;
            return _gameScore;
        }
        private void TetroCollided()
        {
            _isBottomCollided = CheckCollided(0, 1);
            _isLeftCollided = CheckCollided(-1, 0);
            _isRightCollided = CheckCollided(1, 0);
        }
        private bool CheckCollided(int _leftRightOffset, int _bottomOffset)
        {
            Rectangle movingSquare;
            int squareRow = 0;
            int squareColumn = 0;
            for (int i = 0; i <= 3; i++)
            {
                squareRow = _currentTetrisItemRow[i];
                squareColumn = _currentTetrisItemColumn[i];
                try
                {
                    movingSquare = (Rectangle)tetrisGrid.Children
                    .Cast<UIElement>()
                    .FirstOrDefault(e => Grid.GetRow(e) == squareRow + _bottomOffset && Grid.GetColumn(e) == squareColumn + _leftRightOffset);
                    if (movingSquare != null)
                    {
                        if (movingSquare.Name.IndexOf("arrived") == 0)
                        {
                            return true;
                        }
                    }
                }
                catch { }
            }
            if (_downPos > (_tetrisGridRow - _currentTetrisItemHeigth)) { return true; }
            return false;
        }

        private void SapeRotation(int _rotation)
        {
            if (RotationCollided(_rotationAngle))
            {
                _rotationAngle -= 90;
                return;
            }

            if (Shapes.ArrayTetrisItems[_currentShapeNumber].IndexOf("I_") == 0)
            {
                if (_rotation > 90) { _rotation = _rotationAngle = 0; }
                currentTetrisItem = GetVariableByString("I_Tetromino_" + _rotation);
            }
            else if (Shapes.ArrayTetrisItems[_currentShapeNumber].IndexOf("T_") == 0)
            {
                currentTetrisItem = GetVariableByString("T_Tetromino_" + _rotation);
            }
            else if (Shapes.ArrayTetrisItems[_currentShapeNumber].IndexOf("S_") == 0)
            {
                if (_rotation > 90) { _rotation = _rotationAngle = 0; }
                currentTetrisItem = GetVariableByString("S_Tetromino_" + _rotation);
            }
            else if (Shapes.ArrayTetrisItems[_currentShapeNumber].IndexOf("Z_") == 0)
            {
                if (_rotation > 90) { _rotation = _rotationAngle = 0; }
                currentTetrisItem = GetVariableByString("Z_Tetromino_" + _rotation);
            }
            else if (Shapes.ArrayTetrisItems[_currentShapeNumber].IndexOf("J_") == 0)
            {
                currentTetrisItem = GetVariableByString("J_Tetromino_" + _rotation);
            }
            else if (Shapes.ArrayTetrisItems[_currentShapeNumber].IndexOf("L_") == 0)
            {
                currentTetrisItem = GetVariableByString("L_Tetromino_" + _rotation);
            }
            else if (Shapes.ArrayTetrisItems[_currentShapeNumber].IndexOf("O_") == 0) // Do not rotate this
            {
                return;
            }

            _isRotated = true;
            AddShape(_currentShapeNumber, _leftPos, _downPos);
        }
        private void AddShape(int shapeNumber, int _left = 0, int _down = 0)
        {
            RemoveShape();
            _currentTetrisItemRow = new List<int>();
            _currentTetrisItemColumn = new List<int>();
            Rectangle square = null;
            if (!_isRotated)
            {
                currentTetrisItem = null;
                currentTetrisItem = GetVariableByString(Shapes.ArrayTetrisItems[shapeNumber].ToString());
            }
            int firstDim = currentTetrisItem.GetLength(0);
            int secondDim = currentTetrisItem.GetLength(1);
            _currentTetrisItemWidth = secondDim;
            _currentTetrisItemHeigth = firstDim;
            if (currentTetrisItem == Shapes.I_Tetromino_90)
            {
                _currentTetrisItemWidth = 1;
            }
            else if (currentTetrisItem == Shapes.I_Tetromino_0) { _currentTetrisItemHeigth = 1; }
            for (int row = 0; row < firstDim; row++)
            {
                for (int column = 0; column < secondDim; column++)
                {
                    int bit = currentTetrisItem[row, column];
                    if (bit == 1)
                    {
                        square = GetBasicSquare(Shapes.ShapeColor[shapeNumber - 1]);
                        tetrisGrid.Children.Add(square);
                        square.Name = "moving_" + Grid.GetRow(square) + "_" + Grid.GetColumn(square);
                        if (_down >= tetrisGrid.RowDefinitions.Count - _currentTetrisItemHeigth)
                        {
                            _down = tetrisGrid.RowDefinitions.Count - _currentTetrisItemHeigth;
                        }
                        Grid.SetRow(square, _rowCount + _down);
                        Grid.SetColumn(square, _columnCount + _left);
                        _currentTetrisItemRow.Add(_rowCount + _down);
                        _currentTetrisItemColumn.Add(_columnCount + _left);

                    }
                    _columnCount++;
                }
                _columnCount = 0;
                _rowCount++;
            }
            _columnCount = 0;
            _rowCount = 0;
            if (!_isNextShapeDrawed)
            {
                DrawNextShape(_nextShapeNumber);
            }
        }
        private void RemoveShape()
        {
            int index = 0;
            while (index < tetrisGrid.Children.Count)
            {
                UIElement element = tetrisGrid.Children[index];
                if (element is Rectangle)
                {
                    Rectangle square = (Rectangle)element;
                    if (square.Name.IndexOf("moving_") == 0)
                    {
                        tetrisGrid.Children.Remove(element);
                        index = -1;
                    }
                }
                index++;
            }

        }
        private void DrawNextShape(int shapeNumber)
        {
            nextShapeCanvas.Children.Clear();
            int[,] nextShapeTetromino = null;
            nextShapeTetromino = GetVariableByString(Shapes.ArrayTetrisItems[shapeNumber]);
            int firstDim = nextShapeTetromino.GetLength(0);
            int secondDim = nextShapeTetromino.GetLength(1);
            int x = 0;
            int y = 0;
            Rectangle square;
            for (int row = 0; row < firstDim; row++)
            {
                for (int column = 0; column < secondDim; column++)
                {
                    int bit = nextShapeTetromino[row, column];
                    if (bit == 1)
                    {
                        square = GetBasicSquare(Shapes.ShapeColor[shapeNumber - 1]);
                        nextShapeCanvas.Children.Add(square);
                        Canvas.SetLeft(square, x);
                        Canvas.SetTop(square, y);
                    }
                    x += 25;
                }
                x = 0;
                y += 25;
            }
            _isNextShapeDrawed = true;
        }
        private bool RotationCollided(int _rotation)
        {
            if (CheckCollided(0, _currentTetrisItemWidth - 1)) { return true; }//Bottom  
            else if (CheckCollided(0, -(_currentTetrisItemWidth - 1))) { return true; }// Top 
            else if (CheckCollided(0, -1)) { return true; }// Top 
            else if (CheckCollided(-1, _currentTetrisItemWidth - 1)) { return true; }// Left 
            else if (CheckCollided(1, _currentTetrisItemWidth - 1)) { return true; }// Right 
            return false;
        }
        private int[,] GetVariableByString(string variable)
        {
            return (int[,])typeof(Shapes).GetField(variable).GetValue(this);
        }
        private Rectangle GetBasicSquare(Color rectColor)
        {
            Rectangle rectangle = new Rectangle();
            //rectangle.Width = 25;
            //rectangle.Height = 25;
            rectangle.StrokeThickness = 1;
            rectangle.Stroke = Brushes.White;
            rectangle.Fill = GetGradientColor(rectColor);
            return rectangle;
        }
        private LinearGradientBrush GetGradientColor(Color clr)
        {
            LinearGradientBrush gradientColor = new LinearGradientBrush();
            gradientColor.StartPoint = new Point(0, 0);
            gradientColor.EndPoint = new Point(1, 1.5);
            GradientStop black = new GradientStop();
            black.Color = Colors.Black;
            black.Offset = -1.5;
            gradientColor.GradientStops.Add(black);
            GradientStop other = new GradientStop();
            other.Color = clr;
            other.Offset = 0.70;
            gradientColor.GradientStops.Add(other);
            return gradientColor;
        }

        
    }
}
