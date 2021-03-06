using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Groundbeef.Core
{
    public static class AOT
    {
        private const BindingFlags MethodBindings = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        public static Thread PreloadAssembly(Assembly assembly)
        {
            var thread = new Thread(() =>
            {
                Parallel.ForEach(assembly.GetTypes(), (Type type) =>
                {
                    foreach (MethodInfo method in type.GetMethods(MethodBindings))
                        PreloadMethod(method);
                });
            });
            thread.Priority = ThreadPriority.Lowest;
            thread.Start();
            return thread;
        }

        public static bool PreloadMethod(in MethodInfo method)
        {
            if ((method.Attributes & MethodAttributes.Abstract) == MethodAttributes.Abstract || method.ContainsGenericParameters)
                return false;
            System.Runtime.CompilerServices.RuntimeHelpers.PrepareMethod(method.MethodHandle);
            return true;
        }
    }
}