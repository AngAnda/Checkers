namespace Checkers.ViewModels
{
    public class Cell : BaseViewModel
    {
        private bool _isBlack;

        private bool _isOccupied;

        private CheckerTypes _content;

        public bool IsBlack
        {
            get { return _isBlack; }

            set
            {
                _isBlack = value;
                OnPropertyChanged(nameof(IsBlack));
            }
        }

        public bool IsOccupied
        {
            get { return _isOccupied; }

            set
            {
                _isOccupied = value;
                OnPropertyChanged(nameof(IsOccupied));
            }
        }

        public CheckerTypes Content
        {
            get { return _content; }

            set
            {
                _content = value;
                OnPropertyChanged(nameof(Content));
            }
        }

        public Cell(bool isBlack, CheckerTypes content = default)
        {
            IsBlack = isBlack;
            if (content != default)
            {
                IsOccupied = true;
            }
            else
            {
                IsOccupied = false;

            }
            Content = content;
        }
    }
}
