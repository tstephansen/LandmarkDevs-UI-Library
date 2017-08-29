#region
using System;
#endregion

namespace LandmarkDevs.UI.Material.Models
{
    /// <summary>
    ///     Class ThemeSettingsModel.
    ///     Holds the settings for the application theme.
    /// </summary>
    [Serializable]
    public class ThemeSettingsModel
    {
        /// <summary>
        /// Gets or sets the name of the primary.
        /// </summary>
        /// <value>The name of the primary.</value>
        public string PrimaryName { get; set; }
        /// <summary>
        /// Gets or sets the name of the accent.
        /// </summary>
        /// <value>The name of the accent.</value>
        public string AccentName { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is dark.
        /// </summary>
        /// <value><c>true</c> if this instance is dark; otherwise, <c>false</c>.</value>
        public bool IsDark { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the foreground text is inverted.
        /// </summary>
        /// <value><c>true</c> if the foreground text is inverted; otherwise, <c>false</c>.</value>
        public bool TextForegroundInverted { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the foreground color is dark.
        /// </summary>
        /// <value><c>true</c> if the foreground color is dark; otherwise, <c>false</c>.</value>
        public bool ForegroundIsDark { get; set; }
    }
}