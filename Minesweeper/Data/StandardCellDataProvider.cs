using System;

namespace Minesweeper.Data
{
    /// <summary>
    /// Standard cells data provider.
    /// </summary>
    public class StandardCellDataProvider : ICellDataProvider
    {
        private readonly bool[,] _bombs;

        /// <summary>
        /// Constructor <see cref="EmptyCellDataProvider"/>.
        /// </summary>
        public StandardCellDataProvider(bool[,] sourceBombs)
        {
            _bombs = sourceBombs;
            FieldId = Guid.NewGuid();
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
    }
}