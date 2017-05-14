#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

#endregion

namespace LandmarkDevs.UI.Material.Controls.Panels
{
    /// <summary>
    ///     Class TransitionCard.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.ContentControl" />
    [TemplatePart(Name = PART_ContentGrid, Type = typeof(Grid))]
    [TemplatePart(Name = PART_CurrentContent, Type = typeof(ContentControl))]
    [TemplatePart(Name = PART_PreviousContent, Type = typeof(ContentControl))]
    public class TransitionCard : ContentControl
    {
        static TransitionCard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TransitionCard),
                new FrameworkPropertyMetadata(typeof(TransitionCard)));
        }

        /// <summary>
        ///     When overridden in a derived class, is invoked whenever application code or internal processes call
        ///     <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            CurrentContent = GetTemplateChild(PART_PreviousContent) as ContentControl;
            PreviousContent = GetTemplateChild(PART_CurrentContent) as ContentControl;
            base.OnApplyTemplate();
            ContentGrid = GetTemplateChild(PART_ContentGrid) as Grid;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TransitionCard" /> class.
        /// </summary>
        public TransitionCard()
        {
            ActualSizes = new Dictionary<Type, Size>();
            DesiredSizes = new Dictionary<Type, Size>();
        }

        /// <summary>
        ///     The old size
        /// </summary>
        internal Size OldSize;

        /// <summary>
        ///     The new size
        /// </summary>
        internal Size NewSize;

        /// <summary>
        ///     The actual sizes
        /// </summary>
        internal Dictionary<Type, Size> ActualSizes;

        /// <summary>
        ///     The desired sizes
        /// </summary>
        internal Dictionary<Type, Size> DesiredSizes;


        /// <summary>
        ///     Called when the <see cref="P:System.Windows.Controls.ContentControl.Content" /> property changes.
        /// </summary>
        /// <param name="oldContent">The old value of the <see cref="P:System.Windows.Controls.ContentControl.Content" /> property.</param>
        /// <param name="newContent">The new value of the <see cref="P:System.Windows.Controls.ContentControl.Content" /> property.</param>
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            if (PreviousContent != null && CurrentContent != null)
            {
                CurrentContent.Content = oldContent;
                PreviousContent.Content = newContent;
                OldSize = NewSize;

                var oldContentType = oldContent.GetType();
                var newContentType = newContent.GetType();

                if (!DesiredSizes.Keys.Contains(oldContentType))
                {
                    DesiredSizes.Add(oldContentType, ((FrameworkElement) oldContent).DesiredSize);
                }
                if (!DesiredSizes.Keys.Contains(newContentType))
                {
                    DesiredSizes.Add(newContentType, ((FrameworkElement) newContent).DesiredSize);
                }
                if (!ActualSizes.Keys.Contains(oldContentType))
                {
                    ActualSizes.Add(oldContentType,
                        new Size(((FrameworkElement) oldContent).ActualWidth,
                            ((FrameworkElement) oldContent).ActualHeight));
                }
                if (!ActualSizes.Keys.Contains(newContentType))
                {
                    ActualSizes.Add(newContentType,
                        new Size(((FrameworkElement) newContent).ActualWidth,
                            ((FrameworkElement) newContent).ActualHeight));
                }
                ActualSizes.TryGetValue(newContentType, out NewSize);
                CreateFade(1, 0, (FrameworkElement) CurrentContent.Content);
                CreateResize(CurrentContent, OldSize);
                CreateResize(PreviousContent, NewSize);
                CreateFade(0, 1, (FrameworkElement) PreviousContent.Content);
            }
            else if (oldContent == null && newContent != null)
            {
                var e = (FrameworkElement) newContent;
                e.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                e.Arrange(new Rect(0, 0, e.DesiredSize.Width, e.DesiredSize.Height));
                NewSize = new Size(e.DesiredSize.Width, e.DesiredSize.Height);
            }
        }

        private void CreateResize(DependencyObject sender, Size newSize)
        {
            var element = (FrameworkElement) sender;
            var sb = new Storyboard();
            var newX = new DoubleAnimationUsingKeyFrames();
            var newY = new DoubleAnimationUsingKeyFrames();
            var duration = TimeSpan.FromMilliseconds(250);
            sb.Duration = duration;
            newX.KeyFrames.Add(new SplineDoubleKeyFrame
            {
                KeyTime = duration,
                KeySpline = new KeySpline(0.4, 0, 1, 1),
                Value = newSize.Width
            });
            newY.KeyFrames.Add(new SplineDoubleKeyFrame
            {
                KeyTime = duration,
                KeySpline = new KeySpline(0.4, 0, 1, 1),
                Value = newSize.Height
            });
            sb.Children.Add(newX);
            sb.Children.Add(newY);
            Storyboard.SetTarget(newX, element);
            Storyboard.SetTarget(newY, element);
            Storyboard.SetTargetProperty(newX, new PropertyPath("(FrameworkElement.Width)", null));
            Storyboard.SetTargetProperty(newY, new PropertyPath("(FrameworkElement.Height)", null));
            sb.Begin();
        }

        private void CreateFade(double from, double to, DependencyObject element)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.Duration = new Duration(TimeSpan.FromMilliseconds(10));
            da.From = from;
            da.To = to;
            Storyboard.SetTarget(da, element);
            Storyboard.SetTargetProperty(da, new PropertyPath("Opacity", null));
            Storyboard sb = new Storyboard();
            sb.Children.Add(da);
            sb.Begin();
        }

        #region Template Properties
        private const string PART_ContentGrid = "PART_ContentGrid";
        private const string PART_CurrentContent = "PART_CurrentContent";
        private const string PART_PreviousContent = "PART_PreviousContent";

        /// <summary>
        ///     The current content
        /// </summary>
        internal ContentControl CurrentContent;

        /// <summary>
        ///     The previous content
        /// </summary>
        internal ContentControl PreviousContent;

        /// <summary>
        ///     The content grid
        /// </summary>
        internal Grid ContentGrid;
        #endregion

        #region Dependency Properties
        /// <summary>
        ///     The automatic size property
        /// </summary>
        public static readonly DependencyProperty AutoSizeProperty = DependencyProperty.Register(
            "AutoSize", typeof(bool), typeof(TransitionCard), new FrameworkPropertyMetadata(false, (s, e) =>
            {
                // Transition somewhere here.
                var element = (FrameworkElement) s;
                if ((bool) e.NewValue)
                {
                    element.Width = Double.NaN;
                    element.Height = Double.NaN;
                }
                else
                {
                    var grid = (TransitionCard) element;
                    element.Width = !Double.IsNaN(grid.GridWidth) ? grid.GridWidth : element.DesiredSize.Width;
                    element.Height = !Double.IsNaN(grid.GridHeight) ? grid.GridHeight : element.DesiredSize.Height;
                }
            }));

        /// <summary>
        ///     Gets or sets a value indicating whether [automatic size].
        /// </summary>
        /// <value><c>true</c> if [automatic size]; otherwise, <c>false</c>.</value>
        public bool AutoSize
        {
            get { return (bool) GetValue(AutoSizeProperty); }
            set { SetValue(AutoSizeProperty, value); }
        }

        /// <summary>
        ///     The grid width property
        /// </summary>
        public static readonly DependencyProperty GridWidthProperty = DependencyProperty.Register(
            "GridWidth", typeof(Double), typeof(TransitionCard), new FrameworkPropertyMetadata(Double.NaN, (s, e) =>
            {
                var p = (TransitionCard) s;
                if (p.AutoSize)
                    return;
                var element = (FrameworkElement) s;
                var newVal = (Double) e.NewValue;
                element.Width = !Double.IsNaN(newVal) ? newVal : Double.NaN;
            }));

        /// <summary>
        ///     Gets or sets the width of the grid.
        /// </summary>
        /// <value>The width of the grid.</value>
        public Double GridWidth
        {
            get { return (Double) GetValue(GridWidthProperty); }
            set { SetValue(GridWidthProperty, value); }
        }

        /// <summary>
        ///     The grid height property
        /// </summary>
        public static readonly DependencyProperty GridHeightProperty = DependencyProperty.Register(
            "GridHeight", typeof(Double), typeof(TransitionCard), new FrameworkPropertyMetadata(Double.NaN, (s, e) =>
            {
                var p = (TransitionCard) s;
                if (p.AutoSize)
                    return;
                var element = (FrameworkElement) s;
                var newVal = (Double) e.NewValue;
                element.Height = !Double.IsNaN(newVal) ? newVal : Double.NaN;
            }));

        /// <summary>
        ///     Gets or sets the height of the grid.
        /// </summary>
        /// <value>The height of the grid.</value>
        public Double GridHeight
        {
            get { return (Double) GetValue(GridHeightProperty); }
            set { SetValue(GridHeightProperty, value); }
        }
        #endregion
    }
}