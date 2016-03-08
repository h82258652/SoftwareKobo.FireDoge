using SoftwareKobo.FireDoge.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace SoftwareKobo.FireDoge.Controls
{
    public delegate void AddressBarItemsSourceRequestedHandler(AddressBar addressBar, EventArgs e);

    [TemplatePart(Name = PopupPartName, Type = typeof(Popup))]
    [TemplatePart(Name = DropDownBoxContainerPartName, Type = typeof(Border))]
    [TemplatePart(Name = DropDownBoxPartName, Type = typeof(ListBox))]
    public partial class AddressBar
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(AddressBar), new PropertyMetadata(null));

        private const string DropDownBoxContainerPartName = @"PART_DropDownBoxContainer";

        private const string DropDownBoxPartName = @"PART_DropDownBox";

        private const string PopupPartName = @"PART_Popup";

        private ListBox _dropDownBox;

        private Border _dropDownBoxContainer;

        private Popup _popup;

        public event AddressBarItemsSourceRequestedHandler ItemsSourceRequested;

        public event EventHandler<AddressBarSubmitedEventArgs> Submited;

        internal static List<WeakReference<AddressBar>> Addressbars = new List<WeakReference<AddressBar>>();

        public AddressBar()
        {
            Addressbars.Add(new WeakReference<AddressBar>(this));
        }

        internal static void RemoveFocus()
        {
            // HACK hack 实现，因为 CefSharp 获得焦点时，这个控件的 LostFocus 并不会触发，Popup 无法关闭。
            for (int i = 0; i < Addressbars.Count; i++)
            {
                var barRef = Addressbars[i];
                AddressBar bar;
                if (barRef.TryGetTarget(out bar))
                {
                    bar.Dispatcher.Invoke(() =>
                    {
                        var popup = bar._popup;
                        if (popup != null)
                        {
                            popup.IsOpen = false;
                        }
                    });
                }
            }
        }

        public IEnumerable ItemsSource
        {
            get
            {
                return (IEnumerable)GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _popup = (Popup)GetTemplateChild(PopupPartName);
            _dropDownBoxContainer = (Border)GetTemplateChild(DropDownBoxContainerPartName);
            SyncDropDownBoxContainerStyle();
            _dropDownBox = (ListBox)GetTemplateChild(DropDownBoxPartName);
            if (_dropDownBox != null)
            {
                _dropDownBox.SelectionChanged += DropDownBox_SelectionChanged;
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (e.Key == Key.Enter)
            {
                Keyboard.ClearFocus();
                DoSubmit(Text);
            }
            else
            {
                ItemsSourceRequested?.Invoke(this, EventArgs.Empty);

                if (_popup != null)
                {
                    _popup.IsOpen = true;
                }
            }
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            // 控件失去焦点，隐藏 Popup。
            if (_popup != null)
            {
                _popup.IsOpen = false;
            }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == BorderThicknessProperty)
            {
                SyncDropDownBoxContainerStyle();
            }
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            ItemsSourceRequested?.Invoke(this, EventArgs.Empty);
        }

        private void DoSubmit(string address)
        {
            if (_popup != null)
            {
                _popup.IsOpen = false;
            }
            Submited?.Invoke(this, new AddressBarSubmitedEventArgs(address));
            Keyboard.ClearFocus();
        }

        private void DropDownBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var value = _dropDownBox?.SelectedValue?.ToString();
            if (value != null)
            {
                DoSubmit(value);
            }
        }

        private void SyncDropDownBoxContainerStyle()
        {
            if (_dropDownBoxContainer != null)
            {
                _dropDownBoxContainer.BorderThickness = new Thickness(BorderThickness.Left, 0, BorderThickness.Right, BorderThickness.Bottom);
                _dropDownBoxContainer.CornerRadius = new CornerRadius(0, 0, CornerRadius.BottomRight, CornerRadius.BottomLeft);
            }
        }
    }
}