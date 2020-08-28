using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Groundbeef.Collections.Spans
{
    internal static unsafe class PInvoke
    {
        /// <summary>
        /// Copies a block of unmanaged memory.
        /// </summary>
        // Source: http://pinvoke.net/default.aspx/msvcrt/memcpy.html
        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        internal static extern IntPtr MemCpy(IntPtr dest, IntPtr src, UIntPtr count);

        /// <summary>
        /// Moves a block of maybe overlapping unmanaged memory.
        /// </summary>
        // Source: http://pinvoke.net/default.aspx/msvcrt/memmove.html
        [DllImport("msvcrt.dll", EntryPoint = "memmove", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr MemMove(IntPtr dest, IntPtr src, UIntPtr count);

        /// <summary>
        /// Assigns a specified value to a block of unmanaged memory.
        /// </summary>
        // Source: http://www.pinvoke.net/default.aspx/msvcrt/memset.html
        [DllImport("msvcrt.dll", EntryPoint = "memset", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        internal static extern IntPtr MemSet(IntPtr dest, int c, int byteCount);

        /// <summary>
        /// Copies a block of unmanaged memory.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void MemCpy(void* destination, void* source, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            MemCpy(destination, source, (uint)count);
        }

        /// <summary>
        /// Copies a block of unmanaged memory.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void MemCpy(void* destination, void* source, uint count)
        {
            MemCpy(new IntPtr(destination), new IntPtr(source), new UIntPtr(count));
        }

        /// <summary>
        /// Moves a block of maybe overlapping unmanaged memory.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void MemMove(void* destination, void* source, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            MemMove(new IntPtr(destination), new IntPtr(source), new UIntPtr((uint)count));
        }

        /// <summary>
        /// Moves a block of maybe overlapping unmanaged memory.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void MemMove(void* destination, void* source, uint count)
        {
            MemMove(new IntPtr(destination), new IntPtr(source), new UIntPtr(count));
        }

        /// <summary>
        /// Assigns a specified value to a block of unmanaged memory.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void MemSet(void* destination, char value, int byteCount)
        {
            if ((value & 0xFF) != value)
                throw new ArgumentException(nameof(value));
            MemSet(destination, value, byteCount);
        }

        /// <summary>
        /// Assigns a specified value to a block of unmanaged memory.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void MemSet(void* destination, byte value, int byteCount)
        {
            MemSet(new IntPtr(destination), value, byteCount);
        }
    }
}