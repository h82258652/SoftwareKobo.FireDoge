using System.Windows;
using System.Windows.Controls;

namespace SoftwareKobo.FireDoge.Controls
{
    public partial class AddressBar : TextBox
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(AddressBar), new PropertyMetadata(default(CornerRadius)));

        static AddressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AddressBar), new FrameworkPropertyMetadata(typeof(AddressBar)));
        }

        public CornerRadius CornerRadius
        {
            get
            {
                return (CornerRadius)GetValue(CornerRadiusProperty);
            }
            set
            {
                SetValue(CornerRadiusProperty, value);
            }
        }
    }
}