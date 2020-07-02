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

namespace algLabirint
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Canvas mCanvas = new Canvas();
        //перечисление открыт или закрыт
        enum CellState { Close, Open };
        //класс содержащий ясейку лабиринта
        class Cell
        {
            public Cell(Point currentPosition)
            {
                Visited = false;
                Position = currentPosition;
            }

            public CellState Left { get; set; }
            public CellState Right { get; set; }
            public CellState Bottom { get; set; }
            public CellState Top { get; set; }
            public Boolean Visited { get; set; }
            public Point Position { get; set; }
        }

        //высота и ширина лабиринта
        private Int32 _Width, _Height;
        //двумерный массив лабиринта
        private Cell[,] Cells;

        public MainWindow()
        {
            InitializeComponent();
            myGrid.Children.Add(mCanvas);
        }

        //метод генерирующий лабиринт(нужно переделать)
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            _Width = 10;
            _Height = 10;
            Cells = new Cell[_Width, _Height];

            for (int y = 0; y < _Height; y++)
                for (int x = 0; x < _Width; x++)
                    Cells[x, y] = new Cell(new Point(x, y));

            Random rand = new Random();
            Int32 startX = rand.Next(_Width);
            Int32 startY = rand.Next(_Height);

            Stack<Cell> path = new Stack<Cell>();

            Cells[startX, startY].Visited = true;
            path.Push(Cells[startX, startY]);

            while (path.Count > 0)
            {
                Cell _cell = path.Peek();

                List<Cell> nextStep = new List<Cell>();
                if (_cell.Position.X > 0 && !Cells[Convert.ToInt32(_cell.Position.X - 1), Convert.ToInt32(_cell.Position.Y)].Visited)
                    nextStep.Add(Cells[Convert.ToInt32(_cell.Position.X) - 1, Convert.ToInt32(_cell.Position.Y)]);
                if (_cell.Position.X < _Width - 1 && !Cells[Convert.ToInt32(_cell.Position.X) + 1, Convert.ToInt32(_cell.Position.Y)].Visited)
                    nextStep.Add(Cells[Convert.ToInt32(_cell.Position.X) + 1, Convert.ToInt32(_cell.Position.Y)]);
                if (_cell.Position.Y > 0 && !Cells[Convert.ToInt32(_cell.Position.X), Convert.ToInt32(_cell.Position.Y) - 1].Visited)
                    nextStep.Add(Cells[Convert.ToInt32(_cell.Position.X), Convert.ToInt32(_cell.Position.Y) - 1]);
                if (_cell.Position.Y < _Height - 1 && !Cells[Convert.ToInt32(_cell.Position.X), Convert.ToInt32(_cell.Position.Y) + 1].Visited)
                    nextStep.Add(Cells[Convert.ToInt32(_cell.Position.X), Convert.ToInt32(_cell.Position.Y) + 1]);

                if (nextStep.Count() > 0)
                {
                    Cell next = nextStep[rand.Next(nextStep.Count())];

                    if (next.Position.X != _cell.Position.X)
                    {
                        if (_cell.Position.X - next.Position.X > 0)
                        {
                            _cell.Left = CellState.Open;
                            next.Right = CellState.Open;
                        }
                        else
                        {
                            _cell.Right = CellState.Open;
                            next.Left = CellState.Open;
                        }
                    }
                    if (next.Position.Y != _cell.Position.Y)
                    {
                        if (_cell.Position.Y - next.Position.Y > 0)
                        {
                            _cell.Top = CellState.Open;
                            next.Bottom = CellState.Open;
                        }
                        else
                        {
                            _cell.Bottom = CellState.Open;
                            next.Top = CellState.Open;
                        }
                    }

                    next.Visited = true;
                    path.Push(next);
                }
                else
                {
                    path.Pop();
                }
            }
            //вызываем рендеринг лабиринта
            renderCells();
        }

        //рендеринг лабиринта
        //нужно переделывать
        private void renderCells()
        {
            for (int y = 0; y < _Height; y++)
                for (int x = 0; x < _Width; x++)
                {
                    //в зависимости от того где находится стенка отрисовываем каждую стенку клетки
                    //по координатам
                    if (Cells[x, y].Top == CellState.Close)
                        mCanvas.Children.Add(new Line()
                        {
                            Stroke = Brushes.Black,
                            StrokeThickness = 1,
                            X1 = 20 * x,
                            Y1 = 20 * y,
                            X2 = 20 * x + 20,
                            Y2 = 20 * y
                        });

                    if (Cells[x, y].Left == CellState.Close)
                        mCanvas.Children.Add(new Line()
                        {
                            Stroke = Brushes.Black,
                            StrokeThickness = 1,
                            X1 = 20 * x,
                            Y1 = 20 * y,
                            X2 = 20 * x,
                            Y2 = 20 * y + 20
                        });

                    if (Cells[x, y].Right == CellState.Close)
                        mCanvas.Children.Add(new Line()
                        {
                            Stroke = Brushes.Black,
                            StrokeThickness = 1,
                            X1 = 20 * x + 20,
                            Y1 = 20 * y,
                            X2 = 20 * x + 20,
                            Y2 = 20 * y + 20
                        });

                    if (Cells[x, y].Bottom == CellState.Close)
                        mCanvas.Children.Add(new Line()
                        {
                            Stroke = Brushes.Black,
                            StrokeThickness = 1,
                            X1 = 20 * x,
                            Y1 = 20 * y + 20,
                            X2 = 20 * x + 20,
                            Y2 = 20 * y + 20
                        });
                }
        }
    }
}
