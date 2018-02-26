using System;
using System.Windows.Input;

namespace Minesweeper.Data
{
    /// <summary>
    /// Field cell data info provider.
    /// </summary>
    public interface ICellDataProvider
    {
        /// <summary>
        /// Game field identity.
        /// </summary>
        Guid FieldId { get; }

        /// <summary>
        /// Field width.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Field height.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Get cell index;
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <returns>Cell type.</returns>
        CellType this[int x, int y] { get; }

        /// <summary>
        /// User button press.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="button">Pressed button.</param>
        void Press(int x, int y, MouseButton button);
    }
}