using System;
using System.Windows;

using Minesweeper.Common;
using Minesweeper.Data;
using Minesweeper.Prism;

namespace Minesweeper.Windows.ViewModels
{
    /// <summary>
    /// <see cref="Views.MainWindow"/> view model.
    /// </summary>
    public class MainWindowViewModel : BindableBase
    {
        private ICellDataProvider _dataProvider;

        /// <summary>
        /// Constructor <see cref="MainWindowViewModel"/>.
        /// </summary>
        public MainWindowViewModel()
        {
            DataProvider = CreateRandomizedField();
            NewGameCommand = new DelegateCommand(NewGameClick);
        }

        /// <summary>
        /// New game command.
        /// </summary>
        public DelegateCommand NewGameCommand { get; private set; }

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

        private static ICellDataProvider CreateRandomizedField()
        {
            var random = new Random();
            var randomField = new bool[13, 15];
            for (int x = 0; x < randomField.GetLength(0); x++)
            {
                for (int y = 0; y < randomField.GetLength(1); y++)
                {
                    var randomNumber = random.Next(0, 100);
                    if (randomNumber <= 7)
                    {
                        randomField[x, y] = true;
                    }
                }
            }

            var provider = new StandardCellDataProvider(randomField);
            provider.Gameover += OnGameover;
            return provider;
        }

        private static void OnGameover(object sender, GameArgs gameArgs) 
        {
            if (gameArgs.EndType == EndType.YouHaveWon)
            {
                MessageBox.Show("You have won! Congratulations!");
            }
            else if (gameArgs.EndType == EndType.YouHaveLost)
            {
                MessageBox.Show("You lose, try again!");
            }
        }

        private void NewGameClick(object obj)
        {
            DataProvider = CreateRandomizedField();
        }
    }
}