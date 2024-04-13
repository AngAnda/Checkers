using Checkers.Models;
using Checkers.Services;
using Checkers.Views;
using System.Windows;
using System.Windows.Input;

namespace Checkers.ViewModels
{
    public class BoardViewModel : BaseViewModel
    {
        private readonly GameService _gameService;

        private readonly FilesService _jsonService;

        private GameStatus _gameStatus;

        public GameStatus GameStatus
        {
            get => _gameStatus;
            set
            {
                if (_gameStatus != value)
                {
                    _gameStatus = value;
                    OnPropertyChanged(nameof(GameStatus));
                    OnPropertyChanged(nameof(GameStatus.Cells));
                }
            }
        }

        public PlayerType CurrentPlayer
        {
            get { return _gameStatus.CurrentPlayer; }

            set
            {
                if (_gameStatus.CurrentPlayer != value)
                {
                    _gameStatus.CurrentPlayer = value;
                    OnPropertyChanged(nameof(CurrentPlayer));
                }
            }
        }


        public BoardViewModel()
        {

            GameStatus = new GameStatus(PlayerType.White);
            _gameService = new GameService(GameStatus);
            _jsonService = new FilesService();

            ClickCellCommand = new RelayCommand(ClickCell);
            MovePieceCommand = new RelayCommand(MovePiece, IsMoveValid);
            SaveGameCommand = new RelayCommand(SaveGame);
            LoadGameCommand = new RelayCommand(LoadGame);
            NewGameCommand = new RelayCommand(NewGame);
            DisplayInfoCommand = new RelayCommand(DisplayInfo);
            DisplayStatisticsCommand = new RelayCommand(DisplayStatistics);
            ToggleMultipleJumps = new RelayCommand(ToggleMultipleJumpsCommand, IsMultipleJumpsCheckable);
        }


        public ICommand ClickCellCommand { get; set; }

        public ICommand MovePieceCommand { get; set; }

        public ICommand SaveGameCommand { get; set; }

        public ICommand LoadGameCommand { get; set; }

        public ICommand NewGameCommand { get; set; }

        public ICommand DisplayInfoCommand { get; set; }

        public ICommand DisplayStatisticsCommand { get; set; }

        public ICommand ToggleMultipleJumps { get; set; }

        public void DisplayInfo(object parameter)
        {
            About aboutWindow = new About();
            aboutWindow.Show();
        }

        public void DisplayStatistics(object parameter)
        {
            Statistics statisticsWindow = new Statistics();
            statisticsWindow.Show();
        }

        public void NewGame(object parameter)
        {
            MessageBoxResult result = MessageBox.Show(
            "Do you want to start a new game?",
            "New Game Settings",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                GameStatus = new GameStatus(PlayerType.White);
                _gameService.LoadGame(GameStatus);
            }

        }

        public void SaveGame(object parameter)
        {
            _jsonService.SaveObjectToFile(GameStatus);
        }

        public void LoadGame(object parameter)
        {
            try
            {
                GameStatus = _jsonService.LoadObjectFromFile<GameStatus>();
                _gameService.LoadGame(GameStatus);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error loading game: " + e.Message);
            }
        }

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
            int whiteCheckers = GameStatus.WhiteCheckers;
            int blackCheckers = GameStatus.BlackCheckers;

            _gameService.MovePiece(GameStatus.Cells, ref whiteCheckers, ref blackCheckers);

            GameStatus.WhiteCheckers = whiteCheckers;
            GameStatus.BlackCheckers = blackCheckers;
            _gameService.GameOver(GameStatus.WhiteCheckers, GameStatus.BlackCheckers);


            _gameStatus.GameStarted = true;
        }

        public bool IsMoveValid(object parameter)
        {
            return _gameService.IsMoveValid(GameStatus.Cells);
        }

        public void ToggleMultipleJumpsCommand(object parameter)
        {
            _gameStatus.IsMultiJump = !_gameStatus.IsMultiJump;
            MessageBox.Show("Multiple jumps are now " + (_gameStatus.IsMultiJump ? "enabled" : "disabled"));
        }

        public bool IsMultipleJumpsCheckable(object parameter)
        {
            return !_gameStatus.GameStarted;
        }

    }
}
