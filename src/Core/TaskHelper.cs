﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Groundbeef.Core
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public static class STAThreadTask
    {
        /// <summary>
        /// Returns a new STAThread spawned <see cref="Task{T}"/> for the provided <see cref="Func{T}"/>.
        /// </summary>
        /// <typeparam name="T">The return type of the <see cref="Task{T}"/>.</typeparam>
        /// <param name="func">The function.</param>
        /// <returns>A new STAThread spawned <see cref="Task{T}"/> for the provided <see cref="Func{T}"/>.</returns>
        public static Task<T> Run<T>(Func<T> func)
        {
            var tcs = new TaskCompletionSource<T>();
            var thread = new Thread(() =>
            {
                try
                {
                    tcs.SetResult(func());
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return tcs.Task;
        }

        /// <summary>
        /// Returns a new STAThread spawned <see cref="Task"/> for the provided <see cref="Action"/>.
        /// </summary>
        /// <param name="func">The function.</param>
        /// <returns>A new STAThread spawned <see cref="Task"/> for the provided <see cref="Action"/>.</returns>
        public static Task Run(Action func)
        {
            var tcs = new TaskCompletionSource<object?>();
            var thread = new Thread(() =>
            {
                try
                {
                    func();
                    tcs.SetResult(null);
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return tcs.Task;
        }
    }

    public static class TaskCollectionExtention
    {
        /// <summary>
        /// Waits for all of the provided System.Threading.Tasks.Task objects to complete execution.
        /// </summary>
        /// <param name="collection">The <see cref="Task[]"/> to await.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WaitAll(this Task[] collection) => Task.WaitAll(collection);

        /// <summary>
        /// Waits for all of the provided System.Threading.Tasks.Task objects to complete execution.
        /// </summary>
        /// <param name="collection">The <see cref="IReadOnlyList<Task>"/> to await.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WaitAll(this IReadOnlyList<Task> collection) => Task.WaitAll(collection.ToArray());

        /// <summary>
        /// Waits for any of the provided System.Threading.Tasks.Task objects to complete execution.
        /// </summary>
        /// <param name="collection">The <see cref="Task[]"/> to await.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WaitAny(this Task[] collection) => Task.WaitAny(collection);

        /// <summary>
        /// Waits for any of the provided System.Threading.Tasks.Task objects to complete execution.
        /// </summary>
        /// <param name="collection">The <see cref="IReadOnlyList<Task>"/> to await.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WaitAny(this IReadOnlyList<Task> collection) => Task.WaitAny(collection.ToArray());

        /// <summary>
        /// Creates a task that will complete when all of the System.Threading.Tasks.Task objects in an array have completed.
        /// </summary>
        /// <param name="collection">The <see cref="Task[]"/> to await.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task WhenAll(this Task[] collection) => Task.WhenAll(collection);

        /// <summary>
        /// Creates a task that will complete when all of the System.Threading.Tasks.Task objects in an array have completed.
        /// </summary>
        /// <param name="collection">The <see cref="IReadOnlyList<Task>"/> to await.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task WhenAll(this IReadOnlyList<Task> collection) => Task.WhenAll(collection.ToArray());

        /// <summary>
        /// Creates a task that will complete when any of the supplied tasks have completed.
        /// </summary>
        /// <param name="collection">The <see cref="Task[]"/> to await.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task WhenAny(this Task[] collection) => Task.WhenAny(collection);

        /// <summary>
        /// Creates a task that will complete when any of the supplied tasks have completed.
        /// </summary>
        /// <param name="collection">The <see cref="IReadOnlyList<Task>"/> to await.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task WhenAny(this IReadOnlyList<Task> collection) => Task.WhenAny(collection.ToArray());
    }
}
