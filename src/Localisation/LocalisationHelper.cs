
using Groundbeef.SharedResources;

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Groundbeef.Localisation
{
    /// <summary>
    /// Helper class for binding to resource strings
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(true)]
    public class LocalisationHelper : INotifyPropertyChanged
    {
        private string? _defaultManager;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Initializes a new instance of the LocalisationHelper class.
        /// </summary>
        public LocalisationHelper()
        {
            if (!DesignHelper.IsInDesignModeStatic)
            {
                // Refresh all the bindings when the locale changes
                ResourceManagerService.LocaleChanged += (s, e) => RaisePropertyChanged(String.Empty);
            }
        }

        /// <summary>
        /// Gets a resource string from the ResourceManager 
        /// You can bind to this property using the .[KEY] syntax e.g.: 
        /// {Binding Source={StaticResource localisation}, Path=.[MainScreenResources.IntroTextLine1]}
        /// </summary>
        /// <param name="key">Key to retrieve in the format [ManagerName].[ResourceKey]</param>
        public string? this[string key]
        {
            get
            {
                bool isValidKey = ValidKey(key);
                if (DesignHelper.IsInDesignModeStatic)
                    return key;
                if (isValidKey)
                    return ResourceManagerService.GetResourceString(GetManagerKey(key), GetResourceKey(key));
                if (!(DefaultManager is null))
                    return ResourceManagerService.GetResourceString(DefaultManager, key);
                else
                    throw new ArgumentException(ExceptionResource.KEY_NOTFOUND, nameof(key));
            }
        }

        /// <summary>
        /// Gets or sets a string representing the default ResourceManager. 
        /// When set a resource string can be obtained without specifing a ManagerName, in that case the value of DefaultManager is used as such.
        /// </summary>
        public string? DefaultManager
        {
            get => _defaultManager;
            set
            {
                _defaultManager = value;
                RaisePropertyChanged();
            }
        }

        #region Private Key Methods
        private bool ValidKey(string input)
        {
            return input.Contains(".");
        }

        private string GetManagerKey(string input)
        {
            return input.Split('.')[0];
        }

        private string GetResourceKey(string input)
        {
            return input.Substring(input.IndexOf('.') + 1);
        }
        #endregion
    }
}
