using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LandmarkDevs.UI.Material.Controls.Panels
{
    /// <summary>
    /// A card is a content control, styled according to Material Design guidelines.
    /// This was taken directly from MaterialDesignThemes.Wpf
    /// All credit for this goes to James.
    /// </summary>
    [TemplatePart(Name = ClipBorderPartName, Type = typeof(Border))]
    public class Card : ContentControl
    {
        /// <summary>
        /// The clip border part name
        /// </summary>
        public const string ClipBorderPartName = "PART_ClipBorder";

        private Border _clipBorder;

        /// <summary>
        /// Initializes static members of the <see cref="Card"/> class.
        /// </summary>
        static Card()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Card), new FrameworkPropertyMetadata(typeof(Card)));
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _clipBorder = Template.FindName(ClipBorderPartName, this) as Border;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.FrameworkElement.SizeChanged" /> event, using the specified information as part of the eventual event data.
        /// </summary>
        /// <param name="sizeInfo">Details of the old and new size involved in the change.</param>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            if (_clipBorder == null) return;

            var farPoint = new Point(
                Math.Max(0, _clipBorder.ActualWidth),
                Math.Max(0, _clipBorder.ActualHeight));

            var clipRect = new Rect(
                new Point(),
                new Point(farPoint.X, farPoint.Y));

            ContentClip = new RectangleGeometry(clipRect, UniformCornerRadius, UniformCornerRadius);
        }

        /// <summary>
        /// The uniform corner radius property
        /// </summary>
        public static readonly DependencyProperty UniformCornerRadiusProperty = DependencyProperty.Register(
            nameof(UniformCornerRadius), typeof(double), typeof(Card), new FrameworkPropertyMetadata(2.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets or sets the uniform corner radius.
        /// </summary>
        /// <value>The uniform corner radius.</value>
        public double UniformCornerRadius
        {
            get { return (double)GetValue(UniformCornerRadiusProperty); }
            set { SetValue(UniformCornerRadiusProperty, value); }
        }

        private static readonly DependencyPropertyKey ContentClipPropertyKey =
            DependencyProperty.RegisterReadOnly(
                "ContentClip", typeof(Geometry), typeof(Card),
                new PropertyMetadata(default(Geometry)));

        /// <summary>
        /// The content clip property
        /// </summary>
        public static readonly DependencyProperty ContentClipProperty =
            ContentClipPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the content clip.
        /// </summary>
        /// <value>The content clip.</value>
        public Geometry ContentClip
        {
            get { return (Geometry)GetValue(ContentClipProperty); }
            private set { SetValue(ContentClipPropertyKey, value); }
        }
    }
}
