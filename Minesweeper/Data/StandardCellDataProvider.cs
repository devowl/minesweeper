using System;

using Minesweeper.Common;

namespace Minesweeper.Data
{
    /// <summary>
    /// Standard cells data provider.
    /// </summary>
    public class StandardCellDataProvider : ICellDataProvider
    {
        private readonly bool[,] _bombs;

        private bool _gameOver = false;

        /// <summary>
        /// Constructor <see cref="EmptyCellDataProvider"/>.
        /// </summary>
        public StandardCellDataProvider(bool[,] sourceBombs)
        {
            _bombs = sourceBombs;
            FieldId = Guid.NewGuid();
            BombsCount = 0;
            for (int x = 0; x < sourceBombs.GetLength(0); x++)
            {
                for (int y = 0; y < sourceBombs.GetLength(1); y++)
                {
                    if (sourceBombs[x, y])
                    {
                        BombsCount++;
                    }
                }
            }
        }

        /// <inheritdoc/>
        public Guid FieldId { get; }

        /// <inheritdoc/>
        public int Width => _bombs.GetLength(0);

        /// <inheritdoc/>
        public int Height => _bombs.GetLength(1);

        /// <inheritdoc/>
        public bool this[int x, int y]
        {
            get
            {
                if (x >= 0 && x < Width && y >= 0 && y < Height)
                {
                    return _bombs[x, y];
                }

                return false;
            }
        }

        public int BombsCount { get; }

        /// <inheritdoc/>
        public event EventHandler<GameArgs> Gameover;

        /// <inheritdoc/>
        public void EndTypeUpdated(EndType endType, int flagged)
        {
            if (_gameOver)
            {
                return;
            }

            if (endType != EndType.ButtonPressed)
            {
                _gameOver = true;
            }

            Gameover?.Invoke(this, new GameArgs(endType, flagged));
        }
    }
}