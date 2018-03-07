using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Minesweeper.Windows.Behaviors
{
    /// <summary>
    /// Behavior for emotion button.
    /// </summary>
    public class EmotionButtonBehavior : Behavior<Image>
    {
        private const int ImageSize = 24;

        /// <summary>
        /// <see cref="ParentWindow"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ParentWindowProperty;

        private ImageSource _imageSourceBefore;

        static EmotionButtonBehavior()
        {
            if (!(Application.Current is App))
            {
                return;
            }

            EmotionsImageSource =
                new BitmapImage(new Uri("pack://application:,,,/Images/Emotions.bmp", UriKind.RelativeOrAbsolute));
            ParentWindowProperty = DependencyProperty.Register(
                nameof(ParentWindow),
                typeof(Window),
                typeof(EmotionButtonBehavior),
                new PropertyMetadata(default(Window)));
        }

        /// <summary>
        /// Main window reference.
        /// </summary>
        public Window ParentWindow
        {
            get
            {
                return (Window)GetValue(ParentWindowProperty);
            }
            set
            {
                SetValue(ParentWindowProperty, value);
            }
        }

        private static BitmapImage EmotionsImageSource { get; }

        private static ImageSource MouseDown
            => new CroppedBitmap(EmotionsImageSource, new Int32Rect(0, 3 * ImageSize, ImageSize, ImageSize));

        /// <inheritdoc/>
        protected override void OnAttached()
        {
            base.OnAttached();
            ParentWindow.PreviewMouseLeftButtonDown += ParentWindowOnPreviewMouseLeftButtonDown;
            ParentWindow.PreviewMouseLeftButtonUp += ParentWindowOnPreviewMouseLeftButtonUp;
            ParentWindow.Deactivated += ParentWindowOnDeactivated;
        }

        /// <inheritdoc/>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            ParentWindow.PreviewMouseLeftButtonDown -= ParentWindowOnPreviewMouseLeftButtonDown;
            ParentWindow.PreviewMouseLeftButtonUp -= ParentWindowOnPreviewMouseLeftButtonUp;
            ParentWindow.Deactivated -= ParentWindowOnDeactivated;
        }

        private void ParentWindowOnDeactivated(object sender, EventArgs eventArgs)
        {
            if (_imageSourceBefore != null)
            {
                AssociatedObject.Source = _imageSourceBefore;
            }
        }

        private void ParentWindowOnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (_imageSourceBefore != null)
            {
                AssociatedObject.Source = _imageSourceBefore;
            }
        }

        private void ParentWindowOnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            _imageSourceBefore = AssociatedObject.Source;
            AssociatedObject.Source = MouseDown;
        }
    }
}