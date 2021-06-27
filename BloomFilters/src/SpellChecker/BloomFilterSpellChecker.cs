using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using SpellChecker.Interfaces;

namespace SpellChecker
{
    // Based on https://en.wikipedia.org/wiki/Bloom_filter
    public class BloomFilterSpellChecker : BaseSpellChecker, ISpellChecker
    {
        public bool[] BitArray { get; }
        private int _hashingFunctionsCount { get; }
        //TODO Implement Verification for false positives where dictionary is accessed
        private bool _verifyFalsePositives { get; }
        public static async Task<ISpellChecker> InitializeAsync(BloomFilterSpellCheckerOptions options)
        {
            var filter = new BloomFilterSpellChecker(options);
            await filter.LoadSourceDictionaryAsync();
            return filter;
        }

        private BloomFilterSpellChecker(BloomFilterSpellCheckerOptions options) : base(options)
        {
            //TODO Investigate this size. 
            // Since the max number an MD5 hash can generate is 255, does it make sense to have more bits?
            BitArray = new bool[256];
            _hashingFunctionsCount = options.HashingFunctionsCount;
            _verifyFalsePositives = options.VerifyFalsePositives;
        }

        public SpellCheckResult Check(string text)
        {
            //TODO Verify all kinds of line breaks
            var aggregateIndex = 0;
            var notFoundWords = text.Split(" ").Select(w =>
            {
                var startIndex = aggregateIndex;
                var length = w.Length;
                aggregateIndex += length + 1;
                var canCheck = !string.IsNullOrWhiteSpace(w.Replace("\r\n", ""));
                var sanitizedWord = w.ToLowerInvariant();
                var isFound = true;
                if (canCheck)
                    isFound = CheckWord(sanitizedWord);
                return (StartIndex: startIndex, Length: length, SanitizedWord: sanitizedWord, IsFound: isFound);
            }).Where(w => !w.IsFound).ToDictionary(r => r.StartIndex, r => (r.SanitizedWord, r.Length));
            return new SpellCheckResult(text, notFoundWords);
        }

        public bool CheckWord(string word)
        {
            var bitsToHash = GetWordHash(word);
            var isFound = true;
            foreach (var bitToHash in bitsToHash)
            {
                if (!BitArray[bitToHash])
                {
                    isFound = false;
                    break;
                }
            }
            return isFound;
        }

        protected override void LoadWord(string word)
        {
            var bitsToHash = GetWordHash(word);
            bitsToHash.ForEach(b =>
            {
                BitArray[b] = true;
            });
        }

        public List<byte> GetWordHash(string word)
        {
            using MD5 md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(word);
            var hashBytes = md5.ComputeHash(inputBytes);
            return hashBytes.Take(_hashingFunctionsCount).ToList();
        }
    }
}
