using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ProphetLamb.Tools.Core
{
    public static class AOT
    {
        private const BindingFlags methodBindings = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        public static void PreloadAssembly(Assembly assembly)
        {
            new Thread(() =>
            {
                Parallel.ForEach(assembly.GetTypes(), (Type type) =>
                {
                    foreach (MethodInfo method in type.GetMethods(methodBindings))
                        PreloadMethod(method);
                });
            })
            { Priority = ThreadPriority.Lowest }
            .Start();
        }

        public static bool PreloadMethod(MethodInfo method)
        {
            if ((method.Attributes & MethodAttributes.Abstract) == MethodAttributes.Abstract || method.ContainsGenericParameters)
                return false;
            System.Runtime.CompilerServices.RuntimeHelpers.PrepareMethod(method.MethodHandle);
            return true;
        }
    }
}