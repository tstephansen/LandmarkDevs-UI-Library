#region
using LandmarkDevs.UI.Material.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

#endregion

namespace LandmarkDevs.UI.Material.Controls.Kanban
{
    /// <summary>
    ///     Class KanbanList.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.ListView" />
    public class KanbanList : ListView
    {
        static KanbanList()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KanbanList),
                new FrameworkPropertyMetadata(typeof(KanbanList)));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="KanbanList" /> class.
        /// </summary>
        public KanbanList()
        {
            CommandBindings.Add(new CommandBinding(KanbanBoardCommands.CloseKanbanItemCommand, CloseKanbanItem,
                CanCloseKanbanItem));
            CommandBindings.Add(new CommandBinding(KanbanBoardCommands.EditKanbanItemCommand, EditKanbanItem,
                CanEditKanbanItem));
        }

        /// <summary>
        ///     When overridden in a derived class, is invoked whenever application code or internal processes call
        ///     <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            MouseMove += KanbanList_PreviewMouseMove;
            Drop += KanbanList_Drop;
        }

        #region Properties
        private void KanbanList_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
                return;
            Point mousePos = e.GetPosition(null);
            Vector diff = new Point(0, 0) - mousePos;
            if (e.LeftButton == MouseButtonState.Pressed &&
                Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                var view = (KanbanList)sender;
                KanbanList parent = view;
                var data = GetKanbanItem(parent, e.GetPosition(view));
                if (data != null)
                {
                    var dragSource = this;
                    object[] source = { this, data };
                    var dataObj = new DataObject("DragSource", source);
                    DragDrop.DoDragDrop(dragSource, dataObj, DragDropEffects.Move);
                }
            }
        }

        private void KanbanList_Drop(object sender, DragEventArgs e)
        {
            var dragSource = e.Data.GetData("DragSource") as object[];
            var data = dragSource[1] as KanbanItemModel;
            var source = dragSource[0] as KanbanList;
            var dest = e.Source as KanbanList;
            var item = data;
            if (dest == source)
                return;
            var itemsSource = dest.ItemsSource as ObservableCollection<KanbanItemModel>;
            if (!itemsSource.Contains(item))
                itemsSource.Add(item);
            ((ObservableCollection<KanbanItemModel>)source.ItemsSource).Remove(item);
        }

        #endregion

        #region Methods
        private static object GetKanbanItem(KanbanList view, Point point)
        {
            UIElement element = view.InputHitTest(point) as UIElement;
            if (element == null)
                return null;
            object data = DependencyProperty.UnsetValue;
            while (data == DependencyProperty.UnsetValue)
            {
                data = view.ItemContainerGenerator.ItemFromContainer(element);
                if (data == DependencyProperty.UnsetValue)
                    element = VisualTreeHelper.GetParent(element) as UIElement;
                if (element == view)
                    return null;
            }
            if (data != DependencyProperty.UnsetValue)
                return data;
            return null;
        }

        private void AnimateKanbanItem(KanbanList view, KanbanItem kItem)
        {
            // Calculate Data
            var currentLocation = kItem.TranslatePoint(new Point(), view);
            var currentHeight = kItem.ActualHeight;
            var parentGrid = view.Parent as Grid;
            KanbanListCommands.KanbanGrid = parentGrid;
            var parentWidth = parentGrid.ActualWidth;
            var parentHeight = parentGrid.ActualHeight;
            const double editHeight = 300;
            const double editWidth = 300;
            var translateX = (parentWidth / 2) - currentLocation.X - (editWidth / 2);
            var translateY = (parentHeight / 2) - currentLocation.Y - (currentHeight / 2);

            // Create Copy and Remove Original
            var model = new KanbanItemModel(kItem.DataContext as KanbanItemModel);
            var item = new KanbanItem
            {
                DataContext = model,
                Name = "EditKanbanItemPopup"
            };
            item.CommandBindings.Add(new CommandBinding(KanbanListCommands.CloseEditKanbanItemCommand, CloseEditedItem,
                CanCloseEditedItem));
            var itemModel = kItem.DataContext as KanbanItemModel;
            KanbanListCommands.EditItem = item;
            ((ObservableCollection<KanbanItemModel>)view.ItemsSource).Remove(itemModel);
            parentGrid.Children.Add(item);

            // Create Storyboard
            var sb = new Storyboard();
            var sbTranslateX = new DoubleAnimationUsingKeyFrames();
            var sbTranslateY = new DoubleAnimationUsingKeyFrames();
            var sbScaleX = new DoubleAnimationUsingKeyFrames();
            var sbScaleY = new DoubleAnimationUsingKeyFrames();
            var duration = TimeSpan.FromMilliseconds(250);
            sb.Duration = duration;

            // Create Animations
            sbTranslateX.KeyFrames.Add(new SplineDoubleKeyFrame
            {
                KeyTime = duration,
                KeySpline = new KeySpline(0.4, 0, 1, 1),
                Value = translateX
            });
            sbTranslateY.KeyFrames.Add(new SplineDoubleKeyFrame
            {
                KeyTime = duration,
                KeySpline = new KeySpline(0.4, 0, 1, 1),
                Value = translateY
            });

            sbScaleX.KeyFrames.Add(new SplineDoubleKeyFrame
            {
                KeyTime = duration,
                KeySpline = new KeySpline(0.4, 0, 1, 1),
                Value = editWidth
            });
            sbScaleY.KeyFrames.Add(new SplineDoubleKeyFrame
            {
                KeyTime = duration,
                KeySpline = new KeySpline(0.4, 0, 1, 1),
                Value = editHeight
            });

            // Add Animations to Storyboard
            sb.Children.Add(sbTranslateX);
            sb.Children.Add(sbTranslateY);
            sb.Children.Add(sbScaleX);
            sb.Children.Add(sbScaleY);
            Storyboard.SetTarget(sbTranslateX, item);
            Storyboard.SetTarget(sbTranslateY, item);
            Storyboard.SetTarget(sbScaleX, item);
            Storyboard.SetTarget(sbScaleY, item);
            item.RenderTransform = new TranslateTransform();
            Storyboard.SetTargetProperty(sbTranslateX,
                new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)", null));
            Storyboard.SetTargetProperty(sbTranslateY,
                new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)", null));
            Storyboard.SetTargetProperty(sbScaleX, new PropertyPath("(FrameworkElement.Width)", null));
            Storyboard.SetTargetProperty(sbScaleY, new PropertyPath("(FrameworkElement.Height)", null));
            sb.Begin();
            Task.Run(() =>
            {
                Thread.Sleep(500);
                Dispatcher.Invoke(() => { parentGrid.Children.Remove(item); });
            });
        }

        #endregion

        #region Routed Commands
        /// <summary>
        ///     Edits the kanban item.
        ///     B(t) = (1-t)^2(P0)+2(1-t)tP1+t^2P2
        ///     t = 0.5;
        ///     x = (1 - t) * (1 - t) * p[0].x + 2 * (1 - t) * t* p[1].x + t* t * p[2].x;
        ///     y = (1 - t) * (1 - t) * p[0].y + 2 * (1 - t) * t* p[1].y + t* t * p[2].y;
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs" /> instance containing the event data.</param>
        private static void EditKanbanItem(object sender, ExecutedRoutedEventArgs e)
        {
            var view = (KanbanList)sender;
            var item = e.Parameter as KanbanItem;
            view.AnimateKanbanItem(view, item);
        }

        private static void CanEditKanbanItem(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private static void CloseKanbanItem(object sender, ExecutedRoutedEventArgs e)
        {
            var host = (KanbanList)sender;
            var param = (KanbanItemModel)e.Parameter;
            ((ObservableCollection<KanbanItemModel>)host.ItemsSource).Remove(param);
        }

        private static void CanCloseKanbanItem(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private static void CloseEditedItem(object sender, ExecutedRoutedEventArgs e)
        {
            KanbanListCommands.KanbanGrid.Children.Remove(KanbanListCommands.EditItem);
        }

        private static void CanCloseEditedItem(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion
    }
}