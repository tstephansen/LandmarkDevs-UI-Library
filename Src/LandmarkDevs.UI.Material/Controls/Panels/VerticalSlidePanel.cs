using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using LandmarkDevs.UI.Material.Helpers;

namespace LandmarkDevs.UI.Material.Controls.Panels
{
    /// <summary>
    /// Class VerticalSlidePanel.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.ContentControl" />
    public class VerticalSlidePanel : ContentControl
    {
        /// <summary>
        /// Initializes static members of the <see cref="VerticalSlidePanel"/> class.
        /// </summary>
        static VerticalSlidePanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VerticalSlidePanel), new FrameworkPropertyMetadata(typeof(VerticalSlidePanel)));
        }

        #region Expanded        
        /// <summary>
        /// The is expanded property
        /// </summary>
        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(VerticalSlidePanel), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsExpandedChanged));

        /// <summary>
        /// Gets or sets a value indicating whether this instance is expanded.
        /// </summary>
        /// <value><see langword="true" /> if this instance is expanded; otherwise, <see langword="false" />.</value>
        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        /// <summary>
        /// The is expanded changed event
        /// </summary>
        public static readonly RoutedEvent IsExpandedChangedEvent =
            EventManager.RegisterRoutedEvent("IsExpandChanged", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(VerticalSlidePanel));

        /// <summary>
        /// Occurs when [is expand changed].
        /// </summary>
        public event RoutedEventHandler IsExpandChanged
        {
            add { AddHandler(IsExpandedChangedEvent, value); }
            remove { RemoveHandler(IsExpandedChangedEvent, value); }
        }

        /// <summary>
        /// Gets or sets the is expanded property change notifier.
        /// </summary>
        /// <value>The is expanded property change notifier.</value>
        internal PropertyChangeNotifier IsExpandedPropertyChangeNotifier { get; set; }

        /// <summary>
        /// Determines whether [is expanded changed] [the specified sender].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void IsExpandedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;
            VerticalSlidePanel drawer = sender as VerticalSlidePanel;
            if (drawer == null)
                return;
            bool newVal = (bool)e.NewValue;
            drawer.IsExpanded = newVal;
            bool expanded = (bool)drawer.GetValue(IsExpandedProperty);
            Storyboard showPanel = new Storyboard();
            DoubleAnimationUsingKeyFrames animKeys = new DoubleAnimationUsingKeyFrames();
            if (expanded)
            {
                animKeys.KeyFrames.Add(new SplineDoubleKeyFrame
                {
                    KeyTime = TimeSpan.FromMilliseconds(250),
                    KeySpline = new KeySpline(0.4, 0, 1, 1),
                    Value = (double)drawer.GetValue(PanelHeightProperty)
                });
            }
            else
            {
                animKeys.KeyFrames.Add(new SplineDoubleKeyFrame
                {
                    KeyTime = TimeSpan.FromMilliseconds(250),
                    KeySpline = new KeySpline(0, 0, 0.2, 1),
                    Value = (double)drawer.GetValue(CollapsedHeightProperty)
                });
            }
            showPanel.Children.Add(animKeys);
            Storyboard.SetTargetName(animKeys, drawer.Name);
            Storyboard.SetTargetProperty(animKeys, new PropertyPath(HeightProperty));
            var opacity = AnimationHelper.DoubleAnimation(0.0, 1.0, expanded, new TimeSpan(0, 0, 0, 0, 200), OpacityProperty);
            Storyboard.SetTargetName(opacity, drawer.Name);
            Storyboard.SetTargetProperty(opacity, new PropertyPath(OpacityProperty));
            showPanel.Children.Add(opacity);
            showPanel.Begin(drawer);
        }
        #endregion

        #region Panel Height        
        /// <summary>
        /// The panel height property
        /// </summary>
        public static readonly DependencyProperty PanelHeightProperty =
            DependencyProperty.Register("PanelHeight", typeof(double), typeof(VerticalSlidePanel), new UIPropertyMetadata(5.0));

        /// <summary>
        /// Gets or sets the height of the panel.
        /// </summary>
        /// <value>The height of the panel.</value>
        public double PanelHeight
        {
            get { return (double)GetValue(PanelHeightProperty); }
            set { SetValue(PanelHeightProperty, value); }
        }
        #endregion

        #region Collapsed Height                
        /// <summary>
        /// The collapsed height property
        /// </summary>
        public static readonly DependencyProperty CollapsedHeightProperty =
            DependencyProperty.Register("CollapsedHeight", typeof(double), typeof(VerticalSlidePanel), new UIPropertyMetadata(5.0, OnCollapsedHeightChange));

        /// <summary>
        /// Gets or sets the collapsed height.
        /// </summary>
        /// <value>The collapsed height.</value>
        public double CollapsedHeight
        {
            get { return (double)GetValue(CollapsedHeightProperty); }
            set { SetValue(CollapsedHeightProperty, value); }
        }

        /// <summary>
        /// Handles the <see cref="E:CollapsedHeightChange" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnCollapsedHeightChange(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var panel = sender as VerticalSlidePanel;
            if (panel != null)
                panel.Height = (double)e.NewValue;
        }
        #endregion
    }
}
