using System;

using Minesweeper.Common;

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
        /// Get "Is bomb" info.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <returns>Is cell bomb.</returns>
        bool this[int x, int y] { get; }

        /// <summary>
        /// When game over, check event arguments.
        /// </summary>
        event EventHandler<GameArgs> Gameover;

        /// <summary>
        /// End type changed.
        /// </summary>
        /// <param name="endType"><see cref="EndType"/> reference.</param>
        void EndTypeUpdated(EndType endType);
    }
}