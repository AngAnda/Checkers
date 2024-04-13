using Checkers.Services;
using Checkers.ViewModels;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Checkers.Models
{
    public class GameStatus
    {
        public GameStatus()
        {
        }

        public GameStatus(PlayerType currentPlayer)
        {
            Cells = new ObservableCollection<Cell>();
            CurrentPlayer = currentPlayer;
            IsMultiJump = false;
            WhiteCheckers = 12;
            BlackCheckers = 12;
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

                    else
                    {
                        Cells.Add(new Cell(isBlack, i, j));
                    }
                }
            }
        }


        public GameStatus(ObservableCollection<Cell> cells, PlayerType currentPlayer, bool isMultiJump)
        {
            Cells = cells;
            CurrentPlayer = currentPlayer;
            IsMultiJump = isMultiJump;
        }

        [JsonConstructor]
        public GameStatus(ObservableCollection<Cell> cells, PlayerType currentPlayer, bool isMultiJump, int whiteCheckers, int blackCheckers)
        {
            Cells = cells;
            CurrentPlayer = currentPlayer;
            IsMultiJump = isMultiJump;
            WhiteCheckers = whiteCheckers;
            BlackCheckers = blackCheckers;
        }

        public ObservableCollection<Cell> Cells { get; set; }

        public PlayerType CurrentPlayer { get; set; }

        public bool IsMultiJump { get; set; }

        public int WhiteCheckers { get; set; }

        public int BlackCheckers { get; set; }

        public bool GameStarted { get; set; }

    }
}
