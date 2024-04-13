using Checkers.Models;
using Checkers.ViewModels;

namespace Checkers.Services
{
    public class BoardManager
    {
        public bool PossibleMultipleMove { get; set; }
        public void MoveChecker(GameStatus gameStatus, int startIndex, int endIndex)
        {
            gameStatus.Cells[endIndex].Content = gameStatus.Cells[startIndex].Content;
            gameStatus.Cells[endIndex].IsOccupied = true;
            gameStatus.Cells[startIndex].IsOccupied = false;
            gameStatus.Cells[startIndex].Content = CheckerTypes.None;
        }

        public void JumpOverChecker(GameStatus gameStatus, int startIndex, int endIndex)
        {
            // se gaseste celula intermediara
            var middleCellIndex = (startIndex + endIndex) / 2; // posibila eroare
            //var middleCellIndex = ((int)(_currentCell.Value.line + _newCell.Value.line) / 2) * 8 + (int)(_currentCell.Value.column + _newCell.Value.column) / 2;

            // se elimina piesa capturata
            gameStatus.Cells[middleCellIndex].Content = CheckerTypes.None;
            gameStatus.Cells[middleCellIndex].IsOccupied = false;

            // se scade numarul de piese
            if (gameStatus.CurrentPlayer == PlayerType.White)
            {
                gameStatus.WhiteCheckers--;
            }
            else
            {
                gameStatus.BlackCheckers--;
            }

        }

        public void ChangePlayer(GameStatus gameStatus)
        {
            gameStatus.CurrentPlayer = (gameStatus.CurrentPlayer == PlayerType.White) ? PlayerType.Black : PlayerType.White;
        }


        public void CheckForKing(GameStatus _gameStatus, Cell newCell)
        {
            if (_gameStatus.CurrentPlayer == PlayerType.White && newCell.Line == 0)
            {
                newCell.Content = CheckerTypes.WhiteKing;
            }
            else if (_gameStatus.CurrentPlayer == PlayerType.Black && newCell.Line == 7)
            {
                newCell.Content = CheckerTypes.BlackKing;
            }
        }
    }
}
