using System;

using Minesweeper.Data;

namespace Minesweeper.Windows.ViewModels
{
    /// <summary>
    /// <see cref="Views.MainWindow"/> view model.
    /// </summary>
    public class MainWindowViewModel
    {
        /// <summary>
        /// Constructor <see cref="MainWindowViewModel"/>.
        /// </summary>
        public MainWindowViewModel()
        {
            DataProvider = CreateRandomizedField();
        }

        /// <summary>
        /// Mines field data provider.
        /// </summary>
        public ICellDataProvider DataProvider { get; private set; }

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

            return new StandardCellDataProvider(randomField);
        }
    }
}