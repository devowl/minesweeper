using System;
using System.Windows.Input;

namespace Minesweeper.Data
{
    /// <summary>
    /// Standard cells data provider.
    /// </summary>
    public class StandardCellDataProvider : ICellDataProvider
    {
        private readonly CellType[,] _bombs;

        /// <summary>
        /// Constructor <see cref="EmptyCellDataProvider"/>.
        /// </summary>
        public StandardCellDataProvider(bool[,] sourceBombs)
        {
            _bombs = CreateBombs(sourceBombs);
            FieldId = Guid.NewGuid();
        }
        
        /// <inheritdoc/>
        public Guid FieldId { get; }

        /// <inheritdoc/>
        public int Width => _bombs.GetLength(0);

        /// <inheritdoc/>
        public int Height => _bombs.GetLength(1);

        /// <inheritdoc/>
        public CellType this[int x, int y] => _bombs[x, y];

        /// <inheritdoc/>
        public void Press(int x, int y, MouseButton button)
        {
            var cellType = _bombs[x, y];
            CellType newValue = cellType;

            switch (cellType)
            {
                case CellType.Button:
                    break;
                case CellType.Flagged:
                    break;
                case CellType.BombExplode:
                    break;
                case CellType.NoBomb:
                    break;
                case CellType.Bomb:
                    if (button == MouseButton.Left)
                    {
                        newValue = CellType.BombExplode;
                    }
                    break;
                case CellType.Empty:
                    break;
            }

            _bombs[x, y] = newValue;
        }

        private static CellType[,] CreateBombs(bool[,] sourceBombs)
        {
            int sourceWidth = sourceBombs.GetLength(0), sourceHeight = sourceBombs.GetLength(1);
            var result = new CellType[sourceWidth, sourceHeight];
            for (int x = 0; x < sourceBombs.GetLength(0); x++)
            {
                for (int y = 0; y < sourceBombs.GetLength(1); y++)
                {
                    var isBomb = sourceBombs[x, y];

                    result[x, y] = isBomb ? CellType.Bomb : CellType.Empty;
                }
            }

            return result;
        }
    }
}