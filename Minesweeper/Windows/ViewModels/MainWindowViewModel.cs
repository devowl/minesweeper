using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;

using Minesweeper.Common;
using Minesweeper.Data;
using Minesweeper.Prism;
using Minesweeper.Windows.Views;

namespace Minesweeper.Windows.ViewModels
{
    /// <summary>
    /// <see cref="Views.MainWindow"/> view model.
    /// </summary>
    public class MainWindowViewModel : BindableBase
    {
        private readonly IDictionary<CurrentMode, Func<ICellDataProvider>> _fieldGenerator;

        private readonly Timer _timer;

        private ICellDataProvider _dataProvider;

        private CurrentMode _currentMode = CurrentMode.Beginner;

        private int _bombsLeft;

        private int _secondsGone;

        private EmotionType _currentEmotionType;

        /// <summary>
        /// Constructor <see cref="MainWindowViewModel"/>.
        /// </summary>
        public MainWindowViewModel()
        {
            _timer = new Timer(1000);
            _timer.Elapsed += (sender, args) => SecondsGone++;
            _fieldGenerator = new Dictionary<CurrentMode, Func<ICellDataProvider>>
            {
                { CurrentMode.Beginner, () => CreateRandomizedField(9, 9, 10) },
                { CurrentMode.Intermediate, () => CreateRandomizedField(16, 16, 40) },
                { CurrentMode.Expert, () => CreateRandomizedField(30, 16, 90) }
            };

            NewGameCommand = new DelegateCommand(p => NewGame());
            BeginnerCommand = new DelegateCommand(p => NewGame(CurrentMode.Beginner));
            IntermediateCommand = new DelegateCommand(p => NewGame(CurrentMode.Intermediate));
            ExpertCommand = new DelegateCommand(p => NewGame(CurrentMode.Expert));
            ExitCommand = new DelegateCommand(p => Application.Current.Shutdown());
            AboutCommand = new DelegateCommand(AboutClick);

            NewGame();
        }

        private enum CurrentMode
        {
            Beginner,

            Intermediate,

            Expert
        }

        /// <summary>
        /// How many bombs left.
        /// </summary>
        public int BombsLeft
        {
            get
            {
                return _bombsLeft;
            }

            set
            {
                _bombsLeft = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Seconds gone from begin.
        /// </summary>
        public int SecondsGone
        {
            get
            {
                return _secondsGone;
            }

            set
            {
                _secondsGone = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// New game command.
        /// </summary>
        public DelegateCommand NewGameCommand { get; private set; }

        /// <summary>
        /// New game beginner command.
        /// </summary>
        public DelegateCommand BeginnerCommand { get; private set; }

        /// <summary>
        /// New game intermediate command.
        /// </summary>
        public DelegateCommand IntermediateCommand { get; private set; }

        /// <summary>
        /// New game expert command.
        /// </summary>
        public DelegateCommand ExpertCommand { get; private set; }

        /// <summary>
        /// Exit command.
        /// </summary>
        public DelegateCommand ExitCommand { get; private set; }

        /// <summary>
        /// About command.
        /// </summary>
        public DelegateCommand AboutCommand { get; private set; }

        /// <summary>
        /// Current emotion type.
        /// </summary>
        public EmotionType CurrentEmotionType
        {
            get
            {
                return _currentEmotionType;
            }

            set
            {
                _currentEmotionType = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Mines field data provider.
        /// </summary>
        public ICellDataProvider DataProvider
        {
            get
            {
                return _dataProvider;
            }

            private set
            {
                _dataProvider = value;
                RaisePropertyChanged();
            }
        }

        private void AboutClick(object obj)
        {
            var aboutWindow = new AboutWindow()
            {
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            aboutWindow.ShowDialog();
        }

        private ICellDataProvider CreateRandomizedField(int width, int height, int bombs)
        {
            var random = new Random();
            var randomField = new bool[height, width];

            for (int i = 0; i < bombs; i++)
            {
                do
                {
                    int randomX = random.Next(0, width);
                    int randomY = random.Next(0, height);

                    if (!randomField[randomY, randomX])
                    {
                        randomField[randomY, randomX] = true;
                        break;
                    }
                } while (true);
            }

            var provider = new StandardCellDataProvider(randomField);
            provider.Gameover += OnGameover;
            return provider;
        }

        private void OnGameover(object sender, GameArgs gameArgs)
        {
            if (gameArgs.EndType == EndType.ButtonPressed)
            {
                BombsLeft = DataProvider.BombsCount - gameArgs.Flagged;

                if (!_timer.Enabled)
                {
                    _timer.Start();
                }
            }

            if (gameArgs.EndType == EndType.YouHaveWon)
            {
                _timer.Stop();
                CurrentEmotionType = EmotionType.Win;
            }
            else if (gameArgs.EndType == EndType.YouHaveLost)
            {
                _timer.Stop();
                CurrentEmotionType = EmotionType.Lose;
            }
        }

        private void NewGame(CurrentMode newMode)
        {
            _currentMode = newMode;
            NewGame();
        }

        private void NewGame()
        {
            if (DataProvider != null)
            {
                DataProvider.Gameover -= OnGameover;
            }

            CurrentEmotionType = EmotionType.Common;
            DataProvider = _fieldGenerator[_currentMode]();
            SecondsGone = 0;
            BombsLeft = DataProvider.BombsCount;
        }
    }
}