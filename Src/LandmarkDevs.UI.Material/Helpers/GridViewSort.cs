using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace LandmarkDevs.UI.Material.Helpers
{
    /// <summary>
    /// Sortable GridView.
    /// Credit for this goes to http://www.thomaslevesque.com/2009/08/04/wpf-automatically-sort-a-gridview-continued/
    /// <example>
    /// yca:GridViewSort.AutoSort="True"
    /// yca:GridViewSort.ShowSortGlyph="False"
    /// </example>
    /// </summary>
    public static class GridViewSort
    {
		#region Public attached properties		
		/// <summary>
		/// Gets the command.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <returns>ICommand.</returns>
		public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

		/// <summary>
		/// Sets the command.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="value">The value.</param>
		public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

		/// <summary>
		/// Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
		/// </summary>
		public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached(
                "Command",
                typeof(ICommand),
                typeof(GridViewSort),
                new UIPropertyMetadata(
                    null,
                    (o, e) =>
                    {
	                    var listView = o as ItemsControl;
	                    if (listView != null && !GetAutoSort(listView))
	                    {
		                    if (e.OldValue != null && e.NewValue == null)
		                    {
			                    listView.RemoveHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
		                    }
		                    if (e.OldValue == null && e.NewValue != null)
		                    {
			                    listView.AddHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
		                    }
	                    }
                    }
                )
            );

		/// <summary>
		/// Gets the automatic sort.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <returns><c>true</c> if autosort, <c>false</c> otherwise.</returns>
		public static bool GetAutoSort(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoSortProperty);
        }

		/// <summary>
		/// Sets the automatic sort.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="value">if set to <c>true</c> [value].</param>
		public static void SetAutoSort(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoSortProperty, value);
        }

		/// <summary>
		/// Using a DependencyProperty as the backing store for AutoSort.  This enables animation, styling, binding, etc...
		/// </summary>
		public static readonly DependencyProperty AutoSortProperty =
            DependencyProperty.RegisterAttached(
                "AutoSort",
                typeof(bool),
                typeof(GridViewSort),
                new UIPropertyMetadata(
                    false,
                    (o, e) =>
                    {
	                    var listView = o as ListView;
	                    if (listView != null && GetCommand(listView) == null)
	                    {
		                    bool oldValue = (bool) e.OldValue;
		                    bool newValue = (bool) e.NewValue;
		                    if (oldValue && !newValue)
		                    {
			                    listView.RemoveHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
		                    }
		                    if (!oldValue && newValue)
		                    {
			                    listView.AddHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
		                    }
	                    }
                    }
                )
            );

		/// <summary>
		/// Gets the name of the property.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <returns>System.String.</returns>
		public static string GetPropertyName(DependencyObject obj)
        {
            return (string)obj.GetValue(PropertyNameProperty);
        }

		/// <summary>
		/// Sets the name of the property.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="value">The value.</param>
		public static void SetPropertyName(DependencyObject obj, string value)
        {
            obj.SetValue(PropertyNameProperty, value);
        }

		/// <summary>
		/// Using a DependencyProperty as the backing store for PropertyName.  This enables animation, styling, binding, etc...
		/// </summary>
		public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.RegisterAttached(
                "PropertyName",
                typeof(string),
                typeof(GridViewSort),
                new UIPropertyMetadata(null)
            );

		/// <summary>
		/// Gets the show sort glyph.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <returns><c>true</c> if the sort glyph is shown, <c>false</c> otherwise.</returns>
		public static bool GetShowSortGlyph(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowSortGlyphProperty);
        }

		/// <summary>
		/// Sets the show sort glyph.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="value">if set to <c>true</c> [value].</param>
		public static void SetShowSortGlyph(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowSortGlyphProperty, value);
        }

		/// <summary>
		/// Using a DependencyProperty as the backing store for ShowSortGlyph.  This enables animation, styling, binding, etc...
		/// </summary>
		public static readonly DependencyProperty ShowSortGlyphProperty =
            DependencyProperty.RegisterAttached("ShowSortGlyph", typeof(bool), typeof(GridViewSort), new UIPropertyMetadata(true));

		/// <summary>
		/// Gets the sort glyph ascending.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <returns>ImageSource.</returns>
		public static ImageSource GetSortGlyphAscending(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(SortGlyphAscendingProperty);
        }

		/// <summary>
		/// Sets the sort glyph ascending.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="value">The value.</param>
		public static void SetSortGlyphAscending(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(SortGlyphAscendingProperty, value);
        }

		/// <summary>
		/// The sort glyph ascending property
		/// </summary>
		public static readonly DependencyProperty SortGlyphAscendingProperty =
            DependencyProperty.RegisterAttached("SortGlyphAscending", typeof(ImageSource), typeof(GridViewSort), new UIPropertyMetadata(null));

		/// <summary>
		/// Gets the sort glyph descending.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <returns>ImageSource.</returns>
		public static ImageSource GetSortGlyphDescending(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(SortGlyphDescendingProperty);
        }

		/// <summary>
		/// Sets the sort glyph descending.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="value">The value.</param>
		public static void SetSortGlyphDescending(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(SortGlyphDescendingProperty, value);
        }

		/// <summary>
		/// The sort glyph descending property
		/// </summary>
		public static readonly DependencyProperty SortGlyphDescendingProperty =
            DependencyProperty.RegisterAttached("SortGlyphDescending", typeof(ImageSource), typeof(GridViewSort), new UIPropertyMetadata(null));

        #endregion

        #region Private attached properties

        private static GridViewColumnHeader GetSortedColumnHeader(DependencyObject obj)
        {
            return (GridViewColumnHeader)obj.GetValue(SortedColumnHeaderProperty);
        }

        private static void SetSortedColumnHeader(DependencyObject obj, GridViewColumnHeader value)
        {
            obj.SetValue(SortedColumnHeaderProperty, value);
        }

        // Using a DependencyProperty as the backing store for SortedColumn.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty SortedColumnHeaderProperty =
            DependencyProperty.RegisterAttached("SortedColumnHeader", typeof(GridViewColumnHeader), typeof(GridViewSort), new UIPropertyMetadata(null));

        #endregion

        #region Column header click event handler

        private static void ColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;
            if (headerClicked != null && headerClicked.Column != null)
            {
                GetPropertyName(headerClicked);
            }
        }

        private static void GetPropertyName(GridViewColumnHeader headerClicked)
        {
            var propertyName = GetPropertyName(headerClicked.Column);
            if (!string.IsNullOrEmpty(propertyName))
            {
                var listView = GetAncestor<ListView>(headerClicked);
                if (listView != null)
                {
                    CreateCommand(listView, headerClicked, propertyName);
                }
            }
        }

        private static void CreateCommand(ListView listView, GridViewColumnHeader headerClicked, string propertyName)
        {
            var command = GetCommand(listView);
            if (command != null)
            {
                if (command.CanExecute(propertyName))
                {
                    command.Execute(propertyName);
                }
            }
            else if (GetAutoSort(listView))
            {
                ApplySort(listView.Items, propertyName, listView, headerClicked);
            }
        }

		#endregion

		#region Helper methods		
		/// <summary>
		/// Gets the ancestor.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reference">The reference.</param>
		/// <returns>T.</returns>
		public static T GetAncestor<T>(DependencyObject reference) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(reference);
            while (!(parent is T))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            if (parent != null)
                return (T)parent;
            else
                return null;
        }

		/// <summary>
		/// Applies the sort.
		/// </summary>
		/// <param name="view">The view.</param>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="listView">The list view.</param>
		/// <param name="sortedColumnHeader">The sorted column header.</param>
		public static void ApplySort(ICollectionView view, string propertyName, ListView listView, GridViewColumnHeader sortedColumnHeader)
        {
            var direction = ListSortDirection.Ascending;
            if (view.SortDescriptions.Count > 0)
            {
                var currentSort = view.SortDescriptions[0];
                if (currentSort.PropertyName == propertyName)
                {
                    if (currentSort.Direction == ListSortDirection.Ascending)
                        direction = ListSortDirection.Descending;
                    else
                        direction = ListSortDirection.Ascending;
                }
                view.SortDescriptions.Clear();

                var currentSortedColumnHeader = GetSortedColumnHeader(listView);
                if (currentSortedColumnHeader != null)
                {
                    RemoveSortGlyph(currentSortedColumnHeader);
                }
            }
            if (!string.IsNullOrEmpty(propertyName))
            {
                view.SortDescriptions.Add(new SortDescription(propertyName, direction));
                if (GetShowSortGlyph(listView))
                    AddSortGlyph(
                        sortedColumnHeader,
                        direction,
                        direction == ListSortDirection.Ascending ? GetSortGlyphAscending(listView) : GetSortGlyphDescending(listView));
                SetSortedColumnHeader(listView, sortedColumnHeader);
            }
        }

        private static void AddSortGlyph(GridViewColumnHeader columnHeader, ListSortDirection direction, ImageSource sortGlyph)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(columnHeader);
            adornerLayer.Add(
                new SortGlyphAdorner(
                    columnHeader,
                    direction,
                    sortGlyph
                    ));
        }

        private static void RemoveSortGlyph(GridViewColumnHeader columnHeader)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(columnHeader);
            var adorners = adornerLayer.GetAdorners(columnHeader);
            if (adorners != null)
            {
                foreach (var adorner in adorners)
                {
                    if (adorner is SortGlyphAdorner)
                        adornerLayer.Remove(adorner);
                }
            }
        }

        #endregion

        #region SortGlyphAdorner nested class
        private class SortGlyphAdorner : Adorner
        {
            private readonly GridViewColumnHeader _columnHeader;
            private readonly ListSortDirection _direction;
            private readonly ImageSource _sortGlyph;

            public SortGlyphAdorner(GridViewColumnHeader columnHeader, ListSortDirection direction, ImageSource sortGlyph)
                : base(columnHeader)
            {
                _columnHeader = columnHeader;
                _direction = direction;
                _sortGlyph = sortGlyph;
            }

            private Geometry GetDefaultGlyph()
            {
                double x1 = _columnHeader.ActualWidth - 13;
                double x2 = x1 + 10;
                double x3 = x1 + 5;
                double y1 = _columnHeader.ActualHeight / 2 - 3;
                double y2 = y1 + 5;

                if (_direction == ListSortDirection.Ascending)
                {
                    double tmp = y1;
                    y1 = y2;
                    y2 = tmp;
                }

                var pathSegmentCollection = new PathSegmentCollection();
                pathSegmentCollection.Add(new LineSegment(new Point(x2, y1), true));
                pathSegmentCollection.Add(new LineSegment(new Point(x3, y2), true));

                var pathFigure = new PathFigure(
                    new Point(x1, y1),
                    pathSegmentCollection,
                    true);

                var pathFigureCollection = new PathFigureCollection();
                pathFigureCollection.Add(pathFigure);

                var pathGeometry = new PathGeometry(pathFigureCollection);
                return pathGeometry;
            }

            protected override void OnRender(DrawingContext drawingContext)
            {
                base.OnRender(drawingContext);

                if (_sortGlyph != null)
                {
                    var x = _columnHeader.ActualWidth - 13;
                    var y = _columnHeader.ActualHeight / 2 - 5;
                    var rect = new Rect(x, y, 10, 10);
                    drawingContext.DrawImage(_sortGlyph, rect);
                }
                else
                {
                    drawingContext.DrawGeometry(Brushes.LightGray, new Pen(Brushes.Gray, 1.0), GetDefaultGlyph());
                }
            }
        }
        #endregion
    }
}
