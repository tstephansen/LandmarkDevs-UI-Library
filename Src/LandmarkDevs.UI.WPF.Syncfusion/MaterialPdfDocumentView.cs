#region
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Windows.PdfViewer;
#endregion

namespace LandmarkDevs.UI.WPF.Syncfusion
{
    /// <summary>
    ///     Class MaterialPdfDocumentView.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.ContentControl" />
    [TemplatePart(Name = PART_OpenFileButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_SaveFileButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_NextPageButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_PreviousPageButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_FirstPageButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_LastPageButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_CurrentPageTextBox, Type = typeof(TextBox))]
    [TemplatePart(Name = PART_ZoomLevelComboBox, Type = typeof(ComboBox))]
    [TemplatePart(Name = PART_PdfDocumentViewer, Type = typeof(PdfDocumentView))]
    [TemplatePart(Name = PART_ZoomInButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_ZoomOutButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_FitToPageWidthButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_FitToPageButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_PrintButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_TotalPagesTextBox, Type = typeof(TextBox))]
    public class MaterialPdfDocumentView : ContentControl
    {
        /// <summary>
        ///     Overrides the default style key of the document view.
        /// </summary>
        static MaterialPdfDocumentView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialPdfDocumentView), new FrameworkPropertyMetadata(typeof(MaterialPdfDocumentView)));
        }

        /// <summary>
        ///     This is called when the template is applied to the controls.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            NextPageButton = GetTemplateChild(PART_NextPageButton) as Button;
            PreviousPageButton = GetTemplateChild(PART_PreviousPageButton) as Button;
            FirstPageButton = GetTemplateChild(PART_FirstPageButton) as Button;
            LastPageButton = GetTemplateChild(PART_LastPageButton) as Button;
            OpenFileButton = GetTemplateChild(PART_OpenFileButton) as Button;
            SaveFileButton = GetTemplateChild(PART_SaveFileButton) as Button;
            CurrentPageTextBox = GetTemplateChild(PART_CurrentPageTextBox) as TextBox;
            ZoomLevelComboBox = GetTemplateChild(PART_ZoomLevelComboBox) as ComboBox;
            PdfDocumentViewer = GetTemplateChild(PART_PdfDocumentViewer) as PdfDocumentView;
            ZoomInButton = GetTemplateChild(PART_ZoomInButton) as Button;
            ZoomOutButton = GetTemplateChild(PART_ZoomOutButton) as Button;
            FitToPageWidthButton = GetTemplateChild(PART_FitToPageWidthButton) as Button;
            FitToPageButton = GetTemplateChild(PART_FitToPageButton) as Button;
            TotalPagesTextBox = GetTemplateChild(PART_TotalPagesTextBox) as TextBox;
            PrintButton = GetTemplateChild(PART_PrintButton) as Button;

            if (FirstPageButton != null)
                FirstPageButton.Click += FirstPageButton_Click;
            if (LastPageButton != null)
                LastPageButton.Click += LastPageButton_Click;
            if (NextPageButton != null)
                NextPageButton.Click += NextPageButton_Click;
            if (PreviousPageButton != null)
                PreviousPageButton.Click += PreviousPageButton_Click;
            if (CurrentPageTextBox != null)
            {
                CurrentPageTextBox.KeyUp += CurrentPageTextBox_KeyUp;
                CurrentPageTextBox.Text = "1";
            }
            if (OpenFileButton != null)
                OpenFileButton.Click += OpenFileButton_Click;
            if (SaveFileButton != null)
                SaveFileButton.Click += SaveFileButton_Click;

            if (PrintButton != null)
                PrintButton.Click += PrintButton_Click;
            if (ZoomLevelComboBox != null)
                ZoomLevelComboBox.SelectionChanged += ZoomLevelComboBox_SelectionChanged;
            if (ZoomInButton != null)
            {
                IsZoomInButtonEnabled = true;
                ZoomInButton.Click += ZoomInButton_Click;
            }
            if (ZoomOutButton != null)
            {
                IsZoomOutButtonEnabled = true;
                ZoomOutButton.Click += ZoomOutButton_Click;
            }
            if (FitToPageButton != null)
                FitToPageButton.Click += FitToPageButton_Click;
            if (FitToPageWidthButton != null)
                FitToPageWidthButton.Click += FitToPageWidthButton_Click;
            if (PdfDocumentViewer != null)
            {
                PdfDocumentViewer.Loaded += PdfDocumentViewer_Loaded;
                PdfDocumentViewer.CurrentPageChanged += PdfDocumentViewer_CurrentPageChanged;
                PdfDocumentViewer.ScrollChanged += PdfDocumentViewer_ScrollChanged;
                PdfDocumentViewer.Loaded += PdfDocumentViewer_Loaded;
            }
            IsFirstPageButtonEnabled = false;
            IsPreviousPageButtonEnabled = false;
        }

        private async void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Task.Run(() => { PdfDocumentViewer.Print(); });
            }
            catch (InvalidPrinterException pex)
            {
                Debug.WriteLine(pex.Message.Trim());
                throw new Exception("NoPrintersInstalledException");
                //var mw = Application.Current.MainWindow as MaterialDesignWindow;
                //await mw.ShowDialogAsync("No Printers Installed", "Please make sure you have a printer installed on your computer.", DialogStyle.Ok);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.Trim());
                throw new Exception("UnknownOperationException");
                //var mw = Application.Current.MainWindow as MaterialDesignWindow;
                //await mw.ShowDialogAsync("Something went wrong!",
                //    "Oh no! Something went wrong and your request was not able to be processed. Please try again and if you continue to have issues contact Tim Stephansen at x2617.",
                //    DialogStyle.Ok);
            }
        }

        #region Template Values
        // ReSharper disable InconsistentNaming
        private const string PART_OpenFileButton = "PART_OpenFileButton";

        private const string PART_SaveFileButton = "PART_SaveFileButton";
        private const string PART_CurrentPageTextBox = "PART_CurrentPageTextBox";
        private const string PART_NextPageButton = "PART_NextPageButton";
        private const string PART_PreviousPageButton = "PART_PreviousPageButton";
        private const string PART_FirstPageButton = "PART_FirstPageButton";
        private const string PART_LastPageButton = "PART_LastPageButton";
        private const string PART_ZoomLevelComboBox = "PART_ZoomLevelComboBox";
        private const string PART_PdfDocumentViewer = "PART_PdfDocumentViewer";
        private const string PART_ZoomInButton = "PART_ZoomInButton";
        private const string PART_ZoomOutButton = "PART_ZoomOutButton";
        private const string PART_FitToPageWidthButton = "PART_FitToPageWidthButton";
        private const string PART_FitToPageButton = "PART_FitToPageButton";
        private const string PART_PrintButton = "PART_PrintButton";
        private const string PART_TotalPagesTextBox = "PART_TotalPagesTextBox";
        // ReSharper restore InconsistentNaming
        internal Button OpenFileButton;

        internal Button PrintButton;
        internal Button SaveFileButton;
        internal TextBox CurrentPageTextBox;
        internal Button NextPageButton;
        internal Button PreviousPageButton;
        internal Button FirstPageButton;
        internal Button LastPageButton;
        internal ComboBox ZoomLevelComboBox;
        internal PdfDocumentView PdfDocumentViewer;
        internal Button ZoomInButton;
        internal Button ZoomOutButton;
        internal Button FitToPageWidthButton;
        internal Button FitToPageButton;
        internal TextBox TotalPagesTextBox;
        #endregion

        #region Dependency Properties
        /// <summary>
        ///     The toolbar background brush property
        /// </summary>
        public static readonly DependencyProperty ToolbarBackgroundBrushProperty = DependencyProperty.Register(
            "ToolbarBackgroundBrush", typeof(SolidColorBrush), typeof(MaterialPdfDocumentView), new PropertyMetadata(default(SolidColorBrush)));

        /// <summary>
        ///     Gets or sets the toolbar background brush.
        /// </summary>
        /// <value>The toolbar background brush.</value>
        public SolidColorBrush ToolbarBackgroundBrush
        {
            get { return (SolidColorBrush)GetValue(ToolbarBackgroundBrushProperty); }
            set { SetValue(ToolbarBackgroundBrushProperty, value); }
        }

        /// <summary>
        ///     The document stream property
        /// </summary>
        public static readonly DependencyProperty DocumentStreamProperty = DependencyProperty.Register(
            "DocumentStream", typeof(Stream), typeof(MaterialPdfDocumentView), new PropertyMetadata(default(Stream)));

        /// <summary>
        ///     Gets or sets the document stream.
        /// </summary>
        /// <value>The document stream.</value>
        public Stream DocumentStream
        {
            get { return (Stream)GetValue(DocumentStreamProperty); }
            set { SetValue(DocumentStreamProperty, value); }
        }

        #region Button Enabled Properties
        /// <summary>
        ///     The is open button enabled property
        /// </summary>
        public static readonly DependencyProperty IsOpenButtonEnabledProperty = DependencyProperty.Register(
            "IsOpenButtonEnabled", typeof(bool), typeof(MaterialPdfDocumentView), new FrameworkPropertyMetadata(false,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsOpenButtonEnabledChanged));

        /// <summary>
        ///     Gets or sets a value indicating whether the open button is enabled.
        /// </summary>
        /// <value><c>true</c> if the open button is enabled; otherwise, <c>false</c>.</value>
        public bool IsOpenButtonEnabled
        {
            get { return (bool)GetValue(IsOpenButtonEnabledProperty); }
            set { SetValue(IsOpenButtonEnabledProperty, value); }
        }

        /// <summary>
        ///     Determines whether [is open button enabled changed] [the specified sender].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void IsOpenButtonEnabledChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;
            ((MaterialPdfDocumentView)sender).OpenFileButton.IsEnabled = (bool)e.NewValue;
        }

        /// <summary>
        ///     The is save button enabled property
        /// </summary>
        public static readonly DependencyProperty IsSaveButtonEnabledProperty = DependencyProperty.Register(
            "IsSaveButtonEnabled", typeof(bool), typeof(MaterialPdfDocumentView), new FrameworkPropertyMetadata(true,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsSaveButtonEnabledChanged));

        /// <summary>
        ///     Gets or sets a value indicating whether the save button is enabled.
        /// </summary>
        /// <value><c>true</c> if the save button is enabled; otherwise, <c>false</c>.</value>
        public bool IsSaveButtonEnabled
        {
            get { return (bool)GetValue(IsSaveButtonEnabledProperty); }
            set { SetValue(IsSaveButtonEnabledProperty, value); }
        }

        /// <summary>
        ///     Determines whether [is save button enabled changed] [the specified sender].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void IsSaveButtonEnabledChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;
            ((MaterialPdfDocumentView)sender).SaveFileButton.IsEnabled = (bool)e.NewValue;
        }

        /// <summary>
        ///     The is first page button enabled property
        /// </summary>
        public static readonly DependencyProperty IsFirstPageButtonEnabledProperty = DependencyProperty.Register(
            "IsFirstPageButtonEnabled", typeof(bool), typeof(MaterialPdfDocumentView),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsFirstPageButtonEnabledChanged));

        /// <summary>
        ///     Gets or sets a value indicating whether the first page button is enabled.
        /// </summary>
        /// <value><c>true</c> if the first page button is enabled; otherwise, <c>false</c>.</value>
        public bool IsFirstPageButtonEnabled
        {
            get { return (bool)GetValue(IsFirstPageButtonEnabledProperty); }
            set { SetValue(IsFirstPageButtonEnabledProperty, value); }
        }

        /// <summary>
        ///     Determines whether [is first page button enabled changed] [the specified sender].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void IsFirstPageButtonEnabledChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;
            ((MaterialPdfDocumentView)sender).FirstPageButton.IsEnabled = (bool)e.NewValue;
        }

        /// <summary>
        ///     The is previous page button enabled property
        /// </summary>
        public static readonly DependencyProperty IsPreviousPageButtonEnabledProperty = DependencyProperty.Register(
            "IsPreviousPageButtonEnabled", typeof(bool), typeof(MaterialPdfDocumentView),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsPreviousPageButtonEnabledChanged));

        /// <summary>
        ///     Gets or sets a value indicating whether the previous page button is enabled.
        /// </summary>
        /// <value><c>true</c> if the previous page button is enabled; otherwise, <c>false</c>.</value>
        public bool IsPreviousPageButtonEnabled
        {
            get { return (bool)GetValue(IsPreviousPageButtonEnabledProperty); }
            set { SetValue(IsPreviousPageButtonEnabledProperty, value); }
        }

        /// <summary>
        ///     Determines whether [is previous page button enabled changed] [the specified sender].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void IsPreviousPageButtonEnabledChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;
            ((MaterialPdfDocumentView)sender).PreviousPageButton.IsEnabled = (bool)e.NewValue;
        }

        /// <summary>
        ///     The is next page button enabled property
        /// </summary>
        public static readonly DependencyProperty IsNextPageButtonEnabledProperty = DependencyProperty.Register(
            "IsNextPageButtonEnabled", typeof(bool), typeof(MaterialPdfDocumentView),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsNextPageButtonEnabledChanged));

        /// <summary>
        ///     Gets or sets a value indicating whether the next page button is enabled.
        /// </summary>
        /// <value><c>true</c> if the next page button is enabled; otherwise, <c>false</c>.</value>
        public bool IsNextPageButtonEnabled
        {
            get { return (bool)GetValue(IsNextPageButtonEnabledProperty); }
            set { SetValue(IsNextPageButtonEnabledProperty, value); }
        }

        /// <summary>
        ///     Determines whether [is next page button enabled changed] [the specified sender].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void IsNextPageButtonEnabledChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;
            ((MaterialPdfDocumentView)sender).NextPageButton.IsEnabled = (bool)e.NewValue;
        }

        /// <summary>
        ///     The is last page button enabled property
        /// </summary>
        public static readonly DependencyProperty IsLastPageButtonEnabledProperty = DependencyProperty.Register(
            "IsLastPageButtonEnabled", typeof(bool), typeof(MaterialPdfDocumentView),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsLastPageButtonEnabledChanged));

        /// <summary>
        ///     Gets or sets a value indicating whether the last page button is enabled.
        /// </summary>
        /// <value><c>true</c> if the last page button is enabled; otherwise, <c>false</c>.</value>
        public bool IsLastPageButtonEnabled
        {
            get { return (bool)GetValue(IsLastPageButtonEnabledProperty); }
            set { SetValue(IsLastPageButtonEnabledProperty, value); }
        }

        /// <summary>
        ///     Determines whether [is last page button enabled changed] [the specified sender].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void IsLastPageButtonEnabledChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;
            ((MaterialPdfDocumentView)sender).LastPageButton.IsEnabled = (bool)e.NewValue;
        }

        /// <summary>
        ///     The is zoom out button enabled property
        /// </summary>
        public static readonly DependencyProperty IsZoomOutButtonEnabledProperty = DependencyProperty.Register(
            "IsZoomOutButtonEnabled", typeof(bool), typeof(MaterialPdfDocumentView),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsZoomOutButtonEnabledChanged));

        /// <summary>
        ///     Gets or sets a value indicating whether the zoom out button is enabled.
        /// </summary>
        /// <value><c>true</c> if the zoom out button is enabled; otherwise, <c>false</c>.</value>
        public bool IsZoomOutButtonEnabled
        {
            get { return (bool)GetValue(IsZoomOutButtonEnabledProperty); }
            set { SetValue(IsZoomOutButtonEnabledProperty, value); }
        }

        /// <summary>
        ///     Determines whether [is zoom out button enabled changed] [the specified sender].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void IsZoomOutButtonEnabledChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;
            ((MaterialPdfDocumentView)sender).ZoomOutButton.IsEnabled = (bool)e.NewValue;
        }

        /// <summary>
        ///     The is zoom in button enabled property
        /// </summary>
        public static readonly DependencyProperty IsZoomInButtonEnabledProperty = DependencyProperty.Register(
            "IsZoomInButtonEnabled", typeof(bool), typeof(MaterialPdfDocumentView),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsZoomInButtonEnabledChanged));

        /// <summary>
        ///     Gets or sets a value indicating whether the zoom in button is enabled.
        /// </summary>
        /// <value><c>true</c> if the zoom in button is enabled; otherwise, <c>false</c>.</value>
        public bool IsZoomInButtonEnabled
        {
            get { return (bool)GetValue(IsZoomInButtonEnabledProperty); }
            set { SetValue(IsZoomInButtonEnabledProperty, value); }
        }

        /// <summary>
        ///     Determines whether [is zoom in button enabled changed] [the specified sender].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void IsZoomInButtonEnabledChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;
            ((MaterialPdfDocumentView)sender).ZoomInButton.IsEnabled = (bool)e.NewValue;
        }

        #endregion

        #region Zoom Properties
        /// <summary>
        ///     The zoom values property
        /// </summary>
        public static readonly DependencyProperty ZoomValuesProperty = DependencyProperty.Register(
            "ZoomValues", typeof(List<string>), typeof(MaterialPdfDocumentView));

        /// <summary>
        ///     Gets or sets the zoom values.
        /// </summary>
        /// <value>The zoom values.</value>
        public List<string> ZoomValues
        {
            get { return (List<string>)GetValue(ZoomValuesProperty); }
            set { SetValue(ZoomValuesProperty, value); }
        }

        /// <summary>
        ///     The selected zoom percent property
        /// </summary>
        public static readonly DependencyProperty SelectedZoomPercentProperty = DependencyProperty.Register(
            "SelectedZoomPercent", typeof(int), typeof(MaterialPdfDocumentView), new FrameworkPropertyMetadata(100,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedZoomPercentChanged));

        /// <summary>
        ///     Gets or sets the selected zoom percent.
        /// </summary>
        /// <value>The selected zoom percent.</value>
        public int SelectedZoomPercent
        {
            get { return (int)GetValue(SelectedZoomPercentProperty); }
            set { SetValue(SelectedZoomPercentProperty, value); }
        }

        /// <summary>
        ///     Handles the <see cref="E:SelectedZoomPercentChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnSelectedZoomPercentChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;
            ((MaterialPdfDocumentView)sender).SelectedZoomPercentChanged();
        }

        /// <summary>
        ///     Called when the selected zoom percent is changed.
        /// </summary>
        private void SelectedZoomPercentChanged()
        {
            PdfDocumentViewer.ZoomTo(SelectedZoomPercent);
        }

        #endregion

        #region Page Number Properties
        /// <summary>
        ///     The current page number property
        /// </summary>
        public static readonly DependencyProperty CurrentPageNumberProperty = DependencyProperty.Register(
            "CurrentPageNumber", typeof(int), typeof(MaterialPdfDocumentView), new FrameworkPropertyMetadata(1,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCurrentPageNumberChanged));

        /// <summary>
        ///     Gets or sets the current page number.
        /// </summary>
        /// <value>The current page number.</value>
        public int CurrentPageNumber
        {
            get { return (int)GetValue(CurrentPageNumberProperty); }
            set { SetValue(CurrentPageNumberProperty, value); }
        }

        /// <summary>
        ///     Handles the <see cref="E:CurrentPageNumberChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        public static void OnCurrentPageNumberChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((MaterialPdfDocumentView)sender).CurrentPageNumberChanged();
        }

        /// <summary>
        ///     Sets the current page number.
        /// </summary>
        private void CurrentPageNumberChanged()
        {
            CurrentPageTextBox.Text = PdfDocumentViewer.CurrentPageIndex.ToString();
            if (PdfDocumentViewer.CurrentPageIndex == 1)
            {
                IsFirstPageButtonEnabled = false;
                IsPreviousPageButtonEnabled = false;
            }
            else
            {
                IsFirstPageButtonEnabled = true;
                IsPreviousPageButtonEnabled = true;
            }
            if (PdfDocumentViewer.CurrentPageIndex == PdfDocumentViewer.PageCount)
            {
                IsNextPageButtonEnabled = false;
                IsLastPageButtonEnabled = false;
            }
            else
            {
                IsNextPageButtonEnabled = true;
                IsLastPageButtonEnabled = true;
            }
        }

        /// <summary>
        ///     The total pages property
        /// </summary>
        public static readonly DependencyProperty TotalPagesProperty = DependencyProperty.Register(
            "TotalPages", typeof(int), typeof(MaterialPdfDocumentView), new FrameworkPropertyMetadata(default(int),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTotalPagesChanged));

        /// <summary>
        ///     Handles the <see cref="E:TotalPagesChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void OnTotalPagesChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((MaterialPdfDocumentView)sender).TotalPagesChanged();
        }

        /// <summary>
        ///     Called when the total number of pages has changed.
        /// </summary>
        private void TotalPagesChanged()
        {
            TotalPagesTextBox.Text = TotalPages.ToString();
        }

        /// <summary>
        ///     Gets or sets the total pages.
        /// </summary>
        /// <value>The total pages.</value>
        public int TotalPages
        {
            get { return (int)GetValue(TotalPagesProperty); }
            set { SetValue(TotalPagesProperty, value); }
        }

        #endregion

        #endregion

        #region Methods
        /// <summary>
        ///     Handles the Loaded event of the PdfDocumentViewer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.ArgumentNullException">@The PdfDocumentView is null.</exception>
        private void PdfDocumentViewer_Loaded(object sender, EventArgs e)
        {
            var view = sender as PdfDocumentView;
            if (view == null)
                throw new ArgumentNullException(nameof(view), @"The PdfDocumentView is null.");
            if (DocumentStream == null)
                return;
            view.Load(DocumentStream);
            TotalPages = view.PageCount;
            CurrentPageNumber = 1;
            CurrentPageTextBox.Text = "1";
            view.GoToFirstPage();
            view.ZoomTo(100);
            var itemsArray = ZoomLevelComboBox.ItemsSource as int[];
            Debug.Assert(itemsArray != null, "itemsArray != null");
            var items = itemsArray.ToList();
            var index = items.FindIndex(c => c == 100);
            ZoomLevelComboBox.SelectedIndex = index;
        }

        /// <summary>
        ///     Handles the ScrollChanged event of the PdfDocumentViewer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.ArgumentNullException">@The PdfDocumentView is null.</exception>
        private void PdfDocumentViewer_ScrollChanged(object sender, EventArgs e)
        {
            var view = sender as PdfDocumentView;
            if (view == null)
                throw new ArgumentNullException(nameof(view), @"The PdfDocumentView is null.");
            if (CurrentPageNumber != view.CurrentPageIndex)
                CurrentPageNumber = view.CurrentPageIndex;
        }

        /// <summary>
        ///     Handles the CurrentPageChanged event of the PdfDocumentViewer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.ArgumentNullException">@The PdfDocumentView is null.</exception>
        private void PdfDocumentViewer_CurrentPageChanged(object sender, EventArgs e)
        {
            var view = sender as PdfDocumentView;
            if (view == null)
                throw new ArgumentNullException(nameof(view), @"The PdfDocumentView is null.");
            CurrentPageNumber = view.CurrentPageIndex;
        }

        /// <summary>
        ///     Handles the KeyUp event of the CurrentPageTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        private void CurrentPageTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null)
                throw new ArgumentNullException(nameof(textBox));
            if (e.Key != Key.Enter)
                return;
            if (string.IsNullOrWhiteSpace(textBox.Text))
                return;
            if (!Regex.IsMatch(textBox.Text, "^[0-9]+$"))
                return;
            var pageIndex = Convert.ToInt32(textBox.Text);
            if (pageIndex > PdfDocumentViewer.PageCount || pageIndex == 0)
                return;
            PdfDocumentViewer.GoToPageAtIndex(pageIndex);
        }

        /// <summary>
        ///     Handles the Click event of the NextPageButton control.
        ///     Goes to the next page if possible.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (!PdfDocumentViewer.CanGoToNextPage)
                return;
            PdfDocumentViewer.GoToNextPage();
        }

        /// <summary>
        ///     Handles the Click event of the PreviousPageButton control.
        ///     Goes to the previous page if possible.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (!PdfDocumentViewer.CanGoToPreviousPage)
                return;
            PdfDocumentViewer.GoToPreviousPage();
        }

        /// <summary>
        ///     Handles the Click event of the FirstPageButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void FirstPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (!PdfDocumentViewer.CanGoToFirstPage)
                return;
            PdfDocumentViewer.GoToFirstPage();
        }

        /// <summary>
        ///     Handles the Click event of the LastPageButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void LastPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (!PdfDocumentViewer.CanGoToLastPage)
                return;
            PdfDocumentViewer.GoToLastPage();
        }

        /// <summary>
        ///     Handles the Click event of the OpenFileButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog { Filter = "Pdf Files (.pdf)|*.pdf" };
            dialog.ShowDialog();
            if (string.IsNullOrEmpty(dialog.FileName))
                return;
            var pdf = new PdfLoadedDocument(dialog.FileName);
            PdfDocumentViewer.Load(pdf);
        }

        /// <summary>
        ///     Handles the Click event of the SaveFileButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            var saveDialog = new SaveFileDialog
            {
                Filter = "PDF Document (*.pdf)|*.pdf|All files (*.*)|*.*",
                FilterIndex = 0,
                FileName = "PdfDocument",
                RestoreDirectory = true,
                OverwritePrompt = true
            };
            var saveFile = saveDialog.ShowDialog();
            if (saveFile != true)
                return;
            var pdf = PdfDocumentViewer.LoadedDocument;
            pdf.Save(saveDialog.FileName);
        }

        /// <summary>
        ///     Handles the SelectionChanged event of the ZoomLevelComboBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        private void ZoomLevelComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var box = sender as ComboBox;
            if (box == null)
                throw new ArgumentNullException(nameof(box));
            var zoom = box.SelectedValue;
            if (zoom == null)
                return;
            PdfDocumentViewer.ZoomTo((int)zoom);
        }

        /// <summary>
        ///     Handles the Click event of the ZoomInButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void ZoomInButton_Click(object sender, RoutedEventArgs e)
        {
            var itemsArray = ZoomLevelComboBox.ItemsSource as int[];
            Debug.Assert(itemsArray != null, "itemsArray != null");
            var items = itemsArray.ToList();
            var zoomBoxSelectedValue = ZoomLevelComboBox.SelectedValue;
            if (zoomBoxSelectedValue == null)
            {
                var zoomIndex = items.FindIndex(c => c == 100);
                ZoomLevelComboBox.SelectedIndex = zoomIndex;
                PdfDocumentViewer.ZoomTo(100);
                return;
            }
            var zoom = (int)zoomBoxSelectedValue;
            var index = items.FindIndex(c => c == zoom) - 1;
            if (index < 0)
                return;
            var newZoomLevel = items.ElementAt(index);
            ZoomLevelComboBox.SelectedIndex = index;
            PdfDocumentViewer.ZoomTo(newZoomLevel);
        }

        /// <summary>
        ///     Handles the Click event of the ZoomOutButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void ZoomOutButton_Click(object sender, RoutedEventArgs e)
        {
            var itemsArray = ZoomLevelComboBox.ItemsSource as int[];
            Debug.Assert(itemsArray != null, "itemsArray != null");
            var items = itemsArray.ToList();
            var zoomBoxSelectedValue = ZoomLevelComboBox.SelectedValue;
            if (zoomBoxSelectedValue == null)
            {
                var zoomIndex = items.FindIndex(c => c == 100);
                ZoomLevelComboBox.SelectedIndex = zoomIndex;
                PdfDocumentViewer.ZoomTo(100);
                return;
            }
            var zoom = (int)zoomBoxSelectedValue;
            var index = items.FindIndex(c => c == zoom) + 1;
            if (index < 0)
                return;
            var newZoomLevel = items.ElementAt(index);
            ZoomLevelComboBox.SelectedIndex = index;
            PdfDocumentViewer.ZoomTo(newZoomLevel);
        }

        /// <summary>
        ///     Handles the Click event of the FitToPageButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void FitToPageButton_Click(object sender, RoutedEventArgs e)
        {
            PdfDocumentViewer.ZoomMode = ZoomMode.FitPage;
            ZoomLevelComboBox.Text = PdfDocumentViewer.ZoomPercentage + "%";
        }

        /// <summary>
        ///     Handles the Click event of the FitToPageWidthButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void FitToPageWidthButton_Click(object sender, RoutedEventArgs e)
        {
            PdfDocumentViewer.ZoomMode = ZoomMode.FitWidth;
            ZoomLevelComboBox.Text = PdfDocumentViewer.ZoomPercentage + "%";
        }

        #endregion
    }
}