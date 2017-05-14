using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

/*
 * This was taken directly from MaterialDesignThemes.Wpf
 * All credit for this goes to James.
 **/

namespace LandmarkDevs.UI.Material.Controls
{
    /// <summary>
    /// Enum ShadowDepth
    /// </summary>
    public enum ShadowDepth
    {
        /// <summary>
        /// Depth 0
        /// </summary>
        Depth0,
        /// <summary>
        /// Depth 1
        /// </summary>
        Depth1,
        /// <summary>
        /// Depthh 2
        /// </summary>
        Depth2,
        /// <summary>
        /// Depth 3
        /// </summary>
        Depth3,
        /// <summary>
        /// Depth 4
        /// </summary>
        Depth4,
        /// <summary>
        /// Depth 5
        /// </summary>
        Depth5
    }

    /// <summary>
    /// Class ShadowLocalInfo.
    /// </summary>
    internal class ShadowLocalInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShadowLocalInfo"/> class.
        /// </summary>
        /// <param name="standardOpacity">The standard opacity.</param>
        public ShadowLocalInfo(double standardOpacity)
        {
            StandardOpacity = standardOpacity;
        }

        /// <summary>
        /// Gets the standard opacity.
        /// </summary>
        /// <value>The standard opacity.</value>
        public double StandardOpacity { get; }
    }

    /// <summary>
    /// Class ShadowAssist.
    /// </summary>
    public static class ShadowAssist
    {
        /// <summary>
        /// The shadow depth property
        /// </summary>
        public static readonly DependencyProperty ShadowDepthProperty = DependencyProperty.RegisterAttached(
            "ShadowDepth", typeof(ShadowDepth), typeof(ShadowAssist), new FrameworkPropertyMetadata(default(ShadowDepth), FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// Sets the shadow depth.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetShadowDepth(DependencyObject element, ShadowDepth value)
        {
            element.SetValue(ShadowDepthProperty, value);
        }

        /// <summary>
        /// Gets the shadow depth.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>ShadowDepth.</returns>
        public static ShadowDepth GetShadowDepth(DependencyObject element)
        {
            return (ShadowDepth)element.GetValue(ShadowDepthProperty);
        }

        /// <summary>
        /// The local information property key
        /// </summary>
        private static readonly DependencyPropertyKey LocalInfoPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "LocalInfo", typeof(ShadowLocalInfo), typeof(ShadowAssist), new PropertyMetadata(default(ShadowLocalInfo)));

        /// <summary>
        /// Sets the local information.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        private static void SetLocalInfo(DependencyObject element, ShadowLocalInfo value)
        {
            element.SetValue(LocalInfoPropertyKey, value);
        }

        /// <summary>
        /// Gets the local information.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>ShadowLocalInfo.</returns>
        private static ShadowLocalInfo GetLocalInfo(DependencyObject element)
        {
            return (ShadowLocalInfo)element.GetValue(LocalInfoPropertyKey.DependencyProperty);
        }

        /// <summary>
        /// The darken property
        /// </summary>
        public static readonly DependencyProperty DarkenProperty = DependencyProperty.RegisterAttached(
            "Darken", typeof(bool), typeof(ShadowAssist), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.AffectsRender, DarkenPropertyChangedCallback));

        /// <summary>
        /// Darkens the property changed callback.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="dependencyPropertyChangedEventArgs">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void DarkenPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var uiElement = dependencyObject as UIElement;
            var dropShadowEffect = uiElement?.Effect as DropShadowEffect;

            if (dropShadowEffect == null) return;

            if ((bool)dependencyPropertyChangedEventArgs.NewValue)
            {
                SetLocalInfo(dependencyObject, new ShadowLocalInfo(dropShadowEffect.Opacity));

                var doubleAnimation = new DoubleAnimation(1, new Duration(TimeSpan.FromMilliseconds(350)))
                {
                    FillBehavior = FillBehavior.HoldEnd
                };
                dropShadowEffect.BeginAnimation(DropShadowEffect.OpacityProperty, doubleAnimation);
            }
            else
            {
                var shadowLocalInfo = GetLocalInfo(dependencyObject);
                if (shadowLocalInfo == null) return;

                var doubleAnimation = new DoubleAnimation(shadowLocalInfo.StandardOpacity, new Duration(TimeSpan.FromMilliseconds(350)))
                {
                    FillBehavior = FillBehavior.HoldEnd
                };
                dropShadowEffect.BeginAnimation(DropShadowEffect.OpacityProperty, doubleAnimation);
            }
        }

        /// <summary>
        /// Sets the darken.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetDarken(DependencyObject element, bool value)
        {
            element.SetValue(DarkenProperty, value);
        }

        /// <summary>
        /// Gets the darken.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool GetDarken(DependencyObject element)
        {
            return (bool)element.GetValue(DarkenProperty);
        }

        /// <summary>
        /// The cache mode property
        /// </summary>
        public static readonly DependencyProperty CacheModeProperty = DependencyProperty.RegisterAttached(
            "CacheMode", typeof(CacheMode), typeof(ShadowAssist), new FrameworkPropertyMetadata(new BitmapCache { EnableClearType = true, SnapsToDevicePixels = true }, FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// Sets the cache mode.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetCacheMode(DependencyObject element, CacheMode value)
        {
            element.SetValue(CacheModeProperty, value);
        }

        /// <summary>
        /// Gets the cache mode.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>CacheMode.</returns>
        public static CacheMode GetCacheMode(DependencyObject element)
        {
            return (CacheMode)element.GetValue(CacheModeProperty);
        }

    }
}
