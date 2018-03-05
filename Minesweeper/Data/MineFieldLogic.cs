using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using Minesweeper.Common;
using Minesweeper.Controls;
using Minesweeper.Prism;

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
        /// Current cell data provider.
        /// </summary>
        public ICellDataProvider CellDataProvider { get; set; }

        private static bool IsTwoButtonsClick
            => Mouse.LeftButton == MouseButtonState.Pressed && Mouse.RightButton == MouseButtonState.Pressed;

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

            CellDataProvider.Gameover += CellDataProviderOnGameover;
            _buttons = buttons;
        }

        private void CellDataProviderOnGameover(object sender, GameArgs args)
        {
            if (args.EndType == EndType.YouHaveLost || args.EndType == EndType.YouHaveLost)
            {
                // Print all unchecked bombs
                _buttons.ForEach(
                    (button, x, y) =>
                    {
                        if (CellDataProvider[x, y] && button.CurrentCellType != CellType.BombExplode &&
                            button.CurrentCellType != CellType.Flagged)
                        {
                            button.SetType(CellType.Bomb);
                        }
                    });
            }
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

        private void CheckGameStatus()
        {
            bool anyClosed = false;

            _buttons.ForEach(
                (button, x, y) =>
                {
                    if (button.CurrentCellType == CellType.Button)
                    {
                        anyClosed = true;
                    }
                });

            if (anyClosed)
            {
                CellDataProvider.EndTypeUpdated(EndType.ButtonPressed);
                return;
            }

            int foundedBombs = 0;
            int closedFields = 0;
            _buttons.ForEach(
                (button, x, y) =>
                {
                    if (button.CurrentCellType == CellType.Flagged)
                    {
                        foundedBombs++;
                    }

                    if (button.CurrentCellType == CellType.Button)
                    {
                        closedFields++;
                    }
                });

            if (foundedBombs == CellDataProvider.BombsCount && closedFields == 0)
            {
                // Show up all bombs. You have won
                CellDataProvider.EndTypeUpdated(EndType.YouHaveWon);
            }
        }

        private void RightButtonClick(object parameter)
        {
            var info = ((ButtonInfo)parameter);
            if (IsTwoButtonsClick)
            {
                ProcessTwoButtonsClick(info);
                return;
            }

            InternalRightButtonClick(info);
        }

        private void InternalRightButtonClick(ButtonInfo info)
        {
            if (info.Button.CurrentCellType == CellType.Button)
            {
                info.Button.SetType(CellType.Flagged);
            }
            else if (info.Button.CurrentCellType == CellType.Flagged)
            {
                info.Button.SetType(CellType.Button);
            }

            CheckGameStatus();
        }

        private void ProcessTwoButtonsClick(ButtonInfo info)
        {
            // if all bombs around are marked, then we do click on everyone button around
            var realBombs = GetBombsAround(info.X, info.Y, (x, y) => CellDataProvider[x, y]);
            var flaggedBombs = GetBombsAround(
                info.X,
                info.Y,
                (x, y) =>
                {
                    if (CheckCellBound(x, y))
                    {
                        var infoButton = (ButtonInfo)_buttons[x, y].Tag;
                        return infoButton.Button.CurrentCellType == CellType.Flagged;
                    }

                    return false;
                });

            if (realBombs == flaggedBombs)
            {
                GetBombsAround(
                    info.X,
                    info.Y,
                    (x, y) =>
                    {
                        if (CheckCellBound(x, y))
                        {
                            var infoButton = (ButtonInfo)_buttons[x, y].Tag;
                            if (infoButton.Button.CurrentCellType == CellType.Button)
                            {
                                InternalLeftButtonClick(infoButton);
                            }
                        }

                        return true;
                    });
            }
        }

        /// <summary>
        /// Button left button click handler.
        /// </summary>
        private void LeftButtonClick(object parameter)
        {
            var info = ((ButtonInfo)parameter);
            if (IsTwoButtonsClick)
            {
                ProcessTwoButtonsClick(info);
                return;
            }

            InternalLeftButtonClick(info);
        }

        private void InternalLeftButtonClick(ButtonInfo info)
        {
            if (info.Button.CurrentCellType == CellType.Button)
            {
                if (CellDataProvider[info.X, info.Y])
                {
                    info.Button.SetType(CellType.BombExplode);
                    CellDataProvider.EndTypeUpdated(EndType.YouHaveLost);
                }
                else
                {
                    info.Button.SetType(CellType.Empty);
                    int bombsNear = GetBombsAround(info.X, info.Y, (x, y) => CellDataProvider[x, y]);
                    if (bombsNear == 0)
                    {
                        var marked = new HashSet<ButtonInfo>();
                        MarkEmptyAround(info, marked);
                    }
                    else
                    {
                        info.Button.SetType((CellType)Enum.Parse(typeof(CellType), $"Near{bombsNear}"));
                    }
                }
            }

            CheckGameStatus();
        }

        private void MarkEmptyAround(ButtonInfo info, HashSet<ButtonInfo> marked)
        {
            marked.Add(info);
            if (info.Button.CurrentCellType == CellType.Flagged)
            {
                return;
            }

            var bombsAround = GetBombsAround(info.X, info.Y, (x, y) => CheckCellBound(x, y) && CellDataProvider[x, y]);

            if (bombsAround == 0)
            {
                info.Button.SetType(CellType.Empty);
            }
            else
            {
                info.Button.SetType((CellType)Enum.Parse(typeof(CellType), $"Near{bombsAround}"));
                return;
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