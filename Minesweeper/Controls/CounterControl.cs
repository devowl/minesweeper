using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using Minesweeper.Data;

namespace Minesweeper.Controls
{
    /// <summary>
    /// Present digital counter.
    /// </summary>
    public class CounterControl : Grid
    {
        private const int NumberWidth = 13;

        private const int NumberHeight = 23;

        /// <summary>
        /// Dependency property for <see cref="Number"/>.
        /// </summary>
        public static readonly DependencyProperty NumberProperty;

        private static readonly BitmapSource NumbersImageSource;
        
        /// <summary>
        /// Static constructor.
        /// </summary>
        static CounterControl()
        {
            if (InDesignMode())
            {
                return;
            }

            NumbersImageSource =
                new BitmapImage(new Uri("pack://application:,,,/Images/Numbers.bmp", UriKind.RelativeOrAbsolute));

            NumberProperty = DependencyProperty.Register(
                nameof(Number),
                typeof(int?),
                typeof(CounterControl),
                new PropertyMetadata(0, NumberChangedCallback));
        }

        public static bool InDesignMode()
        {
            return !(Application.Current is App);
        }

        /// <summary>
        /// <see cref="CounterControl"/> constructor.
        /// </summary>
        public CounterControl()
        {
            if (InDesignMode())
            {
                return;
            }

            ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            NumberChanged();
        }

        /// <summary>
        /// Preview number value from -99 to 999.
        /// </summary>
        public int? Number
        {
            get
            {
                return (int?)GetValue(NumberProperty);
            }

            set
            {
                SetValue(NumberProperty, value);
            }
        }

        private static void NumberChangedCallback(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs args)
        {
            if (args.NewValue == args.OldValue)
            {
                return;
            }

            var counterControl = ((CounterControl)dependencyObject);
            counterControl.NumberChanged();
        }

        private static void GetLastDigit(ref int number, out int result)
        {
            result = 0;
            if (number == 0)
            {
                number = 0;
            }
            else
            {
                result = number % 10;
                number = number / 10;
            }
        }

        private static Image GetCroppedBitmap(NumberType numberType)
        {
            int offset = (int)numberType;
            var bitmap = new CroppedBitmap(
                NumbersImageSource,
                new Int32Rect(0, offset * NumberHeight, NumberWidth, NumberHeight));

            return new Image { Source = bitmap };
        }

        private void NumberChanged()
        {
            ClearView();

            bool isNegative = Number < 0;
            if (Number.HasValue)
            {
                var number = Number.Value;
                int firstDigit, secondDigit, thirdDigit;

                GetLastDigit(ref number, out firstDigit);
                GetLastDigit(ref number, out secondDigit);
                GetLastDigit(ref number, out thirdDigit);

                if (isNegative)
                {
                    AddImage(NumberType.Minus, 0);
                }
                else
                {
                    AddImage(thirdDigit, 0);
                }

                AddImage(secondDigit, 1);
                AddImage(firstDigit, 2);
            }
            else
            {
                DefaultView();
            }
        }

        private void DefaultView()
        {
            for (int i = 0; i < 3; i++)
            {
                AddImage(NumberType.Empty, i);
            }
        }

        private void AddImage(int digit, int columnIndex)
        {
            AddImage((NumberType)Enum.Parse(typeof(NumberType), $"N{Math.Abs(digit)}"), columnIndex);
        }

        private void AddImage(NumberType numberType, int columnIndex)
        {
            var image = GetCroppedBitmap(numberType);
            Children.Add(image);
            SetColumn(image, columnIndex);
        }

        private void ClearView()
        {
            foreach (var child in Children)
            {
                RemoveLogicalChild(child);
            }
        }
    }
}