using Checkers.Helpers;
using Checkers.Models;
using Checkers.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;

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

        public GameService(bool allowMultiJump = false)
        {
            _currentCell = null;
            _newCell = null;
            _currentPlayer = PlayerType.White;

        }

        public bool IsMoveValid(ObservableCollection<Cell> cells)
        {
            if (_currentCell == null || _newCell == null)
                return false;

            if (cells[_currentCell.Value.line * 8 + _currentCell.Value.column].Content == CheckerTypes.WhiteKing ||
                               cells[_currentCell.Value.line * 8 + _currentCell.Value.column].Content == CheckerTypes.BlackKing)
            {
                return IsKingMoveValid();
            }

            return IsJumpMoveValid(cells) || IsSimpleMoveValid();
        }

        /// <summary>Determines whether [is jump move valid] [the specified cells].</summary>
        /// <param name="cells">The cells.</param>
        /// <returns>
        ///   <c>true</c> if [is jump move valid] [the specified cells]; otherwise, <c>false</c>.</returns>
        private bool IsJumpMoveValid(ObservableCollection<Cell> cells)
        {
            bool isMovingTwoSpaces = Math.Abs(_currentCell.Value.line - _newCell.Value.line) == 2 &&
                                     Math.Abs(_currentCell.Value.column - _newCell.Value.column) == 2;

            int middleCellLine = (_currentCell.Value.line + _newCell.Value.line) / 2;
            int middleCellColumn = (_currentCell.Value.column + _newCell.Value.column) / 2;

            int middleCellIndex = middleCellLine * 8 + middleCellColumn;

            bool isOpponentPieceInBetween =
                CheckerHelper.GetPlayerTypeFromChecker(cells[middleCellIndex].Content) != _currentPlayer;

            return isMovingTwoSpaces && isOpponentPieceInBetween;

        }

        private bool IsSimpleMoveValid()
        {

            int rowChange = _newCell.Value.line - _currentCell.Value.line;
            int isWhite = (_currentPlayer == PlayerType.White) ? 1 : -1;

            bool isDiagonalMove = Math.Abs(_currentCell.Value.line - _newCell.Value.line) == 1 &&
                                  Math.Abs(_currentCell.Value.column - _newCell.Value.column) == 1;

            bool isForwardMove = rowChange == -isWhite;

            return isDiagonalMove && isForwardMove;
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

        public void MovePiece(ObservableCollection<Cell> cells, ref int whiteCheckerNumber, ref int blackCheckerNumber)
        {

            var currentCellIndex = _currentCell.Value.line * 8 + _currentCell.Value.column;
            var newCellIndex = _newCell.Value.line * 8 + _newCell.Value.column;

            cells[newCellIndex].Content = cells[currentCellIndex].Content;
            cells[newCellIndex].IsOccupied = true;
            cells[currentCellIndex].IsOccupied = false;
            cells[currentCellIndex].Content = CheckerTypes.None;

            if (Math.Abs(_currentCell.Value.line - _newCell.Value.line) == 2)
            {
                var middleCellIndex = ((int)(_currentCell.Value.line + _newCell.Value.line) / 2) * 8 + (int)(_currentCell.Value.column + _newCell.Value.column) / 2;
                cells[middleCellIndex].Content = CheckerTypes.None;
                cells[middleCellIndex].IsOccupied = false;
                if (_currentPlayer == PlayerType.White)
                {
                    whiteCheckerNumber--;
                }
                else
                {
                    blackCheckerNumber--;
                }
            }
            CheckForKing(cells, newCellIndex);

            CurrentPlayer = (CurrentPlayer == PlayerType.White) ? PlayerType.Black : PlayerType.White;

            _currentCell = null;

            _newCell = null;
        }

        public void GameOver(int whiteCheckerNumber, int blackCheckerNumber)
        {
            if (whiteCheckerNumber == 0 || blackCheckerNumber == 0)
            {
                MessageBox.Show("Game Over");
            }
        }

        private void CheckForKing(ObservableCollection<Cell> cells, int newIndex)
        {
            if (_currentPlayer == PlayerType.White && _newCell.Value.line == 0)
            {
                cells[newIndex].Content = CheckerTypes.WhiteKing;
            }
            else if (_currentPlayer == PlayerType.Black && _newCell.Value.line == 7)
            {
                AssignCheckerType(cells, _newCell.Value, CheckerTypes.BlackKing);
                cells[newIndex].Content = CheckerTypes.BlackKing;
            }
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

            var rowChange = Math.Abs(_currentCell.Value.line - _newCell.Value.line);
            var columnChange = Math.Abs(_currentCell.Value.column - _newCell.Value.column);
            return (rowChange == 1 && columnChange == 1);
        }
    }
}
