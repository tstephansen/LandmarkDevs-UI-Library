using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using MaterialDesignThemes.Wpf;

namespace LandmarkDevs.UI.Material.Controls
{
    /// <summary>
    /// Class SizingContentControl.
    /// Automatically sizes to the content it contains and transitions between the sizes.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.ContentControl" />
    [TemplatePart(Name = PART_ContentCard, Type = typeof(Card))]
    public class SizingContentControl : ContentControl
    {
        /// <summary>
        /// Initializes static members of the <see cref="SizingContentControl"/> class.
        /// </summary>
        static SizingContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SizingContentControl), new FrameworkPropertyMetadata(typeof(SizingContentControl)));
            ContentProperty.OverrideMetadata(typeof(SizingContentControl), new FrameworkPropertyMetadata(OnContentChanged));
        }
        
        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _contentCard = GetTemplateChild(PART_ContentCard) as Card;

        }

        /// <summary>
        /// Called to remeasure a control.
        /// </summary>
        /// <param name="constraint">The maximum size that the method can return.</param>
        /// <returns>The size of the control, up to the maximum specified by <paramref name="constraint" />.</returns>
        protected override Size MeasureOverride(Size constraint)
        {
            if (DesignerProperties.GetIsInDesignMode(this))
                return base.MeasureOverride(constraint);
            var overriddenSize = base.MeasureOverride(constraint);
            var totalSize = new Size(Application.Current.MainWindow.ActualWidth, Application.Current.MainWindow.ActualHeight);
            if (_contentCard == null) return overriddenSize;
            var availableSize = totalSize;
            if (!_contentCard.IsMeasureValid)
                _contentCard.Measure(availableSize);
            var contentCardDesiredSize = _contentCard.DesiredSize;
            _contentCard.MinHeight = contentCardDesiredSize.Height;
            _contentCard.MinWidth = contentCardDesiredSize.Width;
            return overriddenSize;
        }

        #region Template Properties
        // ReSharper disable once InconsistentNaming
        private const string PART_ContentCard = "PART_ContentCard";
        private Card _contentCard;
        #endregion

        /// <summary>
        /// Handles the <see cref="E:ContentChanged" /> event.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnContentChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var control = obj as SizingContentControl;
            var oldCon = e.OldValue;
            var newCon = e.NewValue;
            if (oldCon == null || newCon == null)
                return;
            var oldContent = oldCon as UserControl;
            var newContent = newCon as UserControl;
            if (oldContent == null || newContent == null)
                return;
            var availableSize = new Size(control.ActualWidth, control.ActualHeight);
            oldContent.Measure(availableSize);
            newContent.Measure(availableSize);
            var oldSize = new Size(oldContent.DesiredSize.Width, oldContent.DesiredSize.Height);
            var newSize = newContent.DesiredSize;
            var contentCard = control.GetContentCard();
            var sb = CreateSizeStoryboard(contentCard, oldSize, newSize, availableSize);
            sb?.Begin(control);
        }

        /// <summary>
        /// Gets the content card.
        /// </summary>
        /// <returns>Card.</returns>
        private Card GetContentCard()
        {
            return _contentCard;
        }

        /// <summary>
        /// Creates the size storyboard.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="oldSize">The old size.</param>
        /// <param name="newSize">The new size.</param>
        /// <param name="availableSize">Size of the available.</param>
        /// <returns>Storyboard.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// host
        /// or
        /// hostContent
        /// </exception>
        private static Storyboard CreateSizeStoryboard(Card host, Size oldSize, Size newSize, Size availableSize)
        {
            if (host == null)
                throw new ArgumentNullException(nameof(host));
            var hostContent = host.Content as DependencyObject;
            if (hostContent == null)
                throw new ArgumentNullException(nameof(hostContent));
            var widthAnimation = CreateWidthAnimation(oldSize, newSize, new KeySpline(0.4, 0, 0.2, 1));
            var heightAnimation = CreateHeightAnimation(oldSize, newSize, new KeySpline(0.4, 0, 0.2, 1));
            var opacityAnimation = CreateOpacityAnimation();
            if (oldSize.Width < newSize.Width)
            {
                host.MaxWidth = availableSize.Width;
                SetStoryboardTarget(host, widthAnimation, "(FrameworkElement.MaxWidth)");
            }
            else
            {
                host.MaxWidth = availableSize.Width;
                host.MinWidth = oldSize.Width;
                SetStoryboardTarget(host, widthAnimation, "(FrameworkElement.MinWidth)");
            }
            if (oldSize.Height < newSize.Height)
            {
                host.MaxHeight = availableSize.Height;
                SetStoryboardTarget(host, heightAnimation, "(FrameworkElement.MaxHeight)");
            }
            else
            {
                host.MaxHeight = availableSize.Height;
                host.MinHeight = oldSize.Height;
                SetStoryboardTarget(host, heightAnimation, "(FrameworkElement.MinHeight)");
            }
            SetStoryboardTarget(hostContent, opacityAnimation, new PropertyPath(OpacityProperty));
            var sb = new Storyboard();
            sb.Children.Add(widthAnimation);
            sb.Children.Add(heightAnimation);
            sb.Children.Add(opacityAnimation);
            return sb;
        }

        /// <summary>
        /// Creates the storyboard that transitions to a smaller size.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="oldSize">The old size.</param>
        /// <param name="newSize">The new size.</param>
        /// <returns>Storyboard.</returns>
        private static Storyboard CreateDownSizeStoryboard(Card host, Size oldSize, Size newSize)
        {
            if (host == null)
                throw new ArgumentNullException(nameof(host));
            var hostContent = host.Content as DependencyObject;
            if (hostContent == null)
                throw new ArgumentNullException(nameof(hostContent));
            // Create Animations
            var widthAnimation = CreateWidthAnimation(oldSize, newSize, new KeySpline(0.4, 0, 0.2, 1));
            var heightAnimation = CreateHeightAnimation(oldSize, newSize, new KeySpline(0.4, 0, 0.2, 1));
            var opacityAnimation = CreateOpacityAnimation();
            // Set Storyboard Targets
            SetStoryboardTarget(hostContent, opacityAnimation, new PropertyPath(OpacityProperty));
            SetStoryboardTarget(host, widthAnimation, "(FrameworkElement.MinWidth)");
            SetStoryboardTarget(host, heightAnimation, "(FrameworkElement.MinHeight)");
            // Create Storyboard
            var sb = new Storyboard();
            sb.Children.Add(widthAnimation);
            sb.Children.Add(heightAnimation);
            sb.Children.Add(opacityAnimation);
            return sb;
        }

        /// <summary>
        /// Creates the storyboard that transitions to a larger size.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="oldSize">The old size.</param>
        /// <param name="newSize">The new size.</param>
        /// <returns>Storyboard.</returns>
        private static Storyboard CreateExpandSizeStoryboard(Card host, Size oldSize, Size newSize)
        {
            if (host == null)
                throw new ArgumentNullException(nameof(host));
            var hostContent = host.Content as DependencyObject;
            if (hostContent == null)
                throw new ArgumentNullException(nameof(hostContent));
            // Create Animations
            var widthAnimation = CreateWidthAnimation(oldSize, newSize, new KeySpline(0.4, 0, 0.2, 1));
            var heightAnimation = CreateHeightAnimation(oldSize, newSize, new KeySpline(0.4, 0, 0.2, 1));
            var opacityAnimation = CreateOpacityAnimation();
            // Set Storyboard Targets
            SetStoryboardTarget(hostContent, opacityAnimation, new PropertyPath(OpacityProperty));
            SetStoryboardTarget(host, widthAnimation, "(FrameworkElement.MaxWidth)");
            SetStoryboardTarget(host, heightAnimation, "(FrameworkElement.MaxHeight)");
            // Create Storyboard
            var sb = new Storyboard();
            sb.Children.Add(widthAnimation);
            sb.Children.Add(heightAnimation);
            sb.Children.Add(opacityAnimation);
            return sb;
        }

        /// <summary>
        /// Sets the storyboard target.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="anim">The anim.</param>
        /// <param name="propertyPath">The property path.</param>
        private static void SetStoryboardTarget(DependencyObject host, DoubleAnimationUsingKeyFrames anim, string propertyPath)
        {
            Storyboard.SetTargetProperty(anim, new PropertyPath(propertyPath, null));
            Storyboard.SetTarget(anim, host);
        }

        /// <summary>
        /// Sets the storyboard target.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="anim">The anim.</param>
        /// <param name="propertyPath">The property path.</param>
        private static void SetStoryboardTarget(DependencyObject host, DoubleAnimationUsingKeyFrames anim, PropertyPath propertyPath)
        {
            Storyboard.SetTargetProperty(anim, propertyPath);
            Storyboard.SetTarget(anim, host);
        }

        /// <summary>
        /// Creates the width animation.
        /// </summary>
        /// <param name="oldSize">The old size.</param>
        /// <param name="newSize">The new size.</param>
        /// <param name="keySpline">The key spline.</param>
        /// <returns>DoubleAnimationUsingKeyFrames.</returns>
        private static DoubleAnimationUsingKeyFrames CreateWidthAnimation(Size oldSize, Size newSize, KeySpline keySpline)
        {
            var widthAnimation = new DoubleAnimationUsingKeyFrames();
            widthAnimation.KeyFrames.Add(new SplineDoubleKeyFrame
            {
                KeyTime = TimeSpan.FromMilliseconds(0),
                KeySpline = keySpline,
                Value = oldSize.Width
            });
            widthAnimation.KeyFrames.Add(new SplineDoubleKeyFrame
            {
                KeyTime = TimeSpan.FromMilliseconds(250),
                KeySpline = keySpline,
                Value = newSize.Width
            });
            return widthAnimation;
        }

        /// <summary>
        /// Creates the height animation.
        /// </summary>
        /// <param name="oldSize">The old size.</param>
        /// <param name="newSize">The new size.</param>
        /// <param name="keySpline">The key spline.</param>
        /// <returns>DoubleAnimationUsingKeyFrames.</returns>
        private static DoubleAnimationUsingKeyFrames CreateHeightAnimation(Size oldSize, Size newSize, KeySpline keySpline)
        {
            var heightAnimation = new DoubleAnimationUsingKeyFrames();
            heightAnimation.KeyFrames.Add(new SplineDoubleKeyFrame
            {
                KeyTime = TimeSpan.FromMilliseconds(0),
                KeySpline = keySpline,
                Value = oldSize.Height
            });
            heightAnimation.KeyFrames.Add(new SplineDoubleKeyFrame
            {
                KeyTime = TimeSpan.FromMilliseconds(250),
                KeySpline = keySpline,
                Value = newSize.Height
            });
            return heightAnimation;
        }

        /// <summary>
        /// Creates the opacity animation.
        /// </summary>
        /// <returns>DoubleAnimationUsingKeyFrames.</returns>
        private static DoubleAnimationUsingKeyFrames CreateOpacityAnimation()
        {
            var opacityAnimation = new DoubleAnimationUsingKeyFrames();
            opacityAnimation.KeyFrames.Add(new SplineDoubleKeyFrame
            {
                KeyTime = TimeSpan.FromMilliseconds(0),
                KeySpline = new KeySpline(0.2, 0, 0.4, 1),
                Value = 0
            });
            opacityAnimation.KeyFrames.Add(new SplineDoubleKeyFrame
            {
                KeyTime = TimeSpan.FromMilliseconds(250),
                KeySpline = new KeySpline(0.2, 0, 0.4, 1),
                Value = 0
            });
            opacityAnimation.KeyFrames.Add(new SplineDoubleKeyFrame
            {
                KeyTime = TimeSpan.FromMilliseconds(350),
                KeySpline = new KeySpline(0.2, 0, 0.4, 1),
                Value = 1
            });
            return opacityAnimation;
        }
    }
}
