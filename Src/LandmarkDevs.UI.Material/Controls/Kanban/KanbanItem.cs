#region
using LandmarkDevs.UI.Material.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#endregion

namespace LandmarkDevs.UI.Material.Controls.Kanban
{
    /// <summary>
    ///     Class KanbanItem.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.ContentControl" />
    [TemplateVisualState(GroupName = "CommonStates", Name = NormalState)]
    [TemplateVisualState(GroupName = "CommonStates", Name = AddNoteState)]
    [TemplateVisualState(GroupName = "CommonStates", Name = EditItemState)]
    [TemplatePart(Name = PART_KanbanNotesListView, Type = typeof(ListView))]
    public class KanbanItem : ContentControl
    {
        static KanbanItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KanbanItem),
                new FrameworkPropertyMetadata(typeof(KanbanItem)));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="KanbanItem" /> class.
        /// </summary>
        public KanbanItem()
        {
            KanbanNotes = new ObservableCollection<KanbanNote>();
            KanbanPriorities = new List<KanbanPriority>
            {
                KanbanPriority.Critical,
                KanbanPriority.High,
                KanbanPriority.Normal,
                KanbanPriority.Low
            };
            CommandBindings.Add(new CommandBinding(KanbanItemCommands.AddNoteCommand, AddNewNote, CanAddNewNote));
            CommandBindings.Add(new CommandBinding(KanbanItemCommands.EditItemCommand, EditItem, CanEditItem));
            CommandBindings.Add(new CommandBinding(KanbanItemCommands.SaveNoteCommand, SaveNewNote, CanSaveNewNote));
            CommandBindings.Add(new CommandBinding(KanbanItemCommands.CancelNoteCommand, CancelNewNote, CanCancelNewNote));
            CommandBindings.Add(new CommandBinding(KanbanItemCommands.SaveItemChangesCommand, SaveItemChanges,
                CanSaveItemChanges));
            CommandBindings.Add(new CommandBinding(KanbanItemCommands.CancelItemChangesCommand, CancelItemChanges,
                CanCancelItemChanges));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="KanbanItem" /> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public KanbanItem(KanbanItem item)
        {
            Title = item.Title;
            Summary = item.Summary;
            Created = item.Created;
            CreatedBy = item.CreatedBy;
            Completed = item.Completed;
            Due = item.Due;
            IsComplete = item.IsComplete;
            KanbanNotes = item.KanbanNotes;
            KanbanPriority = item.KanbanPriority;
            PriorityLocation = item.PriorityLocation;
            Width = item.ActualWidth;
            Height = item.ActualHeight;
        }

        /// <summary>
        ///     When overridden in a derived class, is invoked whenever application code or internal processes call
        ///     <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            KanbanNotesListView = GetTemplateChild(PART_KanbanNotesListView) as ListView;
            GotoVisualState(NormalState, false);
        }

        private void GotoVisualState(string stateName, bool useTransitions = true)
        {
            var thisFe = this as FrameworkElement;
            VisualStateManager.GoToState(thisFe, stateName, useTransitions);
        }

        #region Template Properties
        private const string NormalState = "Normal";
        private const string EditItemState = "EditItem";
        private const string AddNoteState = "AddNote";
        private const string PART_KanbanNotesListView = "PART_KanbanNotesListView";

        /// <summary>
        ///     The kanban notes ListView
        /// </summary>
        internal ListView KanbanNotesListView;

        #endregion

        #region Dependency Properties
        /// <summary>
        ///     The title property
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(KanbanItem), new PropertyMetadata(default(string)));

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        ///     The summary property
        /// </summary>
        public static readonly DependencyProperty SummaryProperty = DependencyProperty.Register(
            "Summary", typeof(string), typeof(KanbanItem), new PropertyMetadata(default(string)));

        /// <summary>
        ///     Gets or sets the summary.
        /// </summary>
        /// <value>The summary.</value>
        public string Summary
        {
            get { return (string)GetValue(SummaryProperty); }
            set { SetValue(SummaryProperty, value); }
        }

        /// <summary>
        ///     The created by property
        /// </summary>
        public static readonly DependencyProperty CreatedByProperty = DependencyProperty.Register(
            "CreatedBy", typeof(string), typeof(KanbanItem), new PropertyMetadata(default(string)));

        /// <summary>
        ///     Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public string CreatedBy
        {
            get { return (string)GetValue(CreatedByProperty); }
            set { SetValue(CreatedByProperty, value); }
        }

        /// <summary>
        ///     The created property
        /// </summary>
        public static readonly DependencyProperty CreatedProperty = DependencyProperty.Register(
            "Created", typeof(DateTime), typeof(KanbanItem), new PropertyMetadata(default(DateTime)));

        /// <summary>
        ///     Gets or sets the created.
        /// </summary>
        /// <value>The created.</value>
        public DateTime Created
        {
            get { return (DateTime)GetValue(CreatedProperty); }
            set { SetValue(CreatedProperty, value); }
        }

        /// <summary>
        ///     The due property
        /// </summary>
        public static readonly DependencyProperty DueProperty = DependencyProperty.Register(
            "Due", typeof(DateTime), typeof(KanbanItem), new PropertyMetadata(default(DateTime)));

        /// <summary>
        ///     Gets or sets the due.
        /// </summary>
        /// <value>The due.</value>
        public DateTime Due
        {
            get { return (DateTime)GetValue(DueProperty); }
            set { SetValue(DueProperty, value); }
        }

        /// <summary>
        ///     The updated property
        /// </summary>
        public static readonly DependencyProperty UpdatedProperty = DependencyProperty.Register(
            "Updated", typeof(DateTime), typeof(KanbanItem), new PropertyMetadata(default(DateTime)));

        /// <summary>
        ///     Gets or sets the updated.
        /// </summary>
        /// <value>The updated.</value>
        public DateTime Updated
        {
            get { return (DateTime)GetValue(UpdatedProperty); }
            set { SetValue(UpdatedProperty, value); }
        }

        /// <summary>
        ///     The completed property
        /// </summary>
        public static readonly DependencyProperty CompletedProperty = DependencyProperty.Register(
            "Completed", typeof(DateTime?), typeof(KanbanItem), new PropertyMetadata(default(DateTime?)));

        /// <summary>
        ///     Gets or sets the completed.
        /// </summary>
        /// <value>The completed.</value>
        public DateTime? Completed
        {
            get { return (DateTime?)GetValue(CompletedProperty); }
            set { SetValue(CompletedProperty, value); }
        }

        /// <summary>
        ///     The is complete property
        /// </summary>
        public static readonly DependencyProperty IsCompleteProperty = DependencyProperty.Register(
            "IsComplete", typeof(bool), typeof(KanbanItem), new PropertyMetadata(default(bool)));

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is complete.
        /// </summary>
        /// <value><c>true</c> if this instance is complete; otherwise, <c>false</c>.</value>
        public bool IsComplete
        {
            get { return (bool)GetValue(IsCompleteProperty); }
            set { SetValue(IsCompleteProperty, value); }
        }

        /// <summary>
        ///     The kanban priority property
        /// </summary>
        public static readonly DependencyProperty KanbanPriorityProperty = DependencyProperty.Register(
            "KanbanPriority", typeof(KanbanPriority), typeof(KanbanItem), new PropertyMetadata(default(KanbanPriority)));

        /// <summary>
        ///     Gets or sets the kanban priority.
        /// </summary>
        /// <value>The kanban priority.</value>
        public KanbanPriority KanbanPriority
        {
            get { return (KanbanPriority)GetValue(KanbanPriorityProperty); }
            set { SetValue(KanbanPriorityProperty, value); }
        }

        /// <summary>
        ///     The kanban priorities property
        /// </summary>
        public static readonly DependencyProperty KanbanPrioritiesProperty = DependencyProperty.Register(
            "KanbanPriorities", typeof(IList<KanbanPriority>), typeof(KanbanItem));

        /// <summary>
        ///     Gets or sets the kanban priorities.
        /// </summary>
        /// <value>The kanban priorities.</value>
        public IList<KanbanPriority> KanbanPriorities
        {
            get { return (IList<KanbanPriority>)GetValue(KanbanPrioritiesProperty); }
            set { SetValue(KanbanPrioritiesProperty, value); }
        }

        /// <summary>
        ///     The kanban notes property
        /// </summary>
        public static readonly DependencyProperty KanbanNotesProperty = DependencyProperty.Register(
            "KanbanNotes", typeof(IList<KanbanNote>), typeof(KanbanItem));

        /// <summary>
        ///     Gets or sets the kanban notes.
        /// </summary>
        /// <value>The kanban notes.</value>
        public IList<KanbanNote> KanbanNotes
        {
            get { return (IList<KanbanNote>)GetValue(KanbanNotesProperty); }
            set { SetValue(KanbanNotesProperty, value); }
        }

        /// <summary>
        ///     The priority location property
        /// </summary>
        public static readonly DependencyProperty PriorityLocationProperty = DependencyProperty.Register(
            "PriorityLocation", typeof(PriorityColorLocation), typeof(KanbanItem),
            new PropertyMetadata(PriorityColorLocation.Bottom));

        /// <summary>
        ///     Gets or sets the priority location.
        /// </summary>
        /// <value>The priority location.</value>
        public PriorityColorLocation PriorityLocation
        {
            get { return (PriorityColorLocation)GetValue(PriorityLocationProperty); }
            set { SetValue(PriorityLocationProperty, value); }
        }

        /// <summary>
        ///     The new note text property
        /// </summary>
        public static readonly DependencyProperty NewNoteTextProperty = DependencyProperty.Register(
            "NewNoteText", typeof(string), typeof(KanbanItem), new PropertyMetadata(default(string)));

        /// <summary>
        ///     Gets or sets the new note text.
        /// </summary>
        /// <value>The new note text.</value>
        public string NewNoteText
        {
            get { return (string)GetValue(NewNoteTextProperty); }
            set { SetValue(NewNoteTextProperty, value); }
        }

        #endregion

        #region Routed Commands
        private static void AddNewNote(object sender, ExecutedRoutedEventArgs e)
        {
            var host = sender as KanbanItem;
            host.GotoVisualState(AddNoteState);
        }

        private static void CanAddNewNote(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private static void EditItem(object sender, ExecutedRoutedEventArgs e)
        {
            var host = sender as KanbanItem;
            host.GotoVisualState(EditItemState);
        }

        private static void CanEditItem(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private static void SaveNewNote(object sender, ExecutedRoutedEventArgs e)
        {
            var host = sender as KanbanItem;
            var item = host.DataContext as KanbanItemModel;
            var note = new KanbanNote();
            note.CreatedBy = item.CreatedBy;
            note.Time = DateTime.Now;
            note.Note = host.NewNoteText;
            if (item.Notes == null)
                item.Notes = new ObservableCollection<KanbanNote>();
            item.Notes.Add(note);
            host.KanbanNotes.Add(note);
            host.NewNoteText = "";
            host.GotoVisualState(EditItemState);
        }

        private static void CanSaveNewNote(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private static void CancelNewNote(object sender, ExecutedRoutedEventArgs e)
        {
            var host = sender as KanbanItem;
            host.GotoVisualState(EditItemState);
        }

        private static void CanCancelNewNote(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private static void CancelItemChanges(object sender, ExecutedRoutedEventArgs e)
        {
            var host = sender as KanbanItem;
            host.GotoVisualState(NormalState);
        }

        private static void CanCancelItemChanges(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private static void SaveItemChanges(object sender, ExecutedRoutedEventArgs e)
        {
            var host = sender as KanbanItem;
            host.GotoVisualState(NormalState);
        }

        private static void CanSaveItemChanges(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion
    }

    /// <summary>
    ///     Class KanbanItemCommands.
    /// </summary>
    public static class KanbanItemCommands
    {
        /// <summary>
        ///     Gets the edit item command.
        /// </summary>
        /// <value>The edit item command.</value>
        public static RoutedCommand EditItemCommand { get; } = new RoutedCommand();

        /// <summary>
        ///     Gets the add note command.
        /// </summary>
        /// <value>The add note command.</value>
        public static RoutedCommand AddNoteCommand { get; } = new RoutedCommand();

        /// <summary>
        ///     Gets the save note command.
        /// </summary>
        /// <value>The save note command.</value>
        public static RoutedCommand SaveNoteCommand { get; } = new RoutedCommand();

        /// <summary>
        ///     Gets the cancel note command.
        /// </summary>
        /// <value>The cancel note command.</value>
        public static RoutedCommand CancelNoteCommand { get; } = new RoutedCommand();

        /// <summary>
        ///     Gets the cancel item changes command.
        /// </summary>
        /// <value>The cancel item changes command.</value>
        public static RoutedCommand CancelItemChangesCommand { get; } = new RoutedCommand();

        /// <summary>
        ///     Gets the save item changes command.
        /// </summary>
        /// <value>The save item changes command.</value>
        public static RoutedCommand SaveItemChangesCommand { get; } = new RoutedCommand();
    }

    /// <summary>
    ///     Enum PriorityColorLocation
    /// </summary>
    public enum PriorityColorLocation
    {
        /// <summary>
        ///     The bottom
        /// </summary>
        Bottom = 0,

        /// <summary>
        ///     The left
        /// </summary>
        Left = 1
    }
}