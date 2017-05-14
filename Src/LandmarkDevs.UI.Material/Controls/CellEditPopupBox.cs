using System.Windows;
using System.Windows.Controls;
// ReSharper disable InconsistentNaming

namespace LandmarkDevs.UI.Material.Controls
{
    /// <summary>
    /// Class CellEditPopupBox.
    /// </summary>
    /// <seealso cref="LandmarkDevs.UI.Material.Controls.PopupEx" />
    public class CellEditPopupBox : PopupEx
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CellEditPopupBox"/> class.
        /// </summary>
        public CellEditPopupBox()
        {
            CellPopup = new CellEditPopup();
            Child = CellPopup;
            UpdateLayout();
        }
        
        internal CellEditPopup CellPopup;
        
    }

    /// <summary>
    /// Class CellEditPopup.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.ContentControl" />
    [TemplatePart(Name = PART_CellEditTextBox, Type = typeof(TextBox))]
    public class CellEditPopup : ContentControl
    {
        static CellEditPopup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CellEditPopup), new FrameworkPropertyMetadata(typeof(CellEditPopup)));
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            CellEditTextBox = GetTemplateChild(PART_CellEditTextBox) as TextBox;
        }

        private const string PART_CellEditTextBox = "PART_CellEditTextBox";
        internal TextBox CellEditTextBox;

        /// <summary>
        /// The cell edit text property
        /// </summary>
        public static readonly DependencyProperty CellEditTextProperty = DependencyProperty.Register(
            "CellEditText", typeof(string), typeof(CellEditPopupBox), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCellEditTextChanged));

        /// <summary>
        /// Gets or sets the cell edit text.
        /// </summary>
        /// <value>The cell edit text.</value>
        public string CellEditText
        {
            get { return (string)GetValue(CellEditTextProperty); }
            set { SetValue(CellEditTextProperty, value); }
        }

        private static void OnCellEditTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((CellEditPopup)sender).CellEditTextBox.Text = e.NewValue.ToString();
        }
    }
}
