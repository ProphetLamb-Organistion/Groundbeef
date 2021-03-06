using System;

namespace Groundbeef.Core
{
    public static class ExceptionHelper
    {
        /// <summary>
        /// Performs an action in the try-catch block.
        /// </summary>
        /// <param name="action">The action which will be invoked.</param>
        /// <param name="handler">Custom exception handler.</param>
        public static void Try(this Action action, Action<Exception>? handler = default) => Try<Exception>(action, handler);

        /// <summary>
        /// Performs an action in the try-catch block.
        /// </summary>
        /// <typeparam name="T">Intercepted exception type.</typeparam>
        /// <param name="action">The action which will be invoked.</param>
        /// <param name="handler">Custom exception handler.</param>
        public static void Try<T>(this Action action, Action<T>? handler) where T : Exception
        {
            try
            {
                action();
            }
            catch (T ex)
            {
                handler?.Invoke(ex);
            }
        }
    }
}