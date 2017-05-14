using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
// ReSharper disable InconsistentNaming

namespace LandmarkDevs.UI.Material.Controls
{
    /// <summary>
    /// Class YcaKite.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.ContentControl" />
    [TemplatePart(Name = PART_LayoutRoot, Type = typeof(Grid))]
    [TemplatePart(Name = PART_KiteStoryboard, Type = typeof(Storyboard))]
    public class YcaKite : ContentControl
    {
        static YcaKite()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(YcaKite), new FrameworkPropertyMetadata(typeof(YcaKite)));
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        /// <exception cref="System.Exception">KiteAnimation is null.</exception>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            LayoutRoot = GetTemplateChild(PART_LayoutRoot) as Grid;
            KiteAnimationStoryboard = GetTemplateChild(PART_KiteStoryboard) as Storyboard;
        }

        #region Template Properties
        private const string PART_LayoutRoot = "PART_LayoutRoot";
        private const string PART_KiteStoryboard = "PART_KiteStoryboard";
        internal Grid LayoutRoot;
        internal Storyboard KiteAnimationStoryboard;
        #endregion

        #region Dependency Properties        
        /// <summary>
        /// The kite width property
        /// </summary>
        public static readonly DependencyProperty KiteWidthProperty = DependencyProperty.Register(
            "KiteWidth", typeof(double), typeof(YcaKite), new PropertyMetadata(0.0));

        /// <summary>
        /// Gets or sets the width of the kite.
        /// </summary>
        /// <value>The width of the kite.</value>
        public double KiteWidth
        {
            get { return (double) GetValue(KiteWidthProperty); }
            set { SetValue(KiteWidthProperty, value); }
        }

        /// <summary>
        /// The kite height property
        /// </summary>
        public static readonly DependencyProperty KiteHeightProperty = DependencyProperty.Register(
            "KiteHeight", typeof(double), typeof(YcaKite), new PropertyMetadata(0.0));

        /// <summary>
        /// Gets or sets the height of the kite.
        /// </summary>
        /// <value>The height of the kite.</value>
        public double KiteHeight
        {
            get { return (double) GetValue(KiteHeightProperty); }
            set { SetValue(KiteHeightProperty, value); }
        }

        /// <summary>
        /// The is active property
        /// </summary>
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(
            "IsActive", typeof(bool), typeof(YcaKite), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsActiveChanged));

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        private static void IsActiveChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;
            var kite = sender as YcaKite;
            if (kite == null)
                return;
            var isActiveState = (bool) e.NewValue;
            kite.ChangeVisualState(isActiveState);
        }

        private void ChangeVisualState(bool isActiveState)
        {
            if (isActiveState)
            {
                var vsg = LayoutRoot == null ? null : VisualStateManager.GetVisualStateGroups(LayoutRoot);
                var vsgs = vsg?.OfType<VisualStateGroup>().FirstOrDefault(c => c.Name == "KiteStates");
                if (vsgs == null)
                    return;
                var state = vsgs.States.OfType<VisualState>().FirstOrDefault(c => c.Name == "ActiveState");
                var sb = state?.Storyboard;
                VisualStateManager.GoToState(this, "ActiveState", true);
                if (sb == null)
                    return;
                sb.Begin(LayoutRoot, true);
            }
            else
            {
                var vsg = LayoutRoot == null ? null : VisualStateManager.GetVisualStateGroups(LayoutRoot);
                var vsgs = vsg?.OfType<VisualStateGroup>().FirstOrDefault(c => c.Name == "KiteStates");
                if (vsgs == null)
                    return;
                var state = vsgs.States.OfType<VisualState>().FirstOrDefault(c => c.Name == "ActiveState");
                var sb = state?.Storyboard;
                VisualStateManager.GoToState(this, "InActiveState", true);
                if (sb == null)
                    return;
                sb.Stop(LayoutRoot);
            }
        }
        #endregion

    }
}
