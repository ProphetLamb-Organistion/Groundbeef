using System;

namespace ProphetLamb.Tools.Text
{

    /// <summary>
    /// Splits a string by tokens and returns the elements inbetween tokens.
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(true)]
    public class BulkTokenizer
    {
        private readonly ITokenizer _tokenizer;
        private readonly string?[] _splitTokens;

        /// <summary>
        /// Initializes a new instacne of <see cref="BulkTokenizer{T}"/>, with a specified <see cref="ITokenizer"/>.
        /// </summary>
        /// <param name="tokenizer">The <see cref="ITokenizer"/> used.</param>
        /// <param name="tokens">The ordered <see cref="string?[]?"/> containing the tokens,</param>
        public BulkTokenizer(in ITokenizer tokenizer, in string?[] tokens)
        {
            _tokenizer = tokenizer;
            _splitTokens = tokens;
        }

        /// <summary>
        /// Initializes a new instacne of <see cref="BulkTokenizer{T}"/>, with the default <see cref="Tokenizer"/>.
        /// </summary>
        /// <param name="text">The source <see cref="string"/> to parse.</param>
        /// <param name="tokens">The ordered <see cref="string?[]?"/> </param>
        public BulkTokenizer(in string text, in string?[] tokens)
        {
            _tokenizer = new Tokenizer(text);
            _splitTokens = tokens;
        }

        /// <summary>
        /// Returns a <see cref="string?[]"/> with length equal to the number of tokens - 1, containing the elements inbetween all tokens. 
        /// If a token was not found the value at the index will be null, and the token will be skipped.
        /// </summary>
        /// <returns>A <see cref="string?[]"/> with length equal to the number of tokens - 1, containing the elements inbetween all tokens.</returns>
        public string?[] Tokenize()
        {
            var result = new string?[_splitTokens.Length - 1];
            _tokenizer.First(_splitTokens[0]);
            for (int i = 1; i < _splitTokens.Length; i++)
            {
                if (_splitTokens[i] is null)
                {
                    result[i-1] = null;
                }
                else
                {
                    _tokenizer.Next(_splitTokens[i]);
                    result[i-1] = _tokenizer.Current;
                }
            }
            return result;
        }
    }
}