using System;
using System.Windows.Input;

using Minesweeper.Common;

namespace Minesweeper.Data
{
    /// <summary>
    /// Empty cells data provider.
    /// </summary>
    public class EmptyCellDataProvider : ICellDataProvider
    {
        /// <summary>
        /// Constructor <see cref="EmptyCellDataProvider"/>.
        /// </summary>
        public EmptyCellDataProvider()
        {
            FieldId = Guid.Empty;
        }
        
        /// <inheritdoc/>
        public Guid FieldId { get; }

        /// <inheritdoc/>
        public int Width => 10;

        /// <inheritdoc/>
        public int Height => 10;

        /// <inheritdoc/>
        public bool this[int x, int y] => false;

        public int BombsCount => 0;

        /// <inheritdoc/>
        public event EventHandler<GameArgs> Gameover;

        /// <inheritdoc/>
        public void EndTypeUpdated(EndType endType, int flagged)
        {
        }
    }
}