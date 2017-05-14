#region
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

#endregion

namespace LandmarkDevs.UI.Material.Controls.Panels
{
    /// <summary>
    ///     Class MaterialPanel.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.ContentControl" />
    [TemplatePart(Name = PART_LayoutRoot, Type = typeof(Grid))]
    [TemplatePart(Name = PART_PaintArea, Type = typeof(Rectangle))]
    [TemplatePart(Name = PART_MainContentPresenter, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = PART_MainGrid, Type = typeof(Grid))]
    [TemplatePart(Name = PART_HiddenContent, Type = typeof(ContentControl))]
    public class MaterialPanel : ContentControl
    {
        static MaterialPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialPanel),
                new FrameworkPropertyMetadata(typeof(MaterialPanel)));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MaterialPanel" /> class.
        /// </summary>
        public MaterialPanel()
        {
            RenderTransform = new TranslateTransform();
        }

        /// <summary>
        ///     When overridden in a derived class, is invoked whenever application code or internal processes call
        ///     <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            LayoutRoot = GetTemplateChild(PART_LayoutRoot) as Grid;
            PaintArea = GetTemplateChild(PART_PaintArea) as Rectangle;
            MainContentPresenter = GetTemplateChild(PART_MainContentPresenter) as ContentPresenter;
            MainGrid = GetTemplateChild(PART_MainGrid) as Grid;
            HiddenContent = GetTemplateChild(PART_HiddenContent) as ContentControl;
        }

        #region Template Properties
        // ReSharper disable InconsistentNaming
        private const string PART_LayoutRoot = "PART_LayoutRoot";
        private const string PART_PaintArea = "PART_PaintArea";
        private const string PART_MainContentPresenter = "PART_MainContentPresenter";
        private const string PART_MainGrid = "PART_MainGrid";
        private const string PART_HiddenContent = "PART_HiddenContent";
        // ReSharper restore InconsistentNaming

        /// <summary>
        ///     The layout root
        /// </summary>
        internal Grid LayoutRoot;

        /// <summary>
        ///     The paint area
        /// </summary>
        internal Rectangle PaintArea;

        /// <summary>
        ///     The main content presenter
        /// </summary>
        internal ContentPresenter MainContentPresenter;

        /// <summary>
        ///     The main grid
        /// </summary>
        internal Grid MainGrid;

        /// <summary>
        ///     The hidden content
        /// </summary>
        internal ContentControl HiddenContent;
        #endregion

        #region Methods
        /// <summary>
        ///     Called when the <see cref="P:System.Windows.Controls.ContentControl.Content" /> property changes.
        /// </summary>
        /// <param name="oldContent">The old value of the <see cref="P:System.Windows.Controls.ContentControl.Content" /> property.</param>
        /// <param name="newContent">The new value of the <see cref="P:System.Windows.Controls.ContentControl.Content" /> property.</param>
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            if (oldContent != null && newContent != null)
            {
                HiddenContent.Content = newContent;
                var newSize = new Size();
                var newElement = (FrameworkElement) newContent;
                var oldElement = (FrameworkElement) oldContent;
                newElement.Measure(newSize);
                if (Math.Abs(newElement.DesiredSize.Width) < 1E-15)
                {
                    Arrange(new Rect(new Size(ActualWidth, ActualHeight)));
                    UpdateLayout();
                }
                var oldSize = oldElement.DesiredSize;
                newSize = newElement.DesiredSize;
                HiddenContent.Content = null;
                PaintArea.Fill = GetVisualBrush(LayoutRoot);
                PaintArea.Width = oldSize.Width;
                PaintArea.Height = oldSize.Height;
                PaintArea.Visibility = Visibility.Visible;
                MainContentPresenter.Visibility = Visibility.Collapsed;
                var sb = CreateOpacityChangeStoryboard(1, 0, TimeSpan.FromMilliseconds(100), PaintArea);
                sb.Completed += (s, e) =>
                {
                    var sb1 = CreateMaterialTransformationStoryboard(oldSize, newSize, TimeSpan.FromMilliseconds(250),
                        LayoutRoot);
                    sb1.Completed += (s1, e1) =>
                    {
                        var sb2 = CreateOpacityChangeStoryboard(0, 1, TimeSpan.FromMilliseconds(100),
                            MainContentPresenter);
                        sb2.Completed += (s2, e2) => { PaintArea.Opacity = 1; };
                        sb2.Begin(this);
                        PaintArea.Visibility = Visibility.Collapsed;
                        MainContentPresenter.Visibility = Visibility.Visible;
                    };
                    sb1.Begin(this);
                };
                sb.Begin(this);
            }
            base.OnContentChanged(oldContent, newContent);
        }

        private Storyboard CreateMaterialTransformationStoryboard(Size from, Size to, TimeSpan duration,
            DependencyObject element)
        {
            var daX = new DoubleAnimationUsingKeyFrames {Duration = duration};
            daX.KeyFrames.Add(new SplineDoubleKeyFrame
            {
                KeyTime = TimeSpan.FromTicks(0),
                KeySpline = new KeySpline(0.4, 0.0, 0.2, 1),
                Value = from.Width
            });
            daX.KeyFrames.Add(new SplineDoubleKeyFrame
            {
                KeyTime = duration,
                KeySpline = new KeySpline(0.4, 0.0, 0.2, 1),
                Value = to.Width
            });
            Storyboard.SetTarget(daX, element);
            Storyboard.SetTargetProperty(daX, new PropertyPath("Width", null));

            var daY = new DoubleAnimationUsingKeyFrames {Duration = duration};
            daY.KeyFrames.Add(new SplineDoubleKeyFrame
            {
                KeyTime = TimeSpan.FromTicks(0),
                KeySpline = new KeySpline(0.4, 0.0, 0.2, 1),
                Value = from.Height
            });
            daY.KeyFrames.Add(new SplineDoubleKeyFrame
            {
                KeyTime = duration,
                KeySpline = new KeySpline(0.4, 0.0, 0.2, 1),
                Value = to.Height
            });
            Storyboard.SetTarget(daY, element);
            Storyboard.SetTargetProperty(daY, new PropertyPath("Height", null));

            var sb = new Storyboard();
            sb.Children.Add(daX);
            sb.Children.Add(daY);
            return sb;
        }

        private Storyboard CreateOpacityChangeStoryboard(double from, double to, TimeSpan duration,
            DependencyObject element)
        {
            var da = new DoubleAnimation
            {
                Duration = duration,
                From = from,
                To = to
            };
            Storyboard.SetTarget(da, element);
            Storyboard.SetTargetProperty(da, new PropertyPath("Opacity", null));

            var sb = new Storyboard();
            sb.Children.Add(da);
            return sb;
        }

        private Brush GetVisualBrush(Visual visual)
        {
            var target = new RenderTargetBitmap((int) ActualWidth, (int) ActualHeight, 96, 96, PixelFormats.Pbgra32);
            target.Render(visual);
            var brush = new ImageBrush(target);
            brush.Freeze();
            return brush;
        }
        #endregion
    }
}