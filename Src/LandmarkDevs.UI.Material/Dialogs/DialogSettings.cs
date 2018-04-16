using System.Threading;
using System.Windows;

namespace LandmarkDevs.UI.Material.Dialogs
{
    /// <summary>
    ///     Class DialogSettings.
    /// </summary>
    public class DialogSettings
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DialogSettings" /> class.
        /// </summary>
        public DialogSettings()
        {
            OkButtonText = "OK";
            NoButtonText = "No";
            CancelButtonText = "Cancel";
            ExtraButtonText = "Yes";
            CancellationToken = CancellationToken.None;
        }

        /// <summary>
        ///     Gets or sets the ok button text.
        /// </summary>
        /// <value>The ok button text.</value>
        public string OkButtonText { get; set; }

        /// <summary>
        ///     Gets or sets the no button text.
        /// </summary>
        /// <value>The no button text.</value>
        public string NoButtonText { get; set; }

        /// <summary>
        ///     Gets or sets the cancel button text.
        /// </summary>
        /// <value>The cancel button text.</value>
        public string CancelButtonText { get; set; }

        /// <summary>
        ///     Gets or sets the extra button text.
        /// </summary>
        /// <value>The extra button text.</value>
        public string ExtraButtonText { get; set; }

        /// <summary>
        ///     Gets or sets the cancellation token.
        /// </summary>
        /// <value>The cancellation token.</value>
        public CancellationToken CancellationToken { get; set; }

        /// <summary>
        ///     Gets or sets the dialog style.
        /// </summary>
        /// <value>The dialog style.</value>
        public DialogStyle DialogStyle { get; set; }

        /// <summary>
        ///     Gets or sets the custom resource dictionary.
        /// </summary>
        /// <value>The custom resource dictionary.</value>
        public ResourceDictionary CustomResourceDictionary { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [suppress default resources].
        /// </summary>
        /// <value><c>true</c> if [suppress default resources]; otherwise, <c>false</c>.</value>
        public bool SuppressDefaultResources { get; set; }
    }
}