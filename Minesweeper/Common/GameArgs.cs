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
        public GameArgs(EndType endType)
        {
            EndType = endType;
        }

        /// <summary>
        /// End game type.
        /// </summary>
        public EndType EndType { get; private set; }
    }
}
