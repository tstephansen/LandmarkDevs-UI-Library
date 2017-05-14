using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using LandmarkDevs.UI.Common.Helpers;

namespace LandmarkDevs.UI.Material.Controls.Panels
{
    /// <summary>
    /// Class HorizontalSlidePanel.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.ContentControl" />
    public class HorizontalSlidePanel : ContentControl
    {
        /// <summary>
        /// Initializes static members of the <see cref="HorizontalSlidePanel"/> class.
        /// </summary>
        static HorizontalSlidePanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HorizontalSlidePanel),
                new FrameworkPropertyMetadata(typeof(HorizontalSlidePanel)));
        }

        #region Expanded        
        /// <summary>
        /// The is expanded property
        /// </summary>
        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded",
            typeof(bool), typeof(HorizontalSlidePanel),
            new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                IsExpandedChanged));

        /// <summary>
        /// Gets or sets a value indicating whether this instance is expanded.
        /// </summary>
        /// <value><see langword="true" /> if this instance is expanded; otherwise, <see langword="false" />.</value>
        public bool IsExpanded
        {
            get { return (bool) GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        /// <summary>
        /// The is expanded changed event
        /// </summary>
        public static readonly RoutedEvent IsExpandedChangedEvent =
            EventManager.RegisterRoutedEvent("IsExpandChanged", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(HorizontalSlidePanel));

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
            HorizontalSlidePanel drawer = sender as HorizontalSlidePanel;
            if (drawer == null)
                return;
            bool newVal = (bool) e.NewValue;
            drawer.IsExpanded = newVal;
            bool expanded = (bool) drawer.GetValue(IsExpandedProperty);
            Storyboard showPanel = new Storyboard();
            DoubleAnimationUsingKeyFrames animKeys = new DoubleAnimationUsingKeyFrames();
            if (expanded)
            {
                animKeys.KeyFrames.Add(new SplineDoubleKeyFrame
                {
                    KeyTime = TimeSpan.FromMilliseconds(250),
                    KeySpline = new KeySpline(0.4, 0, 1, 1),
                    Value = (double) drawer.GetValue(PanelWidthProperty)
                });
            }
            else
            {
                animKeys.KeyFrames.Add(new SplineDoubleKeyFrame
                {
                    KeyTime = TimeSpan.FromMilliseconds(250),
                    KeySpline = new KeySpline(0, 0, 0.2, 1),
                    Value = (double) drawer.GetValue(CollapsedWidthProperty)
                });
            }
            showPanel.Children.Add(animKeys);
            Storyboard.SetTargetName(animKeys, drawer.Name);
            Storyboard.SetTargetProperty(animKeys, new PropertyPath(WidthProperty));
            var opacity = AnimationHelper.DoubleAnimation(0.0, 1.0, expanded, new TimeSpan(0, 0, 0, 0, 200),
                OpacityProperty);
            Storyboard.SetTargetName(opacity, drawer.Name);
            Storyboard.SetTargetProperty(opacity, new PropertyPath(OpacityProperty));
            showPanel.Children.Add(opacity);
            showPanel.Begin(drawer);
        }

        #endregion

        #region Panel Width        
        /// <summary>
        /// The panel width property
        /// </summary>
        public static readonly DependencyProperty PanelWidthProperty =
            DependencyProperty.Register("PanelWidth", typeof(double), typeof(HorizontalSlidePanel),
                new UIPropertyMetadata(5.0));

        /// <summary>
        /// Gets or sets the width of the panel.
        /// </summary>
        /// <value>The width of the panel.</value>
        public double PanelWidth
        {
            get { return (double) GetValue(PanelWidthProperty); }
            set { SetValue(PanelWidthProperty, value); }
        }

        #endregion

        #region Collapsed Width                
        /// <summary>
        /// The collapsed width property
        /// </summary>
        public static readonly DependencyProperty CollapsedWidthProperty =
            DependencyProperty.Register("CollapsedWidth", typeof(double), typeof(HorizontalSlidePanel),
                new UIPropertyMetadata(5.0, new PropertyChangedCallback(OnCollapsedWidthChange)));

        /// <summary>
        /// Gets or sets the collapsed width.
        /// </summary>
        /// <value>The collapsed width.</value>
        public double CollapsedWidth
        {
            get { return (double) GetValue(CollapsedWidthProperty); }
            set { SetValue(CollapsedWidthProperty, value); }
        }

        /// <summary>
        /// Handles the <see cref="E:CollapsedWidthChange" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnCollapsedWidthChange(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var panel = sender as HorizontalSlidePanel;
            if (panel != null)
                panel.Width = (double) e.NewValue;
        }

        #endregion
    }
}