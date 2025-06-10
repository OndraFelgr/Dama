using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Dama
{
    public partial class MainWindow : Window
    {
        private const int Size = 8;
        private Button[,] board = new Button[Size, Size];
        private bool isWhiteTurn = true;

        public MainWindow()
        {
            InitializeComponent();
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            for (int i = 0; i < Size; i++)
            {
                BoardGrid.RowDefinitions.Add(new RowDefinition());
                BoardGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    var btn = new Button();
                    btn.Click += Piece_Click;
                    btn.Tag = new Point(row, col);

                    btn.Background = (row + col) % 2 == 0 ? Brushes.BurlyWood : Brushes.SaddleBrown;

                    if ((row < 3) && (row + col) % 2 != 0)
                    {
                        btn.Content = CreatePiece(Brushes.Black);
                    }
                    else if ((row > 4) && (row + col) % 2 != 0)
                    {
                        btn.Content = CreatePiece(Brushes.White);
                    }

                    Grid.SetRow(btn, row);
                    Grid.SetColumn(btn, col);

                    board[row, col] = btn;
                    BoardGrid.Children.Add(btn);
                }
            }
        }

        private Ellipse CreatePiece(Brush color)
        {
            return new Ellipse
            {
                Width = 40,
                Height = 40,
                Fill = color
            };
        }

        private Button selectedButton = null;

        private void Piece_Click(object sender, RoutedEventArgs e)
        {
            var clicked = sender as Button;
            if (clicked == null) return;

            var point = (Point)clicked.Tag;
            var row = (int)point.X;
            var col = (int)point.Y;

            var piece = clicked.Content as Ellipse;

            if (selectedButton == null)
            {
                if (piece != null)
                {
                    var color = piece.Fill;
                    if ((isWhiteTurn && color == Brushes.White) || (!isWhiteTurn && color == Brushes.Black))
                    {
                        selectedButton = clicked;
                        clicked.BorderBrush = Brushes.Red;
                        clicked.BorderThickness = new Thickness(3);
                    }
                }
            }
            else
            {
                var from = (Point)selectedButton.Tag;
                var to = point;

                if (IsValidMove(from, to))
                {
                    bool wasJump = Math.Abs(to.X - from.X) == 2;

                    if (wasJump)
                    {
                        int midX = (int)(from.X + (to.X - from.X) / 2);
                        int midY = (int)(from.Y + (to.Y - from.Y) / 2);
                        board[midX, midY].Content = null;
                    }

                    clicked.Content = selectedButton.Content;
                    selectedButton.Content = null;
                    selectedButton.BorderThickness = new Thickness(1);
                    selectedButton = null;

                    if (wasJump && CanJumpAgain(to))
                    {
                        selectedButton = board[(int)to.X, (int)to.Y];
                        selectedButton.BorderBrush = Brushes.Red;
                        selectedButton.BorderThickness = new Thickness(3);
                    }
                    else
                    {
                        isWhiteTurn = !isWhiteTurn;
                    }
                }
                else
                {
                    selectedButton.BorderThickness = new Thickness(1);
                    selectedButton = null;
                }
            }
        }

        private bool IsValidMove(Point from, Point to)
        {
            int dx = (int)(to.X - from.X);
            int dy = (int)(to.Y - from.Y);

            if (to.X < 0 || to.X >= Size || to.Y < 0 || to.Y >= Size)
                return false;

            if (board[(int)to.X, (int)to.Y].Content != null)
                return false;

            if (isWhiteTurn && dx == -1 && Math.Abs(dy) == 1)
                return true;

            if (!isWhiteTurn && dx == 1 && Math.Abs(dy) == 1)
                return true;

            
            if (Math.Abs(dx) == 2 && Math.Abs(dy) == 2)
            {
                int midX = (int)(from.X + dx / 2);
                int midY = (int)(from.Y + dy / 2);

                var middlePiece = board[midX, midY].Content as Ellipse;
                var fromPiece = board[(int)from.X, (int)from.Y].Content as Ellipse;

                if (middlePiece != null && fromPiece != null)
                {
                    if (middlePiece.Fill != fromPiece.Fill)
                        return true;
                }
            }

            return false;
        }

        private bool CanJumpAgain(Point from)
        {
            int[,] directions = { { -2, -2 }, { -2, 2 }, { 2, -2 }, { 2, 2 } };

            var fromPiece = board[(int)from.X, (int)from.Y].Content as Ellipse;
            if (fromPiece == null) return false;
            var fromColor = fromPiece.Fill;

            for (int i = 0; i < directions.GetLength(0); i++)
            {
                int dx = directions[i, 0];
                int dy = directions[i, 1];

                int toX = (int)from.X + dx;
                int toY = (int)from.Y + dy;

                if (toX < 0 || toX >= Size || toY < 0 || toY >= Size)
                    continue;

                int midX = (int)from.X + dx / 2;
                int midY = (int)from.Y + dy / 2;

                var middlePiece = board[midX, midY].Content as Ellipse;
                var target = board[toX, toY].Content;

                if (target == null && middlePiece != null && middlePiece.Fill != fromColor)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
