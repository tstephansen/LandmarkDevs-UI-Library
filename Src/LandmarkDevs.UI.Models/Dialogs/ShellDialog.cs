namespace LandmarkDevs.UI.Models.Dialogs
{
    /// <summary>
    /// Class ShellDialog.
    /// </summary>
    public class ShellDialog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShellDialog"/> class.
        /// </summary>
        public ShellDialog() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ShellDialog"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        public ShellDialog(string title, string message)
        {
            Title = title;
            Message = message;
        }
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
}