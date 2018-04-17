using LandmarkDevs.UI.Models.Kanban;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace LandmarkDevs.UI.Material.Controls.Kanban
{
    /// <summary>
    ///     Class KanbanBoard.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.Control" />
    [TemplatePart(Name = PART_LaneOne, Type = typeof(KanbanList))]
    [TemplatePart(Name = PART_LaneTwo, Type = typeof(KanbanList))]
    [TemplatePart(Name = PART_LaneThree, Type = typeof(KanbanList))]
    [TemplatePart(Name = PART_LaneFour, Type = typeof(KanbanList))]
    [TemplatePart(Name = PART_LaneThreeTitleTextBlock, Type = typeof(TextBlock))]
    [TemplatePart(Name = PART_LaneFourTitleTextBlock, Type = typeof(TextBlock))]
    [TemplatePart(Name = PART_KanbanBoardCard, Type = typeof(Card))]
    [TemplatePart(Name = PART_AnimationCanvas, Type = typeof(Canvas))]
    [TemplatePart(Name = PART_LaneOneColumnDefinition, Type = typeof(ColumnDefinition))]
    [TemplatePart(Name = PART_LaneTwoColumnDefinition, Type = typeof(ColumnDefinition))]
    [TemplatePart(Name = PART_LaneThreeColumnDefinition, Type = typeof(ColumnDefinition))]
    [TemplatePart(Name = PART_LaneFourColumnDefinition, Type = typeof(ColumnDefinition))]
    [TemplateVisualState(GroupName = "KanbanBoardStates", Name = NormalVisualState)]
    [TemplateVisualState(GroupName = "KanbanBoardStates", Name = AddItemVisualState)]
    public class KanbanBoard : Control
    {
        static KanbanBoard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KanbanBoard),
                new FrameworkPropertyMetadata(typeof(KanbanBoard)));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="KanbanBoard" /> class.
        /// </summary>
        public KanbanBoard()
        {
            LaneOneItems = new ObservableCollection<KanbanItemModel>();
            LaneTwoItems = new ObservableCollection<KanbanItemModel>();
            LaneThreeItems = new ObservableCollection<KanbanItemModel>();
            LaneFourItems = new ObservableCollection<KanbanItemModel>();
            NewKanbanItemPriorities = new List<string>
            {
                "Critical",
                "High",
                "Normal",
                "Low"
            };
            NewKanbanItemSelectedPriority = NewKanbanItemPriorities.ElementAt(2);
            CommandBindings.Add(new CommandBinding(KanbanBoardCommands.AddNewKanbanItemCommand, AddNewKanbanItem,
                CanAddNewKanbanItem));
            CommandBindings.Add(new CommandBinding(KanbanBoardCommands.SaveNewKanbanItemCommand, SaveNewKanbanItem,
                CanSaveNewKanbanItem));
            CommandBindings.Add(new CommandBinding(KanbanBoardCommands.CancelNewKanbanItemCommand, CancelNewKanbanItem,
                CanCancelNewKanbanItem));
            Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/LandmarkDevs.UI.Material;component/Styles/ApplicationStyles.xaml")
            });
            Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/LandmarkDevs.UI.Material;component/Themes/KanbanBoard.xaml")
            });
        }

        /// <summary>
        ///     When overridden in a derived class, is invoked whenever application code or internal processes call
        ///     <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            LaneOne = GetTemplateChild(PART_LaneOne) as KanbanList;
            LaneTwo = GetTemplateChild(PART_LaneTwo) as KanbanList;
            LaneThree = GetTemplateChild(PART_LaneThree) as KanbanList;
            LaneFour = GetTemplateChild(PART_LaneFour) as KanbanList;
            LaneThreeTitleTextBlock = GetTemplateChild(PART_LaneThreeTitleTextBlock) as TextBlock;
            LaneFourTitleTextBlock = GetTemplateChild(PART_LaneFourTitleTextBlock) as TextBlock;
            LaneOneColumnDefinition = GetTemplateChild(PART_LaneOneColumnDefinition) as ColumnDefinition;
            LaneTwoColumnDefinition = GetTemplateChild(PART_LaneTwoColumnDefinition) as ColumnDefinition;
            LaneThreeColumnDefinition = GetTemplateChild(PART_LaneThreeColumnDefinition) as ColumnDefinition;
            LaneFourColumnDefinition = GetTemplateChild(PART_LaneFourColumnDefinition) as ColumnDefinition;
            KanbanCard = GetTemplateChild(PART_KanbanBoardCard) as Card;
            AnimationCanvas = GetTemplateChild(PART_AnimationCanvas) as Canvas;
            InitializeLanes();
            if (Lanes == Lanes.Two)
            {
                CreateTwoLanes();
            }

            if (Lanes == Lanes.Three)
            {
                CreateThreeLanes();
            }
            GotoVisualState(NormalVisualState, false);
        }

        private void InitializeLanes()
        {
            if (LaneOne != null)
                LaneOne.ItemsSource = LaneOneItems;
            if (LaneTwo != null)
                LaneTwo.ItemsSource = LaneTwoItems;
            if (LaneThree != null)
                LaneThree.ItemsSource = LaneThreeItems;
            if (LaneFour != null)
                LaneFour.ItemsSource = LaneFourItems;
        }

        private void CreateTwoLanes()
        {
            if (LaneOneColumnDefinition != null)
                LaneOneColumnDefinition.Width = new GridLength(2, GridUnitType.Star);
            if (LaneTwoColumnDefinition != null)
                LaneTwoColumnDefinition.Width = new GridLength(2, GridUnitType.Star);
            if (LaneThreeColumnDefinition != null)
                LaneThreeColumnDefinition.Width = new GridLength(0);
            if (LaneFourColumnDefinition != null)
                LaneFourColumnDefinition.Width = new GridLength(0);

            if (LaneThree != null)
            {
                LaneThree.Visibility = Visibility.Collapsed;
                LaneThree.ItemsSource = LaneThreeItems;
                if (LaneThreeTitleTextBlock != null)
                    LaneThreeTitleTextBlock.Visibility = Visibility.Collapsed;
            }
            if (LaneFour != null)
            {
                LaneFour.Visibility = Visibility.Collapsed;
                LaneFour.ItemsSource = LaneFourItems;
                if (LaneFourTitleTextBlock != null)
                    LaneFourTitleTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void CreateThreeLanes()
        {
            if (LaneOneColumnDefinition != null)
                LaneOneColumnDefinition.Width = new GridLength(3, GridUnitType.Star);
            if (LaneTwoColumnDefinition != null)
                LaneTwoColumnDefinition.Width = new GridLength(3, GridUnitType.Star);
            if (LaneThreeColumnDefinition != null)
                LaneThreeColumnDefinition.Width = new GridLength(3, GridUnitType.Star);
            if (LaneFourColumnDefinition != null)
                LaneFourColumnDefinition.Width = new GridLength(0);

            if (LaneFour != null)
            {
                LaneFour.Visibility = Visibility.Collapsed;
                LaneFour.ItemsSource = LaneFourItems;
                if (LaneFourTitleTextBlock != null)
                    LaneFourTitleTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void GotoVisualState(string stateName, bool useTransitions = true)
        {
            var thisFe = this as FrameworkElement;
            VisualStateManager.GoToState(thisFe, stateName, useTransitions);
        }

        #region Templates
        // ReSharper disable InconsistentNaming
        private const string PART_LaneOne = "PART_LaneOne";

        private const string PART_LaneTwo = "PART_LaneTwo";
        private const string PART_LaneThree = "PART_LaneThree";
        private const string PART_LaneFour = "PART_LaneFour";
        private const string PART_LaneThreeTitleTextBlock = "PART_LaneThreeTitleTextBlock";
        private const string PART_LaneFourTitleTextBlock = "PART_LaneFourTitleTextBlock";
        private const string PART_LaneOneColumnDefinition = "PART_LaneOneColumnDefinition";
        private const string PART_LaneTwoColumnDefinition = "PART_LaneTwoColumnDefinition";
        private const string PART_LaneThreeColumnDefinition = "PART_LaneThreeColumnDefinition";
        private const string PART_LaneFourColumnDefinition = "PART_LaneFourColumnDefinition";
        private const string NormalVisualState = "Normal";
        private const string AddItemVisualState = "AddItem";
        private const string PART_KanbanBoardCard = "PART_KanbanBoardCard";
        private const string PART_AnimationCanvas = "PART_AnimationCanvas";
        // ReSharper restore InconsistentNaming

        /// <summary>
        ///     The lane one
        /// </summary>
        internal KanbanList LaneOne;

        /// <summary>
        ///     The lane two
        /// </summary>
        internal KanbanList LaneTwo;

        /// <summary>
        ///     The lane three
        /// </summary>
        internal KanbanList LaneThree;

        /// <summary>
        ///     The lane four
        /// </summary>
        internal KanbanList LaneFour;

        /// <summary>
        ///     The lane three title text block
        /// </summary>
        internal TextBlock LaneThreeTitleTextBlock;

        /// <summary>
        ///     The lane four title text block
        /// </summary>
        internal TextBlock LaneFourTitleTextBlock;

        /// <summary>
        ///     The lane one column definition
        /// </summary>
        internal ColumnDefinition LaneOneColumnDefinition;

        /// <summary>
        ///     The lane two column definition
        /// </summary>
        internal ColumnDefinition LaneTwoColumnDefinition;

        /// <summary>
        ///     The lane three column definition
        /// </summary>
        internal ColumnDefinition LaneThreeColumnDefinition;

        /// <summary>
        ///     The lane four column definition
        /// </summary>
        internal ColumnDefinition LaneFourColumnDefinition;

        /// <summary>
        ///     The hide add new item dialog storyboard
        /// </summary>
        internal Storyboard HideAddNewItemDialogStoryboard;

        /// <summary>
        ///     The kanban card
        /// </summary>
        internal Card KanbanCard;

        /// <summary>
        ///     The animation canvas
        /// </summary>
        internal Canvas AnimationCanvas;

        #endregion

        #region Dependency Properties

        #region Data Fields
        /// <summary>
        ///     The lane one items property
        /// </summary>
        public static readonly DependencyProperty LaneOneItemsProperty = DependencyProperty.Register(
            "LaneOneItems", typeof(IList<KanbanItemModel>), typeof(KanbanBoard));

        /// <summary>
        ///     Gets or sets the lane one items.
        /// </summary>
        /// <value>The lane one items.</value>
        public IList<KanbanItemModel> LaneOneItems
        {
            get { return (IList<KanbanItemModel>)GetValue(LaneOneItemsProperty); }
            set { SetValue(LaneOneItemsProperty, value); }
        }

        /// <summary>
        ///     The lane two items property
        /// </summary>
        public static readonly DependencyProperty LaneTwoItemsProperty = DependencyProperty.Register(
            "LaneTwoItems", typeof(IList<KanbanItemModel>), typeof(KanbanBoard));

        /// <summary>
        ///     Gets or sets the lane two items.
        /// </summary>
        /// <value>The lane two items.</value>
        public IList<KanbanItemModel> LaneTwoItems
        {
            get { return (IList<KanbanItemModel>)GetValue(LaneTwoItemsProperty); }
            set { SetValue(LaneTwoItemsProperty, value); }
        }

        /// <summary>
        ///     The lane three items property
        /// </summary>
        public static readonly DependencyProperty LaneThreeItemsProperty = DependencyProperty.Register(
            "LaneThreeItems", typeof(IList<KanbanItemModel>), typeof(KanbanBoard));

        /// <summary>
        ///     Gets or sets the lane three items.
        /// </summary>
        /// <value>The lane three items.</value>
        public IList<KanbanItemModel> LaneThreeItems
        {
            get { return (IList<KanbanItemModel>)GetValue(LaneThreeItemsProperty); }
            set { SetValue(LaneThreeItemsProperty, value); }
        }

        /// <summary>
        ///     The lane four items property
        /// </summary>
        public static readonly DependencyProperty LaneFourItemsProperty = DependencyProperty.Register(
            "LaneFourItems", typeof(IList<KanbanItemModel>), typeof(KanbanBoard));

        /// <summary>
        ///     Gets or sets the lane four items.
        /// </summary>
        /// <value>The lane four items.</value>
        public IList<KanbanItemModel> LaneFourItems
        {
            get { return (IList<KanbanItemModel>)GetValue(LaneFourItemsProperty); }
            set { SetValue(LaneFourItemsProperty, value); }
        }

        #endregion

        /// <summary>
        ///     The lanes property
        /// </summary>
        public static readonly DependencyProperty LanesProperty = DependencyProperty.Register(
            "Lanes", typeof(Lanes), typeof(KanbanBoard), new PropertyMetadata(default(Lanes)));

        /// <summary>
        ///     Gets or sets the lanes.
        /// </summary>
        /// <value>The lanes.</value>
        public Lanes Lanes
        {
            get { return (Lanes)GetValue(LanesProperty); }
            set { SetValue(LanesProperty, value); }
        }

        /// <summary>
        ///     The lane one title property
        /// </summary>
        public static readonly DependencyProperty LaneOneTitleProperty = DependencyProperty.Register(
            "LaneOneTitle", typeof(string), typeof(KanbanBoard), new PropertyMetadata(default(string)));

        /// <summary>
        ///     Gets or sets the lane one title.
        /// </summary>
        /// <value>The lane one title.</value>
        public string LaneOneTitle
        {
            get { return (string)GetValue(LaneOneTitleProperty); }
            set { SetValue(LaneOneTitleProperty, value); }
        }

        /// <summary>
        ///     The lane two title property
        /// </summary>
        public static readonly DependencyProperty LaneTwoTitleProperty = DependencyProperty.Register(
            "LaneTwoTitle", typeof(string), typeof(KanbanBoard), new PropertyMetadata(default(string)));

        /// <summary>
        ///     Gets or sets the lane two title.
        /// </summary>
        /// <value>The lane two title.</value>
        public string LaneTwoTitle
        {
            get { return (string)GetValue(LaneTwoTitleProperty); }
            set { SetValue(LaneTwoTitleProperty, value); }
        }

        /// <summary>
        ///     The lane three title property
        /// </summary>
        public static readonly DependencyProperty LaneThreeTitleProperty = DependencyProperty.Register(
            "LaneThreeTitle", typeof(string), typeof(KanbanBoard), new PropertyMetadata(default(string)));

        /// <summary>
        ///     Gets or sets the lane three title.
        /// </summary>
        /// <value>The lane three title.</value>
        public string LaneThreeTitle
        {
            get { return (string)GetValue(LaneThreeTitleProperty); }
            set { SetValue(LaneThreeTitleProperty, value); }
        }

        /// <summary>
        ///     The lane four title property
        /// </summary>
        public static readonly DependencyProperty LaneFourTitleProperty = DependencyProperty.Register(
            "LaneFourTitle", typeof(string), typeof(KanbanBoard), new PropertyMetadata(default(string)));

        /// <summary>
        ///     Gets or sets the lane four title.
        /// </summary>
        /// <value>The lane four title.</value>
        public string LaneFourTitle
        {
            get { return (string)GetValue(LaneFourTitleProperty); }
            set { SetValue(LaneFourTitleProperty, value); }
        }

        #region New Kanban Item
        /// <summary>
        ///     The new kanban item title property
        /// </summary>
        public static readonly DependencyProperty NewKanbanItemTitleProperty = DependencyProperty.Register(
            "NewKanbanItemTitle", typeof(string), typeof(KanbanBoard), new FrameworkPropertyMetadata(default(string),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        ///     Gets or sets the new kanban item title.
        /// </summary>
        /// <value>The new kanban item title.</value>
        public string NewKanbanItemTitle
        {
            get { return (string)GetValue(NewKanbanItemTitleProperty); }
            set { SetValue(NewKanbanItemTitleProperty, value); }
        }

        /// <summary>
        ///     The new kanban item summary property
        /// </summary>
        public static readonly DependencyProperty NewKanbanItemSummaryProperty = DependencyProperty.Register(
            "NewKanbanItemSummary", typeof(string), typeof(KanbanBoard), new FrameworkPropertyMetadata(default(string),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        ///     Gets or sets the new kanban item summary.
        /// </summary>
        /// <value>The new kanban item summary.</value>
        public string NewKanbanItemSummary
        {
            get { return (string)GetValue(NewKanbanItemSummaryProperty); }
            set { SetValue(NewKanbanItemSummaryProperty, value); }
        }

        /// <summary>
        ///     The new kanban item priorities property
        /// </summary>
        public static readonly DependencyProperty NewKanbanItemPrioritiesProperty = DependencyProperty.Register(
            "NewKanbanItemPriorities", typeof(IList<string>), typeof(KanbanBoard));

        /// <summary>
        ///     Gets or sets the new kanban item priorities.
        /// </summary>
        /// <value>The new kanban item priorities.</value>
        public IList<string> NewKanbanItemPriorities
        {
            get { return (IList<string>)GetValue(NewKanbanItemPrioritiesProperty); }
            set { SetValue(NewKanbanItemPrioritiesProperty, value); }
        }

        /// <summary>
        ///     The new kanban item selected priority property
        /// </summary>
        public static readonly DependencyProperty NewKanbanItemSelectedPriorityProperty = DependencyProperty.Register(
            "NewKanbanItemSelectedPriority", typeof(string), typeof(KanbanBoard));

        /// <summary>
        ///     Gets or sets the new kanban item selected priority.
        /// </summary>
        /// <value>The new kanban item selected priority.</value>
        public string NewKanbanItemSelectedPriority
        {
            get { return (string)GetValue(NewKanbanItemSelectedPriorityProperty); }
            set { SetValue(NewKanbanItemSelectedPriorityProperty, value); }
        }

        #endregion

        /// <summary>
        ///     The priority color location property
        /// </summary>
        public static readonly DependencyProperty PriorityColorLocationProperty = DependencyProperty.Register(
            "PriorityColorLocation", typeof(PriorityColorLocation), typeof(KanbanBoard),
            new FrameworkPropertyMetadata(PriorityColorLocation.Bottom,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        ///     Gets or sets the priority color location.
        /// </summary>
        /// <value>The priority color location.</value>
        public PriorityColorLocation PriorityColorLocation
        {
            get { return (PriorityColorLocation)GetValue(PriorityColorLocationProperty); }
            set { SetValue(PriorityColorLocationProperty, value); }
        }

        /// <summary>
        ///     The kanban board margin property
        /// </summary>
        public static readonly DependencyProperty KanbanBoardMarginProperty = DependencyProperty.Register(
            "KanbanBoardMargin", typeof(Thickness), typeof(KanbanBoard), new PropertyMetadata(new Thickness(20)));

        /// <summary>
        ///     Gets or sets the kanban board margin.
        /// </summary>
        /// <value>The kanban board margin.</value>
        public Thickness KanbanBoardMargin
        {
            get { return (Thickness)GetValue(KanbanBoardMarginProperty); }
            set { SetValue(KanbanBoardMarginProperty, value); }
        }

        #endregion

        #region Routed Commands
        private static void AddNewKanbanItem(object sender, ExecutedRoutedEventArgs e)
        {
            var host = (KanbanBoard)sender;
            VisualStateManager.GetVisualStateGroups(host);
            host.GotoVisualState(AddItemVisualState);
        }

        private static void CanAddNewKanbanItem(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private static void SaveNewKanbanItem(object sender, ExecutedRoutedEventArgs e)
        {
            var host = (KanbanBoard)sender;
            host.Dispatcher.VerifyAccess();
            host.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => host.SaveNewItem(host)));
            host.GotoVisualState(NormalVisualState);
        }

        /// <summary>
        ///     Hides the add item dialog storyboard.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <returns>Task.</returns>
        /// <exception cref="System.InvalidOperationException">
        ///     KanbanBoard is missing!
        ///     or
        ///     KanbanCard is null
        /// </exception>
        public Task HideAddItemDialogStoryboard(KanbanBoard host)
        {
            if (host == null)
                throw new InvalidOperationException("KanbanBoard is missing!");
            if (KanbanCard == null)
                throw new InvalidOperationException("KanbanCard is null");
            Dispatcher.VerifyAccess();
            var taskCompletionSource = new TaskCompletionSource<object>();
            var sb = (Storyboard)Resources.MergedDictionaries[1]["HideAddItemDialogStoryboard"];
            sb = sb.Clone();
            EventHandler completionHandler = null;
            completionHandler = (sender, args) =>
            {
                sb.Completed -= completionHandler;
                taskCompletionSource.TrySetResult(null);
            };
            sb.Completed += completionHandler;
            KanbanCard.BeginStoryboard(sb);
            HideAddNewItemDialogStoryboard = sb;
            return taskCompletionSource.Task;
        }

        private void SaveNewItem(KanbanBoard host)
        {
            HideAddItemDialogStoryboard(this).ContinueWith(c =>
            {
                host.Dispatcher.BeginInvoke(new Action(() =>
                {
                    var currentTime = DateTime.Now;
                    var priority = ConvertKanbanPriorityStringToEnum(NewKanbanItemSelectedPriority);
                    var item = new KanbanItemModel
                    {
                        Title = NewKanbanItemTitle,
                        Summary = NewKanbanItemSummary,
                        CreatedBy = Environment.UserName,
                        Created = currentTime,
                        IsComplete = false,
                        Updated = currentTime,
                        KanbanPriority = priority,
                        Notes = new ObservableCollection<KanbanNote>()
                    };
                    LaneOneItems.Add(item);
                    ClearNewKanbanFields();
                }));
            });
        }

        private void ClearNewKanbanFields()
        {
            Dispatcher.Invoke(() =>
            {
                NewKanbanItemTitle = "";
                NewKanbanItemSummary = "";
                NewKanbanItemSelectedPriority = NewKanbanItemPriorities.ElementAt(2);
            });
        }

        private static KanbanPriority ConvertKanbanPriorityStringToEnum(string priority)
        {
            switch (priority)
            {
                case "Critical":
                    return KanbanPriority.Critical;
                case "High":
                    return KanbanPriority.High;
                default:
                case "Normal":
                    return KanbanPriority.Normal;
                case "Low":
                    return KanbanPriority.Low;
            }
        }

        private static void CanSaveNewKanbanItem(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private static void CancelNewKanbanItem(object sender, ExecutedRoutedEventArgs e)
        {
            var host = (KanbanBoard)sender;
            host.NewKanbanItemSelectedPriority = host.NewKanbanItemPriorities.First(c => c == "Normal");
            host.NewKanbanItemTitle = "";
            host.NewKanbanItemSummary = "";
            host.GotoVisualState(NormalVisualState);
        }

        private static void CanCancelNewKanbanItem(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion
    }
}