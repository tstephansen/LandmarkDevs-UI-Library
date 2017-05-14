#region
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

#endregion

namespace LandmarkDevs.UI.Common.Helpers
{
    /// <summary>
    ///     Class used to assist in creating animations.
    /// </summary>
    public class AnimationHelper
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AnimationHelper" /> class.
        /// </summary>
        protected AnimationHelper()
        {
        }

        /// <summary>
        ///     Creates a new Timeline.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="modifier"></param>
        /// <param name="duration"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        [SuppressMessage("SonarLint", "S1172:Unused method parameters should be removed")]
        public static Timeline DoubleAnimation(double from, double to, bool modifier, TimeSpan duration,
            DependencyProperty property)
        {
            DoubleAnimation animation = new DoubleAnimation();
            if (modifier)
            {
                animation.From = from;
                animation.To = to;
            }
            else
            {
                animation.To = from;
                animation.From = to;
            }
            animation.Duration = new Duration(duration);
            return animation;
        }

        /// <summary>
        ///     Creates a storyboard with a DoubleAnimation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <param name="modifier"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="duration"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static Storyboard CreateDoubleAnimation<T>(T control, bool modifier, double from, double to,
            TimeSpan duration, DependencyProperty property) where T : Control
        {
            return
                CreateDoubleAnimation<T>(control, modifier,
                    (p, a) =>
                    {
                        a.From = from;
                        a.To = to;
                    },
                    (p, a) =>
                    {
                        a.From = to;
                        a.To = from;
                    }, duration, property);
        }

        /// <summary>
        ///     Creates a storyboard with a DoubleAnimation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <param name="modifier"></param>
        /// <param name="onTrue"></param>
        /// <param name="onFalse"></param>
        /// <param name="duration"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static Storyboard CreateDoubleAnimation<T>(T control, bool modifier, Action<T, DoubleAnimation> onTrue,
            Action<T, DoubleAnimation> onFalse, TimeSpan duration, DependencyProperty property) where T : Control
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));
            DoubleAnimation panelAnimation = new DoubleAnimation();
            if (modifier)
            {
                onTrue?.Invoke(control, panelAnimation);
            }
            else
            {
                onFalse?.Invoke(control, panelAnimation);
            }
            panelAnimation.Duration = new Duration(duration);

            Storyboard sb = new Storyboard();
            Storyboard.SetTargetName(panelAnimation, control.Name);
            Storyboard.SetTargetProperty(panelAnimation, new PropertyPath(property));
            sb.Children.Add(panelAnimation);
            return sb;
        }

        /// <summary>
        ///     Creates a Timeline for an Int32Animation
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="modifier"></param>
        /// <param name="duration"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        [SuppressMessage("SonarLint", "S1172:Unused method parameters should be removed")]
        public static Timeline Int32Animation(Int32 from, Int32 to, bool modifier, TimeSpan duration,
            DependencyProperty property)
        {
            DoubleAnimation animation = new DoubleAnimation();
            if (modifier)
            {
                animation.From = from;
                animation.To = to;
            }
            else
            {
                animation.To = from;
                animation.From = to;
            }
            animation.Duration = new Duration(duration);
            return animation;
        }

        /// <summary>
        ///     Creates an Int32Animation storyboard.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <param name="modifier"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="duration"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        [SuppressMessage("SonarLint", "S1172:Unused method parameters should be removed")]
        public static Storyboard CreateInt32Animation<T>(T control, bool modifier, Int32 from, Int32 to,
            TimeSpan duration, DependencyProperty property) where T : Control
        {
            return
                CreateInt32Animation<T>(control, modifier,
                    (p, a) =>
                    {
                        a.From = from;
                        a.To = to;
                    },
                    (p, a) =>
                    {
                        a.From = to;
                        a.To = from;
                    }, duration, property);
        }

        /// <summary>
        ///     Creates an Int32Animation storyboard.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <param name="modifier"></param>
        /// <param name="onTrue"></param>
        /// <param name="onFalse"></param>
        /// <param name="duration"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static Storyboard CreateInt32Animation<T>(T control, bool modifier, Action<T, Int32Animation> onTrue,
            Action<T, Int32Animation> onFalse, TimeSpan duration, DependencyProperty property) where T : Control
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));
            Int32Animation panelAnimation = new Int32Animation();
            if (modifier)
            {
                onTrue?.Invoke(control, panelAnimation);
            }
            else
            {
                onFalse?.Invoke(control, panelAnimation);
            }
            panelAnimation.Duration = new Duration(duration);
            Storyboard sb = new Storyboard();
            Storyboard.SetTargetName(panelAnimation, control.Name);
            Storyboard.SetTargetProperty(panelAnimation, new PropertyPath(property));
            sb.Children.Add(panelAnimation);
            return sb;
        }
    }
}