using System;

namespace Minesweeper.Common
{ 
    /// <summary>
    /// Game event arguments.
    /// </summary>
    public class GameArgs : EventArgs
    {
        /// <summary>
        /// <see cref="GameArgs"/> constructor.
        /// </summary>
        public GameArgs(EndType endType, int flagged)
        {
            EndType = endType;
            Flagged = flagged;
        }

        /// <summary>
        /// Flagged cells.
        /// </summary>
        public int Flagged { get; private set; }

        /// <summary>
        /// End game type.
        /// </summary>
        public EndType EndType { get; private set; }
    }
}
