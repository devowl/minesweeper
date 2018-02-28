using System;
using System.Windows.Input;

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

        /// <inheritdoc/>
        public void Press(int x, int y, MouseButton button)
        {
        }
    }
}