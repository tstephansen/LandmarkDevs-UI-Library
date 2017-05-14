using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using LandmarkDevs.UI.Common.Helpers;

namespace LandmarkDevs.UI.Material.Controls
{
    /// <summary>
    /// Class NavigationDrawer.
    /// This serves as a navigation drawer for the application.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.ContentControl" />
    public class NavigationDrawer : ContentControl
    {
        /// <summary>
        /// Initializes static members of the <see cref="NavigationDrawer"/> class.
        /// </summary>
        static NavigationDrawer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationDrawer), new FrameworkPropertyMetadata(typeof(NavigationDrawer)));
        }

        /// <summary>
        /// The header text property
        /// </summary>
        public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register(
            "HeaderText", typeof(string), typeof(NavigationDrawer), new PropertyMetadata(default(string)));

        /// <summary>
        /// Gets or sets the header text.
        /// </summary>
        /// <value>The header text.</value>
        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }

        /// <summary>
        /// The header content property
        /// </summary>
        public static readonly DependencyProperty HeaderContentProperty = DependencyProperty.Register(
            "HeaderContent", typeof(ContentControl), typeof(NavigationDrawer), new PropertyMetadata(default(ContentControl)));

        /// <summary>
        /// Gets or sets the content of the header.
        /// </summary>
        /// <value>The content of the header.</value>
        public ContentControl HeaderContent
        {
            get { return (ContentControl)GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }

        /// <summary>
        /// The header background property
        /// </summary>
        public static readonly DependencyProperty HeaderBackgroundProperty = DependencyProperty.Register(
            "HeaderBackground", typeof(Brush), typeof(NavigationDrawer), new PropertyMetadata(default(Brush)));

        /// <summary>
        /// Gets or sets the header background.
        /// </summary>
        /// <value>The header background.</value>
        public Brush HeaderBackground
        {
            get { return (Brush)GetValue(HeaderBackgroundProperty); }
            set { SetValue(HeaderBackgroundProperty, value); }
        }

        /// <summary>
        /// The header text font color property
        /// </summary>
        public static readonly DependencyProperty HeaderTextFontColorProperty = DependencyProperty.Register(
            "HeaderTextFontColor", typeof(SolidColorBrush), typeof(NavigationDrawer), new PropertyMetadata(new SolidColorBrush { Color = Color.FromRgb(255, 255, 255) }));

        /// <summary>
        /// Gets or sets the color of the header text font.
        /// </summary>
        /// <value>The color of the header text font.</value>
        public SolidColorBrush HeaderTextFontColor
        {
            get { return (SolidColorBrush)GetValue(HeaderTextFontColorProperty); }
            set { SetValue(HeaderTextFontColorProperty, value); }
        }

        /// <summary>
        /// The header text font size property
        /// </summary>
        public static readonly DependencyProperty HeaderTextFontSizeProperty = DependencyProperty.Register(
            "HeaderTextFontSize", typeof(double), typeof(NavigationDrawer), new FrameworkPropertyMetadata(default(double)));

        /// <summary>
        /// Gets or sets the size of the header text font.
        /// </summary>
        /// <value>The size of the header text font.</value>
        public double HeaderTextFontSize
        {
            get { return (double)GetValue(HeaderTextFontSizeProperty); }
            set { SetValue(HeaderTextFontSizeProperty, value); }
        }

        /// <summary>
        /// The header text visibility property
        /// </summary>
        public static readonly DependencyProperty HeaderTextVisibilityProperty = DependencyProperty.Register(
            "HeaderTextVisibility", typeof(Visibility), typeof(NavigationDrawer), new FrameworkPropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// Gets or sets the header text visibility.
        /// </summary>
        /// <value>The header text visibility.</value>
        public Visibility HeaderTextVisibility
        {
            get { return (Visibility)GetValue(HeaderTextVisibilityProperty); }
            set { SetValue(HeaderTextVisibilityProperty, value); }
        }

        #region Expanded
        /// <summary>
        /// The is expanded property
        /// </summary>
        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(NavigationDrawer), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsExpandedChanged));

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
                typeof(RoutedEventHandler), typeof(NavigationDrawer));

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
            NavigationDrawer drawer = sender as NavigationDrawer;
            if (drawer == null)
                return;
            bool newVal = (bool)e.NewValue;
            drawer.IsExpanded = newVal;
            bool expanded = (bool)drawer.GetValue(IsExpandedProperty);
            Storyboard showDrawer = new Storyboard();
            DoubleAnimationUsingKeyFrames animKeys = new DoubleAnimationUsingKeyFrames();
            if (expanded)
            {
                animKeys.KeyFrames.Add(new SplineDoubleKeyFrame
                {
                    KeyTime = TimeSpan.FromMilliseconds(250),
                    KeySpline = new KeySpline(0.4, 0, 1, 1),
                    Value = (double)drawer.GetValue(DrawerWidthProperty)
                });
            }
            else
            {
                animKeys.KeyFrames.Add(new SplineDoubleKeyFrame
                {
                    KeyTime = TimeSpan.FromMilliseconds(250),
                    KeySpline = new KeySpline(0, 0, 0.2, 1),
                    Value = (double)drawer.GetValue(CollapsedWidthProperty)
                });
            }
            showDrawer.Children.Add(animKeys);
            Storyboard.SetTargetName(animKeys, drawer.Name);
            Storyboard.SetTargetProperty(animKeys, new PropertyPath(WidthProperty));
            Timeline opacity = AnimationHelper.DoubleAnimation(0.0, 1.0, expanded, new TimeSpan(0, 0, 0, 0, 200), OpacityProperty);
            Storyboard.SetTargetName(opacity, drawer.Name);
            Storyboard.SetTargetProperty(opacity, new PropertyPath(OpacityProperty));
            showDrawer.Children.Add(opacity);
            showDrawer.Begin(drawer);
        }

        #endregion Expanded

        #region Navigation Drawer Width
        /// <summary>
        /// The drawer width property
        /// </summary>
        public static readonly DependencyProperty DrawerWidthProperty =
            DependencyProperty.Register("DrawerWidth", typeof(double), typeof(NavigationDrawer), new UIPropertyMetadata(5.0));

        /// <summary>
        /// Gets or sets the width of the drawer.
        /// </summary>
        /// <value>The width of the drawer.</value>
        public double DrawerWidth
        {
            get { return (double)GetValue(DrawerWidthProperty); }
            set { SetValue(DrawerWidthProperty, value); }
        }

        #endregion Navigation Drawer Width

        #region Collapsed Width
        /// <summary>
        /// The collapsed width property
        /// </summary>
        public static readonly DependencyProperty CollapsedWidthProperty =
            DependencyProperty.Register("CollapsedWidth", typeof(double), typeof(NavigationDrawer), new UIPropertyMetadata(5.0, OnCollapsedWidthChange));

        /// <summary>
        /// Gets or sets the width of the collapsed.
        /// </summary>
        /// <value>The width of the collapsed.</value>
        public double CollapsedWidth
        {
            get { return (double)GetValue(CollapsedWidthProperty); }
            set { SetValue(CollapsedWidthProperty, value); }
        }

        /// <summary>
        /// Handles the <see cref="E:CollapsedWidthChange" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnCollapsedWidthChange(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            NavigationDrawer drawer = sender as NavigationDrawer;
            if (drawer != null)
                drawer.Width = (double)e.NewValue;
        }

        #endregion Collapsed Width
    }
}