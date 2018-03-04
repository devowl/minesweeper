using System;

using Minesweeper.Controls;

namespace Minesweeper.Common
{
    /// <summary>
    /// Extentions container.
    /// </summary>
    public static class CommonExtentions
    {
        /// <summary>
        /// For each implementation for array.
        /// </summary>
        /// <param name="buttons">Buttons array.</param>
        /// <param name="process">Process each one button.</param>
        public static void ForEach(this MineButton[,] buttons, Action<MineButton, int, int> process)
        {
            for (int i = 0; i < buttons.GetLength(0); i++)
            {
                for (int j = 0; j < buttons.GetLength(1); j++)
                {
                    var button = buttons[i, j];
                    process(button, i, j);
                }
            }
        }

        /// <summary>
        /// For each implementation for array.
        /// </summary>
        /// <param name="buttons">Buttons array.</param>
        /// <param name="process">Process each one button.</param>
        public static void ForEach(this MineButton[,] buttons, Action<MineButton> process)
        {
            buttons.ForEach((button, x, y) => process(button));
        }
    }
}