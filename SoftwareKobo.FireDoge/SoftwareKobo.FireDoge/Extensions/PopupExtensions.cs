using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace SoftwareKobo.FireDoge.Extensions
{
    // 让 Popup 跟随窗口移动。
    // http://stackoverflow.com/questions/1600218/how-can-i-move-a-wpf-popup-when-its-anchor-element-moves
    public static class PopupExtensions
    {
        public static readonly DependencyProperty IsMoveWithWindowProperty = DependencyProperty.RegisterAttached("IsMoveWithWindow", typeof(bool), typeof(PopupExtensions), new PropertyMetadata(false, IsMoveWithWindowChanged));

        private static readonly Dictionary<Popup, EventHandler> WindowLocationChangedHandlers = new Dictionary<Popup, EventHandler>();

        public static bool GetIsMoveWithWindow(Popup obj)
        {
            return (bool)obj.GetValue(IsMoveWithWindowProperty);
        }

        public static void SetIsMoveWithWindow(Popup obj, bool value)
        {
            obj.SetValue(IsMoveWithWindowProperty, value);
        }

        public static void UpdatePosition(this Popup popup)
        {
            if (popup == null)
            {
                throw new ArgumentNullException(nameof(popup));
            }

            if (popup.IsOpen)
            {
                var method = typeof(Popup).GetMethod("UpdatePosition", BindingFlags.NonPublic | BindingFlags.Instance);
                method.Invoke(popup, null);
            }
        }

        private static void IsMoveWithWindowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = Window.GetWindow(d);
            if (window == null)
            {
                return;
            }

            var popup = (Popup)d;
            var newValue = (bool)e.NewValue;
            if (newValue)
            {
                EventHandler handler = (sender, args) =>
                {
                    UpdatePosition(popup);
                };
                window.LocationChanged += handler;
                WindowLocationChangedHandlers[popup] = handler;
            }
            else
            {
                EventHandler handler;
                if (WindowLocationChangedHandlers.TryGetValue(popup, out handler))
                {
                    window.LocationChanged -= handler;
                }
            }
        }
    }
}