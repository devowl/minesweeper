using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using Minesweeper.Common;
using Minesweeper.Data;

namespace Minesweeper.Controls
{
    /// <summary>
    /// Classic preview button.
    /// </summary>
    public class MineButton : ContentControl
    {
        private const int ButtonImageOffsetStep = 16;

        /// <summary>
        /// Dependency property for <see cref="CurrentCellType"/>.
        /// </summary>
        public static readonly DependencyProperty CurrentCellTypeProperty =
            DependencyProperty.Register(
                nameof(CurrentCellType),
                typeof(CellType),
                typeof(MineButton),
                new PropertyMetadata(CellType.Button, CellTypeChanged));

        private static readonly BitmapSource ButtonTypesImageSource;
        
        static MineButton()
        {
            if (SharedUtils.InDesignMode())
            {
                return;
            }

            ButtonTypesImageSource =
                new BitmapImage(SharedUtils.ButtonTypeUri);
        }

        /// <summary>
        /// Constructor <see cref="MineButton"/>.
        /// </summary>
        public MineButton()
        {
            DataContext = this;
        }

        /// <summary>
        /// Current cell type value.
        /// </summary>
        public CellType CurrentCellType
        {
            get
            {
                return (CellType)GetValue(CurrentCellTypeProperty);
            }

            private set
            {
                SetValue(CurrentCellTypeProperty, value);
            }
        }

        /// <summary>
        /// Set new button type.
        /// </summary>
        /// <param name="cellType">New <see cref="CellType"/> value.</param>
        public void SetType(CellType cellType)
        {
            if (CurrentCellType == cellType)
            {
                return;
            }

            CurrentCellType = cellType;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            UpdateButtonImage();
        }

        private static void CellTypeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (args.OldValue != args.NewValue)
            {
                ((MineButton)dependencyObject).UpdateButtonImage();
            }
        }

        private static Image GetCroppedBitmap(CellType cellType)
        {
            int offset = (int)cellType;
            var bitmap = new CroppedBitmap(
                ButtonTypesImageSource,
                new Int32Rect(0, offset * ButtonImageOffsetStep, ButtonImageOffsetStep, ButtonImageOffsetStep));

            return new Image { Source = bitmap };
        }

        private void UpdateButtonImage()
        {
            Content = GetCroppedBitmap(CurrentCellType);
        }
    }
}