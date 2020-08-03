using System;

namespace Groundbeef.Events
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public delegate void PropertyChangedEventHandler<T>(object? sender, PropertyChangedEventArgs<T> e);

    [System.Runtime.InteropServices.ComVisible(true)]
    public class PropertyChangedEventArgs<T> : ValueChangedEventArgs<T>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ValueChangedEventArgs{T}"/>.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="propertyName">The name of the property changed.</param>
        public PropertyChangedEventArgs(T oldValue, T newValue, string propertyName) : base(oldValue, newValue)
        {
            Name = propertyName;
        }

        /// <summary>
        /// Gets or sets the name of the property changed.
        /// </summary>
        public string Name { get; protected set; }
    }

    [System.Runtime.InteropServices.ComVisible(true)]
    public delegate void PropertyChangedEventHandler(object? sender, PropertyChangedEventArgs e);

    [System.Runtime.InteropServices.ComVisible(true)]
    public class PropertyChangedEventArgs : ValueChangedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ValueChangedEventArgs"/>, determining the type by the provided values, hence both cannot be null and must be of the same Type.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <exception name="ArgumentException">both oldValue and newValue are null or type of oldValue is not type of newValue.</exception>
        /// <param name="propertyName">The name of the property changed.</param>
        public PropertyChangedEventArgs(object? oldValue, object? newValue, string propertyName) : base(oldValue, newValue)
        {
            Name = propertyName;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ValueChangedEventArgs"/>.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="valuesType">The type of both the old- and new value.</param>
        /// <param name="propertyName">The name of the property changed.</param>
        public PropertyChangedEventArgs(object? oldValue, object? newValue, Type valuesType, string propertyName) : base(oldValue, newValue, valuesType)
        {
            Name = propertyName;
        }

        /// <summary>
        /// Gets or sets the name of the property changed.
        /// </summary>
        public string Name { get; set; }
    }
}