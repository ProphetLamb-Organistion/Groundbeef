using System.Windows;

namespace Groundbeef.WPF
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public class BindingProxy : Freezable
    {
        public BindingProxy() { }
        public BindingProxy(object? value)
            => Value = value;

        protected override Freezable CreateInstanceCore()
            => new BindingProxy();

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(object), typeof(BindingProxy), new FrameworkPropertyMetadata(default));

        public object? Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
    }
}
