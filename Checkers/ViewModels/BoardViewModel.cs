using Checkers.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Checkers.ViewModels
{
    public class BoardViewModel : BaseViewModel
    {
        private readonly GameService _gameService;

        private ObservableCollection<Cell> _cells;

        public ObservableCollection<Cell> Cells
        {
            get { return _cells; }

            set
            {
                _cells = value;
                OnPropertyChanged(nameof(Cells));
            }
        }

        public PlayerType CurrentPlayer
        {
            get { return _gameService.CurrentPlayer; }

            set
            {
                if (_gameService.CurrentPlayer != value)
                {
                    _gameService.CurrentPlayer = value;
                    OnPropertyChanged(nameof(CurrentPlayer));
                }
            }
        }

        public BoardViewModel()
        {
            _gameService = new GameService();

            Cells = new ObservableCollection<Cell>();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    bool isBlack = (i + j) % 2 == 1;
                    if (i < 3 && isBlack)
                    {
                        Cells.Add(new Cell(isBlack, i, j, CheckerTypes.BlackPawn));
                    }
                    else if (i > 4 && isBlack)
                    {
                        Cells.Add(new Cell(isBlack, i, j, CheckerTypes.WhitePawn));
                    }
                    else if (i == 3 && j == 3 || i == 4 && j == 4)
                    {
                        Cells.Add(new Cell(isBlack, i, j, CheckerTypes.WhiteKing));
                    }
                    else if (i == 3 && j == 4 || i == 4 && j == 3)
                    {
                        Cells.Add(new Cell(isBlack, i, j, CheckerTypes.BlackKing));
                    }

                    else
                    {
                        Cells.Add(new Cell(isBlack, i, j));
                    }
                }
            }

            ClickCellCommand = new RelayCommand(ClickCell);
            MovePieceCommand = new RelayCommand(MovePiece, isMoveValid);
        }


        public ICommand ClickCellCommand { get; set; }

        public ICommand MovePieceCommand { get; set; }

        public void ClickCell(object parameter)
        {

            var cell = parameter as Cell;

            if (cell != null)
            {
                _gameService.CellClicked(cell);
            }
        }

        public void MovePiece(object parameter)
        {

            _gameService.MovePiece(Cells);
        }

        public bool isMoveValid(object parameter)
        {
            return _gameService.IsMoveValid(Cells);
        }

    }
}
