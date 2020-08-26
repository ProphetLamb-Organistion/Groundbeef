using System.ComponentModel;
using System.Windows;

namespace Groundbeef.Localisation
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public static class DesignHelper
    {
        private static bool? s_isInDesignMode;

        /// <summary>
        /// Gets a value indicating whether the control is in design mode (running in Blend or Visual Studio).
        /// </summary>
        public static bool IsInDesignModeStatic
        {
            get
            {
                if (!s_isInDesignMode.HasValue)
                {
#if SILVERLIGHT
                    _isInDesignMode = DesignerProperties.IsInDesignTool;
#else
                    var prop = DesignerProperties.IsInDesignModeProperty;
                    s_isInDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;
                    if (!s_isInDesignMode.Value && System.Diagnostics.Process.GetCurrentProcess().ProcessName.StartsWith(@"devenv"))
                        s_isInDesignMode = true;
#endif
                }
                return s_isInDesignMode.Value;
            }
        }
    }
}
