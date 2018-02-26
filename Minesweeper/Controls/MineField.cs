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

        private MineButton[,] _buttons;
        /// <summary>
        /// <see cref="CellSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CellSizeProperty;

        /// <summary>
        /// <see cref="CellDataProvider"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CellDataProviderProperty;

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
                new PropertyMetadata(default(EmptyCellDataProvider),
                    (sourceObject, args) =>
                    {
                        var mineField = sourceObject as MineField;
                        mineField?.CellDataProviderChanged(
                            (ICellDataProvider)args.OldValue,
                            (ICellDataProvider)args.NewValue);
                    }));
        }

        private void CellDataProviderChanged(ICellDataProvider oldProvider, ICellDataProvider newProvider)
        {
            RemoveOldField(oldProvider);

            if (newProvider == null)
            {
                CellDataProvider = new EmptyCellDataProvider();
            }

            CreateNewField();
        }

        private void RemoveOldField(ICellDataProvider oldProvider)
        {
            if (oldProvider == null)
            {
                return;
            }

            RowDefinitions.Clear();
            ColumnDefinitions.Clear();
            for (int i = 0; i < _buttons.GetLength(0); i++)
            {
                for (int j = 0; j < _buttons.GetLength(1); j++)
                {
                    var button = _buttons[i, j];
                    button.Click -= ButtonOnClick;
                    RemoveLogicalChild(button);
                }
            }

            _buttons = null;
        }

        private void ButtonOnClick(object sender, RoutedEventArgs args) 
        {
            
        }

        private void CreateNewField()
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
                    button.Click += ButtonOnClick;

                    Children.Add(button);
                    SetColumn(button, i);
                    SetRow(button, j);

                    _buttons[i, j] = button;
                    
                }
            }
        }
        
        /// <summary>
        /// Constructor <see cref="MineField"/>.
        /// </summary>
        public MineField()
        {
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
    }
}