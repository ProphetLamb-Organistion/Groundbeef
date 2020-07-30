using System;

namespace Groundbeef.Events
{
    // Generic
    [System.Runtime.InteropServices.ComVisible(true)]
    public delegate void ValueChangedEventHandler<TValue>(object? sender, ValueChangedEventArgs<TValue> e);

    [System.Runtime.InteropServices.ComVisible(true)]
    public class ValueChangedEventArgs<TValue> : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ValueChangedEventArgs{T}"/>.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        public ValueChangedEventArgs(TValue oldValue, TValue newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        /// <summary>
        /// Represents the old value before the change.
        /// </summary>
        public TValue OldValue { get; protected set; }

        /// <summary>
        /// Represents the new value after the change.
        /// </summary>
        public TValue NewValue { get; protected set; }
    }

    // Typebased
    [System.Runtime.InteropServices.ComVisible(true)]
    public delegate void ValueChangedEventHandler(object? sender, ValueChangedEventArgs e);

    [System.Runtime.InteropServices.ComVisible(true)]
    public class ValueChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ValueChangedEventArgs"/>, determining the type by the provided values, hence both cannot be null and must be of the same Type.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <exception name="ArgumentException">both oldValue and newValue are null or type of oldValue is not type of newValue.</exception>
        public ValueChangedEventArgs(object? oldValue, object? newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
            Type? oldT = OldValue?.GetType(),
                  newT = NewValue?.GetType();
            if (oldT is null && newT is null)
                throw new ArgumentException("Could not determine the type of values, because oldValue and newValue are null.");
            if ((oldT is null ^ newT is null) || oldT == newT)
                ValuesType = oldT??newT??typeof(object);
            else throw new ArgumentException("The type of oldValue is different from the type of newValue.");
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ValueChangedEventArgs"/>.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="valuesType">The type of both the old- and new value.</param>
        public ValueChangedEventArgs(object? oldValue, object? newValue, Type valuesType)
        {
            OldValue = oldValue;
            NewValue = newValue;
            ValuesType = valuesType;
        }

        /// <summary>
        /// Gets or sets the old value before the change.
        /// </summary>
        public object? OldValue { get; set; }

        /// <summary>
        /// Gets or sets the new value after the change.
        /// </summary>
        public object? NewValue { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Type"/> of both the old- and new value.
        /// </summary>
        public Type ValuesType { get; set; }
    }
}
