﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace Groundbeef.WPF
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public class BindingTrigger : INotifyPropertyChanged
    {
        public BindingTrigger()
        {
            Binding = new Binding()
            {
                Source = this,
                Path = new PropertyPath(nameof(Value))
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public Binding Binding { get; }

        public void Refresh()
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));

        public object? Value { get; }
    }
}
