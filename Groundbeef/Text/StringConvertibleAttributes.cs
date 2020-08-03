using System;
using System.Collections.Generic;
using System.Text;

namespace Groundbeef.Text
{
    /// <summary>
    /// Indicates that the method has public, static modifiers, has one <see cref="string"/> parameter, and returns a instance of the class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class FromString : Attribute
    {
    }

    /// <summary>
    /// Indicates that the method has public, static modifiers, has one parameter of the type of the class, and returns a <see cref="string"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class ToString : Attribute
    {
    }
}
