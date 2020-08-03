using Groundbeef.Collections;

using System;

namespace Groundbeef.IO
{
    public readonly ref struct ResourceIndicator
    {
        private readonly ReadOnlySpan<char> _uri;
        private readonly ReadOnlySpan<Range> _blocks;
        private readonly bool _relativePath;

        internal ResourceIndicator(in ReadOnlySpan<char> uri, in ReadOnlySpan<Range> blocks, bool relativePath = false)
        {
            _uri = uri;
            _blocks = blocks;
            _relativePath = relativePath;
        }

        public ReadOnlySpan<char> this[int index]
        {
            get => _uri[_blocks[index]];
        }

        public override string ToString() => ToString('\\').ToString();

        public int Count => _blocks.Length;

        public int Length => _uri.Length;

        public ReadOnlySpan<char> ToString(char separator)
        {
            Span<char> result = new char[_uri.Length];
            _uri.CopyTo(result);
            for(int i = 0; i < _blocks.Length; i++)
                result[_blocks[i].End] = separator;
            return result;
        }

        public static ResourceIndicator FromString(in ReadOnlySpan<char> uri)
        {
            ReadOnlySpan<char> trimmed;
            bool relativePath = FileHelper.IsRelativePath(uri, out int startIndex);
            // Remove .\
            if (relativePath)
                trimmed = uri[startIndex..^0];
            else
                trimmed = uri;
            ReadOnlySpan<Range> ranges = trimmed.SplitWhere(c => c == '\\' || c == '/').ToSpan();
            return new ResourceIndicator(uri, ranges, relativePath);
        }
    }
}