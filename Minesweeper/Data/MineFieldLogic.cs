using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using Minesweeper.Controls;

namespace Minesweeper.Data
{
    /// <summary>
    /// Mine sweeper field logic.
    /// </summary>
    public class MineFieldLogic
    {
        private MineButton[,] _buttons;

        /// <summary>
        /// <see cref="MineFieldLogic"/> constructor.
        /// </summary>
        public MineFieldLogic()
        {
            LeftButtonClickCommand = new DelegateCommand(LeftButtonClick);
            RightButtonClickCommand = new DelegateCommand(RightButtonClick);
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
        /// Process right button click.
        /// </summary>
        private DelegateCommand RightButtonClickCommand { get; }

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
                    button.Tag = info;
                    button.InputBindings.Add(
                        new InputBinding(LeftButtonClickCommand, new MouseGesture(MouseAction.LeftClick))
                        {
                            CommandParameter = info
                        });
                    button.InputBindings.Add(
                        new InputBinding(RightButtonClickCommand, new MouseGesture(MouseAction.RightClick))
                        {
                            CommandParameter = info
                        });
                });

            _buttons = buttons;
        }

        private static int GetBombsAround(int x, int y, Func<int, int, bool> check)
        {
            var bombs = new[]
            {
                check(x - 1, y + 1),
                check(x, y + 1),
                check(x + 1, y + 1),
                check(x - 1, y),
                check(x, y),
                check(x + 1, y),
                check(x - 1, y - 1),
                check(x, y - 1),
                check(x + 1, y - 1),
            };

            return bombs.Where(isBomb => isBomb).Count();
        }

        private void RightButtonClick(object parameter)
        {
            var info = ((ButtonInfo)parameter);
            if (info.Button.CurrentCellType == CellType.Button)
            {
                info.Button.CurrentCellType = CellType.Flagged;
            }
        }

        /// <summary>
        /// Button left button click handler.
        /// </summary>
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
                    int bombsNear = GetBombsAround(info.X, info.Y, (x, y) => CellDataProvider[x, y]);
                    if (bombsNear == 0)
                    {
                        var marked = new HashSet<ButtonInfo>();
                        MarkEmptyAround(info, marked);
                    }
                    else
                    {
                        info.Button.CurrentCellType = (CellType)Enum.Parse(typeof(CellType), $"Near{bombsNear}");
                    }
                }
            }
        }

        private void MarkEmptyAround(ButtonInfo info, HashSet<ButtonInfo> marked)
        {
            marked.Add(info);
            var bombsAround = GetBombsAround(
                info.X,
                info.Y,
                (x, y) =>
                    CheckCellBound(x, y) && CellDataProvider[x, y]);

            if (bombsAround == 0)
            {
                info.Button.CurrentCellType = CellType.Empty;
            }
            else
            {
                LeftButtonClick(info.Button.Tag);
            }

            GetBombsAround(
                info.X,
                info.Y,
                (x, y) =>
                {
                    if (CheckCellBound(x, y))
                    {
                        var infoButton = (ButtonInfo)_buttons[x, y].Tag;
                        if (!marked.Contains(infoButton))
                        {
                            MarkEmptyAround(infoButton, marked);
                        }
                    }

                    return true;
                });
        }

        private bool CheckCellBound(int x, int y)
        {
            return x >= 0 && x < CellDataProvider.Width && y >= 0 && y < CellDataProvider.Height;
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

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                return X.GetHashCode() ^ Y.GetHashCode();
            }
        }
    }
}