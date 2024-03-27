using System.Collections.ObjectModel;

namespace Checkers.ViewModels
{
    public class BoardViewModel : BaseViewModel
    {
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

        public BoardViewModel()
        {
            Cells = new ObservableCollection<Cell>();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    bool isBlack = (i + j) % 2 == 1;
                    if (i < 3 && isBlack)
                    {
                        Cells.Add(new Cell(isBlack, CheckerTypes.BlackPawn));
                    }
                    else if (i > 4 && isBlack)
                    {
                        Cells.Add(new Cell(isBlack, CheckerTypes.WhitePawn));
                    }
                    else if (i == 3 && j == 3 || i == 4 && j == 4)
                    {
                        Cells.Add(new Cell(isBlack, CheckerTypes.WhiteKing));
                    }
                    else if (i == 3 && j == 4 || i == 4 && j == 3)
                    {
                        Cells.Add(new Cell(isBlack, CheckerTypes.BlackKing));
                    }

                    else
                    {
                        Cells.Add(new Cell(isBlack));
                    }
                }
            }
        }
    }
}
