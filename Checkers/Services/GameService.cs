using Checkers.Helpers;
using Checkers.ViewModels;
using System.Collections.ObjectModel;

namespace Checkers.Services
{
    public class GameService
    {
        private (int line, int column)? _currentCell, _newCell;

        private PlayerType _currentPlayer = PlayerType.White;

        public PlayerType CurrentPlayer
        {
            get { return _currentPlayer; }

            set
            {
                _currentPlayer = value;
            }
        }

        public GameService()
        {
            _currentCell = null;
            _newCell = null;
            _currentPlayer = PlayerType.White;
        }

        public bool IsMoveValid()
        {
            if (_currentCell == null || _newCell == null)
                return false;
            return IsSimpleMoveValid();
        }

        private bool IsSimpleMoveValid()
        {
            return (Math.Abs(_currentCell.Value.line - _newCell.Value.line) == 1 &&
                Math.Abs(_currentCell.Value.column - _newCell.Value.column) == 1);
        }

        private void AssignCurrentCell(Cell cell)
        {
            if (CheckerHelper.GetPlayerTypeFromChecker(cell.Content) == _currentPlayer)
            {
                _currentCell = (cell.Line, cell.Column);
            }
        }

        private void AssignNewCell(Cell cell)
        {
            if (CheckerHelper.GetPlayerTypeFromChecker(cell.Content) == PlayerType.None)
            {
                _newCell = (cell.Line, cell.Column);
            }
        }

        public void AssignCheckerType(IList<Cell> cells, (int line, int column) cell, CheckerTypes checkerType)
        {
            var cellIndex = cell.line * 8 + cell.column;
            cells[cellIndex].Content = checkerType;
        }

        public void MovePiece(ObservableCollection<Cell> cells)
        {

            var currentCellIndex = _currentCell.Value.line * 8 + _currentCell.Value.column;
            var newCellIndex = _newCell.Value.line * 8 + _newCell.Value.column;

            cells[newCellIndex].Content = cells[currentCellIndex].Content;
            cells[newCellIndex].IsOccupied = true;
            cells[currentCellIndex].IsOccupied = false;
            cells[currentCellIndex].Content = CheckerTypes.None;


            CurrentPlayer = (CurrentPlayer == PlayerType.White) ? PlayerType.Black : PlayerType.White;
            _currentCell = null;
            _newCell = null;
        }

        public void CellClicked(Cell cell)
        {
            if (cell.IsOccupied)
            {
                AssignCurrentCell(cell);
            }
            else
            {
                AssignNewCell(cell);
            }
        }


        private bool IsKingMoveValid()
        {
            throw new NotImplementedException();
        }
    }
}
