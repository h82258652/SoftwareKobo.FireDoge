using System.Windows;
using System.Windows.Input;

namespace SoftwareKobo.FireDoge.Controls
{
    // http://wpftoolkit.codeplex.com/SourceControl/latest#Main/Source/ExtendedWPFToolkitSolution/Src/Xceed.Wpf.Toolkit/AutoSelectTextBox/Implementation/AutoSelectTextBox.cs
    public partial class AddressBar
    {
        public static readonly DependencyProperty IsSelectAllOnFocusProperty = DependencyProperty.Register(nameof(IsSelectAllOnFocus), typeof(bool), typeof(AddressBar), new PropertyMetadata(default(bool)));

        public bool IsSelectAllOnFocus
        {
            get
            {
                return (bool)GetValue(IsSelectAllOnFocusProperty);
            }
            set
            {
                SetValue(IsSelectAllOnFocusProperty, value);
            }
        }

        protected override void OnPreviewGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnPreviewGotKeyboardFocus(e);

            if (IsSelectAllOnFocus)
            {
                SelectAll();
            }
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            if (IsSelectAllOnFocus && IsKeyboardFocusWithin == false)
            {
                Focus();
                e.Handled = true;
            }
        }
    }
}