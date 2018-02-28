using System;
using System.Windows.Input;

using Minesweeper.Controls;

namespace Minesweeper.Data
{
    /// <summary>
    /// Mine sweeper field logic.
    /// </summary>
    public class MineFieldLogic
    {
        /// <summary>
        /// <see cref="MineFieldLogic"/> constructor.
        /// </summary>
        public MineFieldLogic()
        {
            LeftButtonClickCommand = new DelegateCommand(LeftButtonClick);
        }

        /// <summary>
        /// Game over event.
        /// </summary>
        public event EventHandler<EventArgs> GameOver;

        /// <summary>
        /// Current cell data provider.
        /// </summary>
        public ICellDataProvider CellDataProvider { get; set; }

        /// <summary>
        /// Process left button click.
        /// </summary>
        private DelegateCommand LeftButtonClickCommand { get; }

        /// <summary>
        /// Attach buttons.
        /// </summary>
        /// <param name="buttons">Buttons reference.</param>
        public void AttachButtons(MineButton[,] buttons)
        {
            buttons.ForEach(
                (button, x, y) =>
                {
                    var info = new ButtonInfo(x, y, button);
                    button.InputBindings.Add(
                        new InputBinding(LeftButtonClickCommand, new MouseGesture(MouseAction.LeftClick))
                        {
                            CommandParameter = info
                        });
                });
        }
        
        private void LeftButtonClick(object parameter)
        {
            var info = ((ButtonInfo)parameter);
            if (info.Button.CurrentCellType == CellType.Button)
            {
                if (CellDataProvider[info.X, info.Y])
                {   
                    info.Button.CurrentCellType = CellType.BombExplode;
                    GameOver?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    info.Button.CurrentCellType = CellType.Empty;
                }
            }
        }

        private class ButtonInfo
        {
            /// <summary>
            /// <see cref="ButtonInfo"/> constructor.
            /// </summary>
            public ButtonInfo(int x, int y, MineButton mineButton)
            {
                X = x;
                Y = y;
                Button = mineButton;
            }

            /// <summary>
            /// Button reference.
            /// </summary>
            public MineButton Button { get; }

            /// <summary>
            /// X position.
            /// </summary>
            public int X { get; }

            /// <summary>
            /// Y position.
            /// </summary>
            public int Y { get; }
        }
    }
}