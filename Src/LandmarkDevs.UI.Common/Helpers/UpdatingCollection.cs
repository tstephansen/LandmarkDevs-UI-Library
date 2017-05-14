#region
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

#endregion

namespace LandmarkDevs.UI.Common.Helpers
{
    /// <summary>
    ///     Class UpdatingCollection.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Collections.ObjectModel.ObservableCollection{T}" />
    public class UpdatingCollection<T> : ObservableCollection<T>
    {
        /// <summary>
        /// </summary>
        public class ChildElementPropertyChangedEventArgs : EventArgs
        {
            /// <summary>
            ///     Gets or sets the child element.
            /// </summary>
            /// <value>
            ///     The child element.
            /// </value>
            public object ChildElement { get; set; }

            /// <summary>
            ///     Initializes a new instance of the <see cref="ChildElementPropertyChangedEventArgs" /> class.
            /// </summary>
            /// <param name="item">The item.</param>
            public ChildElementPropertyChangedEventArgs(object item)
            {
                ChildElement = item;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="e">The <see cref="ChildElementPropertyChangedEventArgs" /> instance containing the event data.</param>
        public delegate void ChildElementPropertyChangedEventHandler(ChildElementPropertyChangedEventArgs e);

        /// <summary>
        ///     Occurs when [child element property changed].
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event ChildElementPropertyChangedEventHandler ChildElementPropertyChanged;

        /// <summary>
        ///     Called when [child element property changed].
        /// </summary>
        /// <param name="childelement">The childelement.</param>
        private void OnChildElementPropertyChanged(object childelement)
        {
            ChildElementPropertyChanged?.Invoke(new ChildElementPropertyChangedEventArgs(childelement));
        }

        /// <summary>
        ///     Raises the <see cref="E:CollectionChanged" /> event.
        /// </summary>
        /// <param name="e">
        ///     The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs" /> instance containing
        ///     the event data.
        /// </param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var convertedItem in e.NewItems.OfType<INotifyPropertyChanged>())
                {
                    convertedItem.PropertyChanged += convertedItem_PropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var convertedItem in e.OldItems.OfType<INotifyPropertyChanged>())
                {
                    convertedItem.PropertyChanged -= convertedItem_PropertyChanged;
                }
            }
        }


        /// <summary>
        ///     Handles the PropertyChanged event of the convertedItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs" /> instance containing the event data.</param>
        [SuppressMessage("SonarLint", "S1172:Unused method parameters should be removed")]
        private void convertedItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnChildElementPropertyChanged(sender);
        }
    }
}