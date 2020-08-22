using System;
using System.Collections.Generic;

namespace Groundbeef.Text
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public interface ITokenizer
    {
        string? Current { get; }
        bool First(ReadOnlySpan<char> token);
        bool Next(ReadOnlySpan<char> token);
    }

    /// <summary>
    /// Splits a string by tokens and enumerates the elements inbetween tokens.
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(true)]
    public class Tokenizer : ITokenizer
    {
        private readonly IEqualityComparer<char> _comparer;
        private readonly string _sourceText;
        private string? _token;
        private int _state;

        /// <summary>
        /// Initializes a new instance of <see cref="Tokenizer"/>.
        /// </summary>
        /// <param name="text">The source <see cref="String"/> to parse.</param>
        public Tokenizer(in string text)
        {
            _sourceText = text;
            _comparer = EqualityComparer<char>.Default;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Tokenizer"/>, with a specified <see cref="CharEqualityComparisions"/>.
        /// </summary>
        /// <param name="text">The source <see cref="String"/> to parse.</param>
        /// <param name="mode">The <see cref="CharEqualityComparisions"/> determining how two chars are compared</param>
        public Tokenizer(in string text, in CharEqualityComparisions mode)
        {
            _sourceText = text;
            _comparer = mode switch
            {
                CharEqualityComparisions.InvariantCaseInsensetive => new CharEqualityComparer_InvariantCaseInsensitive(),
                CharEqualityComparisions.CaseInsensitive => new CharEqualityComparer_CaseInsensitive(),
                _ => EqualityComparer<char>.Default,
            };
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Tokenizer"/>, with a specified <see cref="IEqualityComparer{Char}"/>.
        /// </summary>
        /// <param name="text">The source <see cref="String"/> to parse.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{Char}"/> used to determine if two chars are equal.</param>
        public Tokenizer(in string text, in IEqualityComparer<char> comparer)
        {
            _sourceText = text;
            _comparer = comparer;
        }

        /// <summary>
        /// Gets the current token, between the start of the previous consumed token and the end of the token before that.
        /// </summary>
        public string? Current => _token;

        /// <summary>
        /// Attempts to read the first token in the source text. 
        /// If tokens were already consumed resets state. 
        /// In case the token was not found does not change the state.
        /// </summary>
        /// <param name="token">The token searched in the source.</param>
        /// <returns><see cref="true"/> if the token was found; otherwise, <see cref="false"/>.</returns>
        public bool First(ReadOnlySpan<char> token)
        {
            _state = 0;
            return Next(token);
        }

        /// <summary>
        /// Attempts to read the next token in the source text. 
        /// In case the token was not found does not change the state.
        /// </summary>
        /// <param name="token">The token searched in the source.</param>
        /// <returns><see cref="true"/> if the token was found; otherwise, <see cref="false"/>.</returns>
        public bool Next(ReadOnlySpan<char> token)
        {
            ReadOnlySpan<char> openText = _sourceText.AsSpan(_state);
            int tknIndex = 0;
            int openIndex;
            // Read _openText until the token was found or the end is reached.
            for (openIndex = 0; openIndex < openText.Length && tknIndex < token.Length; openIndex++)
            {
                tknIndex = _comparer.Equals(openText[openIndex], token[tknIndex])
                 ? tknIndex + 1
                 : 0;
            }
            // The token was found in _openText
            if (tknIndex + 1 == token.Length)
            {
                _token = openText.Slice(0, openIndex - tknIndex).ToString();
                _state += openIndex;
                return true;
            }
            // Clear current token
            _token = null;
            return false;
        }

        /// <summary>
        /// Resets the state of the <see cref="Tokenizer"/>.
        /// </summary>
        public void Reset()
        {
            _state = 0;
            _token = null;
        }
    }
}