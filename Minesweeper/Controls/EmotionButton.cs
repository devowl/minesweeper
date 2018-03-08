using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Minesweeper.Common;
using Minesweeper.Data;

namespace Minesweeper.Controls
{
    /// <summary>
    /// Emotion button.
    /// </summary>
    public class EmotionButton : Button, INotifyPropertyChanged
    {
        private const int ImageSize = 24;

        /// <summary>
        /// <see cref="EmotionTypeValue"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EmotionTypeValueProperty;

        /// <summary>
        /// <see cref="ParentWindow"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ParentWindowProperty;

        private static readonly BitmapSource EmotionsImageSource;

        private ImageSource _imageSourceBefore;

        private ImageSource _imageSource;

        static EmotionButton()
        {
            if (SharedUtils.InDesignMode())
            {
                return;
            }

            EmotionsImageSource = new BitmapImage(SharedUtils.EmotionsUri);
            ParentWindowProperty = DependencyProperty.Register(
                nameof(ParentWindow),
                typeof(Window),
                typeof(EmotionButton),
                new PropertyMetadata(default(Window), (source, args) => { ((EmotionButton)source).ParentChanged(); }));

            EmotionTypeValueProperty = DependencyProperty.Register(
                nameof(EmotionTypeValue),
                typeof(EmotionType),
                typeof(EmotionButton),
                new PropertyMetadata(
                    default(EmotionType),
                    (source, args) => { ((EmotionButton)source).EmotionTypeChanged(); }));
        }

        /// <summary>
        /// <see cref="EmotionButton"/> constructor.
        /// </summary>
        public EmotionButton()
        {
            Template = CreateButtonTemplate();
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Button emotion type.
        /// </summary>
        public EmotionType EmotionTypeValue
        {
            get
            {
                return (EmotionType)GetValue(EmotionTypeValueProperty);
            }
            set
            {
                SetValue(EmotionTypeValueProperty, value);
            }
        }

        /// <summary>
        /// Button image source.
        /// </summary>
        public ImageSource ImageSource
        {
            get
            {
                return _imageSource;
            }

            set
            {
                _imageSource = value;
                OnPropertyChanged();
            }
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
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static ImageSource GetEmotionImageSource(EmotionType emotionTypeValue)
        {
            int offset = (int)emotionTypeValue;
            return new CroppedBitmap(EmotionsImageSource, new Int32Rect(0, offset * ImageSize, ImageSize, ImageSize));
        }

        private ControlTemplate CreateButtonTemplate()
        {
            var imageFactory = new FrameworkElementFactory(typeof(Image), "EmotionImage");
            imageFactory.SetBinding(
                Image.SourceProperty,
                new Binding(nameof(ImageSource))
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(EmotionButton), 1)
                });

            var controlTemplate = new ControlTemplate { VisualTree = imageFactory };

            var pressedTrigger = new Trigger { Property = IsPressedProperty, Value = true };
            pressedTrigger.Setters.Add(new Setter()
            {
                TargetName = "EmotionImage",
                Property = Image.SourceProperty,
                Value = GetEmotionImageSource(EmotionType.Pressed)
            });

            controlTemplate.Triggers.Add(pressedTrigger);
            return controlTemplate;
        }

        private void EmotionTypeChanged()
        {
            ImageSource = GetEmotionImageSource(EmotionTypeValue);
        }

        private void ParentChanged()
        {
            if (ParentWindow != null)
            {
                ParentWindow.PreviewMouseLeftButtonDown += (sender, args) => MousePressed();
                ParentWindow.PreviewMouseLeftButtonUp += (sender, args) => MouseReleased();
                ParentWindow.Deactivated += (sender, args) => MouseReleased();
                ParentWindow.MouseLeave += (sender, args) => MouseReleased();
            }
        }

        private void MousePressed()
        {
            if (!IsFinilizedState)
            {
                _imageSourceBefore = ImageSource;
                ImageSource = GetEmotionImageSource(EmotionType.PressDown);
            }
        }

        private void MouseReleased()
        {
            if (_imageSourceBefore != null && !IsFinilizedState)
            {
                ImageSource = _imageSourceBefore;
            }
        }

        private bool IsFinilizedState
        {
            get
            {
                return EmotionTypeValue == EmotionType.Lose || EmotionTypeValue == EmotionType.Win;
            }
        }
    }
}