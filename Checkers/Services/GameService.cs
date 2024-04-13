using Checkers.Helpers;
using Checkers.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace Checkers.Services
{
    public class GameService
    {

        private (int line, int column)? _currentCell, _newCell;

        private bool _currentMultipleJump;

        private GameStatus _gameStatus;

        private BoardManager boardManager;

        private MoveValidator moveValidator;
        public GameService(GameStatus gameStatus)
        {
            _currentCell = null;
            _newCell = null;
            _gameStatus = gameStatus;
            moveValidator = new MoveValidator();
            boardManager = new BoardManager();
        }

        public void Reset()
        {
            _currentCell = null;
            _newCell = null;
            _currentMultipleJump = false;
        }

        public void LoadGame(GameStatus gameStatus)
        {
            _gameStatus = gameStatus;
        }

        public bool IsMoveValid(ObservableCollection<Cell> cells)
        {
            if (_currentCell == null || _newCell == null)
                return false;

            Cell startCell = cells[_currentCell.Value.line * 8 + _currentCell.Value.column];
            Cell endCell = cells[_newCell.Value.line * 8 + _newCell.Value.column];

            return moveValidator.IsMoveValid(_gameStatus, startCell, endCell, _currentMultipleJump);

        }

        private void AssignCurrentCell(Cell cell)
        {
            if (CheckerHelper.GetPlayerTypeFromChecker(cell.Content) == _gameStatus.CurrentPlayer)
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


        public void MovePiece(ObservableCollection<Cell> cells, ref int whiteCheckerNumber, ref int blackCheckerNumber)
        {

            bool possibleMultipleMove = false;

            // se gaseste celula initiala
            var currentCellIndex = _currentCell.Value.line * 8 + _currentCell.Value.column;
            var newCellIndex = _newCell.Value.line * 8 + _newCell.Value.column;

            // functionalitate pentru a captura o piesa
            if (moveValidator.IsJump(_gameStatus, cells[currentCellIndex], cells[newCellIndex]))
            {
                // se gaseste celula intermediara
                boardManager.JumpOverChecker(_gameStatus, currentCellIndex, newCellIndex);
                possibleMultipleMove = true;
            }


            // se inlocuiesc celulele in cells
            boardManager.MoveChecker(_gameStatus, currentCellIndex, newCellIndex);


            // se verifica daca functia a devenit king
            boardManager.CheckForKing(_gameStatus, _gameStatus.Cells[newCellIndex]);
            _currentCell = _newCell;

            if (moveValidator.MultipleJumps(_gameStatus, cells[newCellIndex])  // se verifica daca sunt posibile multiple mutari
                && !_currentMultipleJump // se verifica daca suntem in mijlocul unei sarituri multiple
                && possibleMultipleMove
                && _gameStatus.IsMultiJump)

            // se verifica daca sunt permise multiple mutari din setarile jocului
            // se verifica daca a fost facuta o saritura 
            {
                MessageBox.Show($"Multiple jumps are allowed for {_gameStatus.CurrentPlayer}");
                _newCell = null;
                _currentMultipleJump = true;
            }
            else
            {
                boardManager.ChangePlayer(_gameStatus);
                _currentCell = null;
                _newCell = null;
                _gameStatus.GameStarted = false;
                _currentMultipleJump = false;
            }

        }

        public void GameOver(int whiteCheckerNumber, int blackCheckerNumber)
        {
            if (whiteCheckerNumber == 0 || blackCheckerNumber == 0)
            {
                MessageBox.Show("Game Over");
            }
        }


        public void CellClicked(Cell cell)
        {
            if (cell.IsOccupied && !_currentMultipleJump)
            {
                AssignCurrentCell(cell);
            }
            else
            {
                AssignNewCell(cell);
            }
        }

    }
}
