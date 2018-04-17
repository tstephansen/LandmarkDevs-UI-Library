using LandmarkDevs.UI.Models.Dialogs;
using Prism.Events;
using System.Diagnostics.CodeAnalysis;

namespace LandmarkDevs.UI.Material.Models
{
    /// <summary>
    ///     Shows a standard dialog to the user.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ShowDialogEvent : PubSubEvent<DialogModel>
    {
    }

    /// <summary>
    ///     Shows or hides the loading state on the main window.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ShellLoadingStateVisible : PubSubEvent<bool>
    {
    }

    /// <summary>
    ///     Shows or hides the navigation bar.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ChangeNavbarVisibilityEvent : PubSubEvent<bool>
    {
    }

    /// <summary>
    ///     Toggles help tooltips throughout the program.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ToggleHelpTooltipsEvent : PubSubEvent<bool>
    {
    }

    /// <summary>
    ///     Sets the text on the status bar.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SetStatusBarTextEvent : PubSubEvent<string>
    {
    }

    /// <summary>
    ///     Shows or hides the menu button.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SetMenuButtonVisibleEvent : PubSubEvent<bool>
    {
    }
}