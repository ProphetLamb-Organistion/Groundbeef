using ProphetLamb.Tools.Core;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProphetLamb.Tools.Localisation
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public class ResourceManager
    {
        internal static readonly string ResFileExtention = ".json";

        private string resourcePath;
        private CultureInfo _setCulture;
        private ConcurrentDictionary<String, ResourceSet> resSets;
        private ResourceSet currentSet;
        private bool currentSetLoaded;

        public bool CaseInsensitive { get; set; } = true;

        public string BaseName { get; }

        public CultureInfo SetCulture
        {
            get => _setCulture;
            set
            {
                if (_setCulture != value)
                {
                    _setCulture = value;
                    ChangeCulture(value);
                }
            }
        }

        private async void ChangeCulture(CultureInfo culture)
        {

        }

        private string GetResourceFileName(CultureInfo culture)
        {
            var sb = new StringBuilder(255);
            sb.Append(BaseName);
            // Culutre is not invariant culture
            if (!culture.Name.Equals(CultureInfo.InvariantCulture.Name, StringComparison.InvariantCulture))
            {
                // Call internal VerifyCultureName function with throw exception flag.
                culture.VerifyCultureName(true);
                sb.Append('.').Append(culture.Name);
            }
            sb.Append(ResFileExtention);
            return sb.ToString();
        }
    }
}
