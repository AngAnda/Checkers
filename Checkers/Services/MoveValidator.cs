using Checkers.Helpers;
using Checkers.Models;
using Checkers.ViewModels;

namespace Checkers.Services
{
    public class MoveValidator
    {
        public bool IsMoveValid(GameStatus gameStatus, Cell startCell, Cell endCell, bool isMultiJump)
        {
            if (startCell == null || endCell == null)
                return false;

            return IsJumpMoveValid(gameStatus, startCell, endCell)
                || IsSimpleMoveValid(gameStatus, startCell, endCell)
                || IsKingMoveValid(gameStatus, startCell, endCell)
                || IsKingJumpValid(gameStatus, startCell, endCell)
                ;
        }

        public bool IsJump(GameStatus gameStatus, Cell startCell, Cell endCell)
        {
            return IsJumpMoveValid(gameStatus, startCell, endCell) || IsKingJumpValid(gameStatus, startCell, endCell);
        }

        public bool MultipleJumps(GameStatus gameStatus, Cell startCell)
        {
            // check if there are any multi jumps to capture 
            int coeficient = (gameStatus.CurrentPlayer == PlayerType.White) ? -1 : 1;

            // trebuie verificat daca miscarile sunt valide inainte de a fi introduse in 
            int option1 = (startCell.Line + coeficient * 2) * 8 + startCell.Column + 2;
            int option2 = ((startCell.Line + coeficient * 2) * 8 + startCell.Column - 2);

            if (option1 > 0 && option1 < 63 && IsJumpMoveValid(gameStatus, startCell, gameStatus.Cells[option1]))
                return true;

            if (option2 > 0 && option1 < 63 && IsJumpMoveValid(gameStatus, startCell, gameStatus.Cells[option2]))
                return true;

            // For king
            if (startCell.Content == CheckerTypes.WhiteKing || startCell.Content == CheckerTypes.BlackKing)
            {
                int option3 = (startCell.Line - coeficient * 2) * 8 + startCell.Column - 2;
                int option4 = (startCell.Line - coeficient * 2) * 8 + startCell.Column - 2;
                if (option1 > 0 && option3 < 63 && IsJumpMoveValid(gameStatus, startCell, gameStatus.Cells[option3]))
                    return true;

                if (option2 > 0 && option4 < 63 && IsJumpMoveValid(gameStatus, startCell, gameStatus.Cells[option4]))
                    return true;

            }

            return false;
        }

        private bool IsJumpMoveValid(GameStatus gameStatus, Cell startCell, Cell endCell)
        {

            if (startCell.Line < 0
                || startCell.Line > 7
                || endCell.Column < 0
                || startCell.Column > 7
                || gameStatus.Cells[endCell.Line * 8 + endCell.Column].Content != CheckerTypes.None
                )
                return false;

            bool isMovingTwoSpaces = Math.Abs(startCell.Line - endCell.Line) == 2 &&
                                     Math.Abs(startCell.Column - endCell.Column) == 2;

            int middleCellLine = (startCell.Line + endCell.Line) / 2;
            int middleCellColumn = (startCell.Column + endCell.Column) / 2;

            int middleCellIndex = middleCellLine * 8 + middleCellColumn;

            bool isOpponentPieceInBetween =
                CheckerHelper.GetPlayerTypeFromChecker(gameStatus.Cells[middleCellIndex].Content) != gameStatus.CurrentPlayer
                && CheckerHelper.GetPlayerTypeFromChecker(gameStatus.Cells[middleCellIndex].Content) != PlayerType.None;

            int rowChange = (-startCell.Line + endCell.Line) / 2;

            int isWhite = (gameStatus.CurrentPlayer == PlayerType.White) ? 1 : -1;

            bool isForward = rowChange == -isWhite;

            return isMovingTwoSpaces && isOpponentPieceInBetween && isForward;

        }

        private bool IsSimpleMoveValid(GameStatus gameStatus, Cell startCell, Cell endCell)
        {

            int rowChange = startCell.Line - endCell.Line;
            int isWhite = (gameStatus.CurrentPlayer == PlayerType.White) ? 1 : -1;

            bool isDiagonalMove = Math.Abs(startCell.Line - endCell.Line) == 1 &&
                                  Math.Abs(startCell.Column - endCell.Column) == 1;

            bool isForwardMove = rowChange == isWhite;

            return isDiagonalMove && isForwardMove;
        }
        private bool IsKingMoveValid(GameStatus gameStatus, Cell startCell, Cell endCell)
        {
            if (startCell.Content != CheckerTypes.BlackKing && startCell.Content != CheckerTypes.WhiteKing)
                return false;

            var rowChange = Math.Abs(startCell.Line - endCell.Line);
            var columnChange = Math.Abs(startCell.Column - endCell.Column);
            return (rowChange == 1 && columnChange == 1);
        }

        private bool IsKingJumpValid(GameStatus gameStatus, Cell startCell, Cell endCell)
        {
            if (startCell.Content != CheckerTypes.BlackKing && startCell.Content != CheckerTypes.WhiteKing)
                return false;

            var rowChange = Math.Abs(startCell.Line - endCell.Line);
            var columnChange = Math.Abs(startCell.Column - endCell.Column);

            int middleIndex = ((startCell.Line + endCell.Line) / 2) * 8 + (startCell.Column + endCell.Column) / 2;

            bool isOpponentPiece = CheckerHelper.GetPlayerTypeFromChecker(gameStatus.Cells[middleIndex].Content) != gameStatus.CurrentPlayer;

            return (rowChange == 2 && columnChange == 2) && isOpponentPiece;
        }

    }

}

