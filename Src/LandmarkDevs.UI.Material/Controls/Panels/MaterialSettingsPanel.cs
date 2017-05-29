using LandmarkDevs.UI.Material.Controls.Windows;
using System.Windows;
using System.Windows.Controls;

namespace LandmarkDevs.UI.Material.Controls.Panels
{
    /// <summary>
    /// Class MaterialSettingsPanel.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.ContentControl" />
    [TemplatePart(Name = PART_CloseSettingsPanelButton, Type = typeof(Button))]
    public class MaterialSettingsPanel : ContentControl
    {
        static MaterialSettingsPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialSettingsPanel), new FrameworkPropertyMetadata(typeof(MaterialSettingsPanel)));
        }

        private const string PART_CloseSettingsPanelButton = "PART_CloseSettingsPanelButton";
        private Button _closeSettingsPanelButton;

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _closeSettingsPanelButton = GetTemplateChild(PART_CloseSettingsPanelButton) as Button;
            if (_closeSettingsPanelButton != null)
                _closeSettingsPanelButton.Click += (s, e) =>
                {
                    ((MaterialDesignWindow)Application.Current.MainWindow).SettingsPanelVisibility = Visibility.Collapsed;
                };
        }
    }
}