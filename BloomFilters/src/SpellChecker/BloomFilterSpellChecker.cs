using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using SpellChecker.Interfaces;

namespace SpellChecker
{
    // Based on https://en.wikipedia.org/wiki/Bloom_filter
    public class BloomFilterSpellChecker : BaseSpellChecker
    {
        private int _hashingFunctionsCount { get; }
        private int _bitArrayLength { get; }
        private bool _verifyFalsePositives { get; }
        public BitArray BitArray { get; }

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

        protected override void LoadWord(string word)
        {
            var bitsToHash = GetWordHash(word);
            bitsToHash.ForEach(b =>
            {
                BitArray[b] = true;
            });
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

        public List<int> GetWordHash(string word)
        {
            //Double hashing function as in https://www.khoury.northeastern.edu/~pete/pub/bloom-filters-verification.pdf
            var firstIndex = ComputeIndex(word);
            var hashList = new List<int>() { firstIndex };
            if (_hashingFunctionsCount > 1)
            {
                var secondIndex = ComputeIndex($"Double hash implementation for {word} of length {word.Length}");
                var previousIndex = firstIndex;
                for (var i = 1; i < _hashingFunctionsCount; i++)
                {
                    var currentIndex = (previousIndex + secondIndex) % _bitArrayLength;
                    hashList.Add(currentIndex);
                    previousIndex = currentIndex;
                }
            }
            return hashList;
        }

        private int ComputeIndex(string inputString)
        {
            using var sha256 = SHA256.Create();
            var inputBytes = Encoding.ASCII.GetBytes(inputString);
            var regularHash = sha256.ComputeHash(inputBytes);
            var index = Math.Abs(BitConverter.ToInt32(regularHash, 0)) % _bitArrayLength;
            return index;
        }
    }
}
