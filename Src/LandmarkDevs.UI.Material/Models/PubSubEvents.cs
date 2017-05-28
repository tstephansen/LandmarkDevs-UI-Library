using Prism.Events;
using System.Diagnostics.CodeAnalysis;

namespace LandmarkDevs.UI.Material.Models
{
    /// <summary>
    /// Shows a standard dialog to the user.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class ShowDialogEvent : PubSubEvent<DialogModel>
    {
    }

    /// <summary>
    /// Shows or hides the loading state on the main window.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class ShellLoadingStateVisible : PubSubEvent<bool>
    {
    }

    /// <summary>
    /// Shows or hides the navigation bar.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class ChangeNavbarVisibilityEvent : PubSubEvent<bool>
    {
    }

    /// <summary>
    /// Toggles help tooltips throughout the program.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class ToggleHelpTooltipsEvent : PubSubEvent<bool>
    {
    }

    /// <summary>
    /// Sets the text on the status bar.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class SetStatusBarTextEvent : PubSubEvent<string>
    {
    }

    /// <summary>
    /// Shows or hides the menu button.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class SetMenuButtonVisibleEvent : PubSubEvent<bool>
    {
    }
}