using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace LandmarkDevs.UI.Material.Behaviors
{
    /// <summary>
    /// Class EventToCommandBehavior.
    /// </summary>
    /// <seealso cref="System.Windows.Interactivity.Behavior" />
    /// <seealso cref="System.Windows.FrameworkElement"/>
    public class EventToCommandBehavior : Behavior<FrameworkElement>
    {
        private Delegate _handler;
        private EventInfo _oldEvent;

        /// <summary>
        /// Gets or sets the event.
        /// </summary>
        /// <value>The event.</value>
        public string Event { get { return (string)GetValue(EventProperty); } set { SetValue(EventProperty, value); } }
        /// <summary>
        /// The event property
        /// </summary>
        public static readonly DependencyProperty EventProperty = DependencyProperty.Register("Event", typeof(string), typeof(EventToCommandBehavior), new PropertyMetadata(null, OnEventChanged));

        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        /// <value>The command.</value>
        public ICommand Command { get { return (ICommand)GetValue(CommandProperty); } set { SetValue(CommandProperty, value); } }
        /// <summary>
        /// The command property
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(EventToCommandBehavior), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a value indicating whether to pass the arguments (default: false).
        /// </summary>
        /// <value><c>true</c> if passing the arguments; otherwise, <c>false</c>.</value>
        public bool PassArguments { get { return (bool)GetValue(PassArgumentsProperty); } set { SetValue(PassArgumentsProperty, value); } }
        /// <summary>
        /// The pass arguments property
        /// </summary>
        public static readonly DependencyProperty PassArgumentsProperty = DependencyProperty.Register("PassArguments", typeof(bool), typeof(EventToCommandBehavior), new PropertyMetadata(false));

        private static void OnEventChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var beh = (EventToCommandBehavior)d;

            if (beh.AssociatedObject != null) // is not yet attached at initial load
                beh.AttachHandler((string)e.NewValue);
        }

        /// <summary>
        /// Called when [attached].
        /// </summary>
        protected override void OnAttached()
        {
            AttachHandler(Event); // initial set
        }

        /// <summary>
        /// Attaches the handler.
        /// </summary>
        /// <param name="eventName">Name of the event.</param>
        /// <exception cref="ArgumentException"></exception>
        private void AttachHandler(string eventName)
        {
            // detach old event
            if (_oldEvent != null)
                _oldEvent.RemoveEventHandler(AssociatedObject, _handler);

            // attach new event
            if (!string.IsNullOrEmpty(eventName))
            {
                var ei = AssociatedObject.GetType().GetEvent(eventName);
                if (ei != null)
                {
                    var mi = GetType().GetMethod("ExecuteCommand", BindingFlags.Instance | BindingFlags.NonPublic);
                    _handler = Delegate.CreateDelegate(ei.EventHandlerType, this, mi);
                    ei.AddEventHandler(AssociatedObject, _handler);
                    _oldEvent = ei; // store to detach in case the Event property changes
                }
                else
                    throw new ArgumentException(
                        $"The event '{eventName}' was not found on type '{AssociatedObject.GetType().Name}'");
            }
        }
    }
}
