namespace Minesweeper.Data
{
    /// <summary>
    /// Field cell type.
    /// </summary>
    public enum CellType
    {
        /// <summary>
        /// Simple button.
        /// </summary>
        Button = 0,

        /// <summary>
        /// Flagged cell.
        /// </summary>
        Flagged = 1,

        /// <summary>
        /// Pressed cell exploded.
        /// </summary>
        BombExplode = 3,

        /// <summary>
        /// Wrong flagged cell.
        /// </summary>
        NoBomb = 4,

        /// <summary>
        /// Bomb cell.
        /// </summary>
        Bomb = 5,

        /// <summary>
        /// 8 bombs around.
        /// </summary>
        Near8 = 7,

        /// <summary>
        /// 7 bombs around.
        /// </summary>
        Near7 = 8,

        /// <summary>
        /// 6 bombs around.
        /// </summary>
        Near6 = 9,

        /// <summary>
        /// 5 bombs around.
        /// </summary>
        Near5 = 10,

        /// <summary>
        /// 4 bombs around.
        /// </summary>
        Near4 = 11,

        /// <summary>
        /// 3 bombs around.
        /// </summary>
        Near3 = 12,

        /// <summary>
        /// 2 bombs around.
        /// </summary>
        Near2 = 13,

        /// <summary>
        /// 1 bombs around.
        /// </summary>
        Near1 = 14,

        /// <summary>
        /// Empty opened cell.
        /// </summary>
        Empty = 15
    }
}