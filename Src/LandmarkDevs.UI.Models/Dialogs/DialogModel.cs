namespace LandmarkDevs.UI.Models.Dialogs
{
    /// <summary>
    /// Standard dialog model.
    /// </summary>
    public class DialogModel : IDialogModel
    {
        /// <summary>
        /// Create a new dialog model.
        /// </summary>
        /// <param name="title">The dialog title.</param>
        /// <param name="message">The dialog message.</param>
        /// <param name="style">The style of the dialog to be displayed.</param>
        /// <param name="settings">The settings for the dialog.</param>
        public DialogModel(string title, string message, DialogStyle style, DialogSettings settings)
		{
            Title = title;
            Message = message;
            Settings = settings;
            DialogStyle = style;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogModel"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="style">The style.</param>
        public DialogModel(string title, string message, DialogStyle style)
        {
            Title = title;
            Message = message;
            DialogStyle = style;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogModel"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        public DialogModel(string title, string message)
        {
            Title = title;
            Message = message;
            DialogStyle = DialogStyle.Ok;
        }

        /// <summary>
        /// Gets/Sets the dialog settings.
        /// </summary>
        public DialogSettings Settings { get; set; }
        /// <summary>
        /// Gets or sets the dialog style.
        /// </summary>
        /// <value>The dialog style.</value>
        public DialogStyle DialogStyle { get; set; }
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }
    }

    /// <summary>
    /// Interface IStandardDialogModel
    /// </summary>
    public interface IDialogModel
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        string Message { get; set; }
        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        /// <value>The settings.</value>
        DialogSettings Settings { get; set; }
        /// <summary>
        /// Gets or sets the dialog style.
        /// </summary>
        /// <value>The dialog style.</value>
        DialogStyle DialogStyle { get; set; }
    }
}