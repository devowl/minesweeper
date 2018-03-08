using System;
using System.Windows;

namespace Minesweeper.Common
{
    /// <summary>
    /// Shared methods.
    /// </summary>
    public static class SharedUtils
    {
        /// <summary>
        /// Is design control preview mode.
        /// </summary>
        /// <returns></returns>
        public static bool InDesignMode()
        {
            return !(Application.Current is App);
        }
        
        /// <summary>
        /// ButtonType uri.
        /// </summary>
        public static readonly Uri ButtonTypeUri = new Uri("pack://application:,,,/Images/ButtonTypes.bmp", UriKind.RelativeOrAbsolute);

        /// <summary>
        /// Emotions uri.
        /// </summary>
        public static readonly Uri EmotionsUri = new Uri("pack://application:,,,/Images/Emotions.bmp", UriKind.RelativeOrAbsolute);

        /// <summary>
        /// Numbers uri.
        /// </summary>
        public static readonly Uri NumbersUri = new Uri("pack://application:,,,/Images/Numbers.bmp", UriKind.RelativeOrAbsolute);
    }
}