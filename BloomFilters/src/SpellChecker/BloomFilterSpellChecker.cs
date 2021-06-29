using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using SpellChecker.Interfaces;

namespace SpellChecker
{
    // Based on https://en.wikipedia.org/wiki/Bloom_filter
    public class BloomFilterSpellChecker : BaseSpellChecker
    {
        public BitArray BitArray { get; }
        private int _hashingFunctionsCount { get; }
        private int _bitArrayLength { get; }
        private bool _verifyFalsePositives { get; }
        public static async Task<ISpellChecker> InitializeAsync(BloomFilterSpellCheckerOptions options)
        {
            var filter = new BloomFilterSpellChecker(options);
            await filter.LoadSourceDictionaryAsync();
            return filter;
        }

        protected BloomFilterSpellChecker(BloomFilterSpellCheckerOptions options) : base(options)
        {
            _bitArrayLength = options.BitArrayLength;
            BitArray = new BitArray(_bitArrayLength);
            _hashingFunctionsCount = options.HashingFunctionsCount;
            _verifyFalsePositives = options.VerifyFalsePositives;
        }

        public override async Task<bool> CheckWordAsync(string word)
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
            if (isFound && _verifyFalsePositives)
            {
                // We could have a list property that gets filled inside the LoadWord function, however the purpose of this Kata is to minimize memory, so we will open the file and search for the word.
                var isFoundInFile = false;
                await ReadSanitizedFileAsync((line) =>
                {
                    if (line == word)
                    {
                        isFoundInFile = true;
                        return false;
                    }
                    return true;
                });
                isFound = isFoundInFile;
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

        public List<int> GetWordHash(string word)
        {
            using MD5 md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(word);
            var md5Hash = md5.ComputeHash(inputBytes);
            return md5Hash.Take(_hashingFunctionsCount).Select((h, i) => new Random(h * i).Next(0, _bitArrayLength)).ToList();
        }
    }
}
