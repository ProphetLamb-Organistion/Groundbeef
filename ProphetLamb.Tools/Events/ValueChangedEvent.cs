using System;

namespace ProphetLamb.Tools.Events
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public delegate void ValueChangedEventHandler<TValue>(object sender, ValueChangedEventArgs<TValue> e);

    [System.Runtime.InteropServices.ComVisible(true)]
    public class ValueChangedEventArgs<TValue> : EventArgs
    {
        public ValueChangedEventArgs(TValue oldValue, TValue newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public TValue OldValue { get; protected set; }
        public TValue NewValue { get; protected set; }
    }
}
