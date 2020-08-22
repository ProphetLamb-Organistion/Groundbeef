namespace Groundbeef.Reflection
{
    /// <summary>
    /// Invokes the underlying method.
    /// </summary>
    /// <param name="obj">The instance the method is to be invoked on.</param>
    /// <param name="parameters">The parameters passed as arguments to the method.</param>
    /// <returns>The return value of the invokation of the underlying method.</returns>
    public delegate object? Invoker(object? obj, object?[]? parameters);
}
