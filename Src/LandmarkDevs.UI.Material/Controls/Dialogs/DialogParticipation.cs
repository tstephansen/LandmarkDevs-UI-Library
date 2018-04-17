using System;
using System.Collections.Generic;
using System.Windows;

namespace LandmarkDevs.UI.Material.Dialogs
{
    ///// <summary>
    /////     Class DialogParticipation.
    ///// </summary>
    //public static class DialogParticipation
    //{
    //    private static readonly IDictionary<object, DependencyObject> ContextRegistrationIndex =
    //        new Dictionary<object, DependencyObject>();

    //    /// <summary>
    //    ///     The register property
    //    /// </summary>
    //    public static readonly DependencyProperty RegisterProperty = DependencyProperty.RegisterAttached(
    //        "Register", typeof(object), typeof(DialogParticipation),
    //        new PropertyMetadata(default(object), RegisterPropertyChangedCallback));

    //    private static void RegisterPropertyChangedCallback(DependencyObject dependencyObject,
    //        DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    //    {
    //        if (dependencyPropertyChangedEventArgs.OldValue != null)
    //            ContextRegistrationIndex.Remove(dependencyPropertyChangedEventArgs.OldValue);

    //        if (dependencyPropertyChangedEventArgs.NewValue != null)
    //            ContextRegistrationIndex[dependencyPropertyChangedEventArgs.NewValue] = dependencyObject;
    //    }

    //    /// <summary>
    //    ///     Sets the register.
    //    /// </summary>
    //    /// <param name="element">The element.</param>
    //    /// <param name="context">The context.</param>
    //    public static void SetRegister(DependencyObject element, object context)
    //    {
    //        element.SetValue(RegisterProperty, context);
    //    }

    //    /// <summary>
    //    ///     Gets the register.
    //    /// </summary>
    //    /// <param name="element">The element.</param>
    //    /// <returns>System.Object.</returns>
    //    public static object GetRegister(DependencyObject element)
    //    {
    //        return element.GetValue(RegisterProperty);
    //    }

    //    /// <summary>
    //    ///     Determines whether the specified context is registered.
    //    /// </summary>
    //    /// <param name="context">The context.</param>
    //    /// <returns><c>true</c> if the specified context is registered; otherwise, <c>false</c>.</returns>
    //    /// <exception cref="System.ArgumentNullException">context</exception>
    //    internal static bool IsRegistered(object context)
    //    {
    //        if (context == null) throw new ArgumentNullException(nameof(context));
    //        return ContextRegistrationIndex.ContainsKey(context);
    //    }

    //    /// <summary>
    //    ///     Gets the association.
    //    /// </summary>
    //    /// <param name="context">The context.</param>
    //    /// <returns>DependencyObject.</returns>
    //    /// <exception cref="System.ArgumentNullException">context</exception>
    //    internal static DependencyObject GetAssociation(object context)
    //    {
    //        if (context == null) throw new ArgumentNullException(nameof(context));
    //        return ContextRegistrationIndex[context];
    //    }
    //}
}
