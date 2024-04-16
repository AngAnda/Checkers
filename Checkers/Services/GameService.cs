using Checkers.Helpers;
using Checkers.Models;
using Checkers.Repositories;
using System.Collections.ObjectModel;
using System.Windows;

namespace Checkers.Services
{
    public class GameService
    {

        private Cell CurrentCell, NewCell;

        private bool _currentMultipleJump;

        private GameStatus _gameStatus;

        private BoardManager boardManager;

        private MoveValidator moveValidator;

        private GamesDataRepository _gamesDataRepository;
        public GameService(GameStatus gameStatus)
        {
            CurrentCell = null;
            NewCell = null;
            _gameStatus = gameStatus;
            moveValidator = new MoveValidator();
            boardManager = new BoardManager();
            _gamesDataRepository = new GamesDataRepository();
        }

        public void Reset()
        {
            CurrentCell = null;
            NewCell = null;
            _currentMultipleJump = false;
        }

        public void LoadGame(GameStatus gameStatus)
        {
            _gameStatus = gameStatus;
        }

        public bool IsMoveValid(ObservableCollection<Cell> cells)
        {
            if (CurrentCell == null || NewCell == null)
                return false;

            return moveValidator.IsMoveValid(_gameStatus, CurrentCell, NewCell, _currentMultipleJump);

        }


        public bool AnyMoveValid()
        {
            for (int i = 0; i < _gameStatus.Cells.Count; i++)
            {
                if (_gameStatus.Cells[i].IsOccupied && CheckerHelper.GetPlayerTypeFromChecker(_gameStatus.Cells[i].Content) == _gameStatus.CurrentPlayer)
                {
                    for (int j = 0; j < _gameStatus.Cells.Count; j++)
                    {
                        if (!moveValidator.IsMoveValid(_gameStatus, _gameStatus.Cells[i], _gameStatus.Cells[j], _currentMultipleJump))
                        {
                            continue;
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        private void AssignCurrentCell(Cell cell)
        {
            if (CheckerHelper.GetPlayerTypeFromChecker(cell.Content) == _gameStatus.CurrentPlayer)
            {
                CurrentCell = cell;
            }
        }

        private void AssignNewCell(Cell cell)
        {
            if (CheckerHelper.GetPlayerTypeFromChecker(cell.Content) == PlayerType.None)
            {
                NewCell = cell;
            }
        }


        public void MovePiece(ObservableCollection<Cell> cells)
        {

            bool possibleMultipleMove = false;

            //// se gaseste celula initiala
            var currentCellIndex = CurrentCell.Line * 8 + CurrentCell.Column;
            var newCellIndex = NewCell.Line * 8 + NewCell.Column;

            //// functionalitate pentru a captura o piesa
            if (moveValidator.IsJump(_gameStatus, CurrentCell, NewCell))
            {
                // se gaseste celula intermediara
                boardManager.JumpOverChecker(_gameStatus, currentCellIndex, newCellIndex);
                possibleMultipleMove = true;
            }


            // se inlocuiesc celulele in cells
            boardManager.MoveChecker(_gameStatus, currentCellIndex, newCellIndex);


            // se verifica daca functia a devenit king
            boardManager.CheckForKing(_gameStatus, _gameStatus.Cells[newCellIndex]);
            CurrentCell = NewCell;

            if (moveValidator.MultipleJumps(_gameStatus, cells[newCellIndex])  // se verifica daca sunt posibile multiple mutari
                && !_currentMultipleJump // se verifica daca suntem in mijlocul unei sarituri multiple
                && possibleMultipleMove
                && _gameStatus.IsMultiJump)

            // se verifica daca sunt permise multiple mutari din setarile jocului
            // se verifica daca a fost facuta o saritura 
            {
                MessageBox.Show($"Multiple jumps are allowed for {_gameStatus.CurrentPlayer}");
                NewCell = null;
                _currentMultipleJump = true;
            }
            else
            {
                boardManager.ChangePlayer(_gameStatus);
                CurrentCell = null;
                NewCell = null;
                _gameStatus.GameStarted = false;
                _currentMultipleJump = false;
            }
        }

        public void GameOver()
        {
            bool draught = !AnyMoveValid();
            if (_gameStatus.WhiteCheckers == 0 || _gameStatus.BlackCheckers == 0 || draught)
            {
                PlayerType winner = _gameStatus.WhiteCheckers < _gameStatus.BlackCheckers
                    || (_gameStatus.WhiteCheckers == _gameStatus.BlackCheckers && _gameStatus.CurrentPlayer == PlayerType.White) ?
                    PlayerType.White : PlayerType.Black;
                GameStatistics gameStatistics = new GameStatistics(winner, _gameStatus.WhiteCheckers, _gameStatus.BlackCheckers);
                _gamesDataRepository.SaveGameStatistics(gameStatistics);

                MessageBox.Show($"Game Over\n Winner {winner}");
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
