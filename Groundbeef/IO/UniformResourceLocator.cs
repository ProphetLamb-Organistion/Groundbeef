using System.Linq;
using System.IO;
using System;

namespace Groundbeef.IO
{
    public ref struct UniformResourceLocator
    {
        private readonly ReadOnlySpan<string> _resourceLocation;
        private readonly byte _settings;

        public UniformResourceLocator(in ReadOnlySpan<string> resourceLocation, bool relativePath = false, char separator = '\\')
        {
            _resourceLocation = resourceLocation;
            if (separator != '\\' && separator != '/')
                throw new ArgumentException("Unsupported separator", nameof(separator));
            _settings = (byte)((relativePath ? 1 << 0 : 0) | (separator == '\\' ? 1 << 1 : 0));
        }

        public static UniformResourceLocator FromString(in ReadOnlySpan<char> filePath)
        {
            ReadOnlySpan<char> trimmed;
            bool relativePath = FileHelper.IsRelativePath(filePath, out int startIndex);
            // Remove .\
            if (relativePath)
                trimmed = filePath[startIndex..^0];
            else
                trimmed = filePath;
            throw new NotImplementedException();
        }
    }
}