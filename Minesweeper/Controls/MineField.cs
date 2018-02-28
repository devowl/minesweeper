using System;
using System.Windows;
using System.Windows.Controls;

using Minesweeper.Data;

namespace Minesweeper.Controls
{
    /// <summary>
    /// Mines display control.
    /// </summary>
    public class MineField : Grid
    {
        private const int DefaultCellSize = 24;

        /// <summary>
        /// <see cref="CellSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CellSizeProperty;

        /// <summary>
        /// <see cref="CellDataProvider"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CellDataProviderProperty;

        private readonly MineFieldLogic _mineFieldLogic = new MineFieldLogic();

        private MineButton[,] _buttons;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static MineField()
        {
            CellSizeProperty = DependencyProperty.Register(
                nameof(CellSize),
                typeof(int),
                typeof(MineField),
                new PropertyMetadata(DefaultCellSize));

            CellDataProviderProperty = DependencyProperty.Register(
                nameof(CellDataProvider),
                typeof(ICellDataProvider),
                typeof(MineField),
                new PropertyMetadata(
                    default(EmptyCellDataProvider),
                    (sourceObject, args) =>
                    {
                        var mineField = sourceObject as MineField;
                        mineField?.CellDataProviderChanged(
                            (ICellDataProvider)args.OldValue,
                            (ICellDataProvider)args.NewValue);
                    }));
        }

        /// <summary>
        /// Constructor <see cref="MineField"/>.
        /// </summary>
        public MineField()
        {
            _mineFieldLogic.GameOver += OnGameOver;
        }

        private void OnGameOver(object sender, EventArgs e)
        {
            IsGameOver = true;
            _buttons.ForEach(
                (button, x, y) =>
                {
                    if (CellDataProvider[x, y] && button.CurrentCellType != CellType.BombExplode)
                    {
                        button.CurrentCellType = CellType.Bomb;
                    }
                });
        }

        /// <summary>
        /// <see cref="ICellDataProvider"/> instance.
        /// </summary>
        public ICellDataProvider CellDataProvider
        {
            get
            {
                return (ICellDataProvider)GetValue(CellDataProviderProperty);
            }
            set
            {
                SetValue(CellDataProviderProperty, value);
            }
        }

        /// <summary>
        /// Cell field size.
        /// </summary>
        public int CellSize
        {
            get
            {
                return (int)GetValue(CellSizeProperty);
            }
            set
            {
                SetValue(CellSizeProperty, value);
            }
        }

        private bool IsGameOver
        {
            get
            {
                return !IsEnabled;
            }

            set
            {
                IsEnabled = !value;
            }
        }

        private void CellDataProviderChanged(ICellDataProvider oldProvider, ICellDataProvider newProvider)
        {
            RemoveUnactualField(oldProvider);

            if (newProvider == null)
            {
                CellDataProvider = new EmptyCellDataProvider();
            }

            _mineFieldLogic.CellDataProvider = newProvider;
            DrawNewField();
        }

        private void RemoveUnactualField(ICellDataProvider oldProvider)
        {
            if (oldProvider == null)
            {
                return;
            }

            RowDefinitions.Clear();
            ColumnDefinitions.Clear();

            _buttons.ForEach(RemoveLogicalChild);
            _buttons.ForEach(
                (button =>
                 {
                     RemoveLogicalChild(button);
                     button.InputBindings.Clear();
                 }));
            
            _buttons = null;
        }
        
        private void DrawNewField()
        {
            var provider = CellDataProvider;
            _buttons = new MineButton[provider.Width, provider.Height];

            // Add grid definitions
            for (int i = 0; i < provider.Width; i ++)
            {
                var definition = new RowDefinition() { Height = GridLength.Auto };
                RowDefinitions.Add(definition);
            }

            for (int j = 0; j < provider.Height; j++)
            {
                var definition = new ColumnDefinition() { Width = GridLength.Auto };
                ColumnDefinitions.Add(definition);
            }
            
            for (int i = 0; i < provider.Width; i++)
            {
                for (int j = 0; j < provider.Height; j++)
                {
                    var button = new MineButton() { Width = CellSize, Height = CellSize };
                    Children.Add(button);
                    SetColumn(button, i);
                    SetRow(button, j);

                    _buttons[i, j] = button;
                }
            }

            _mineFieldLogic.AttachButtons(_buttons);
            IsGameOver = false;
        }
    }
}