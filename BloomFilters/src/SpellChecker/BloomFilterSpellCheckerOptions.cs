using System;

namespace SpellChecker
{
    public class BloomFilterSpellCheckerOptions : SpellCheckerOptions
    {
        public int HashingFunctionsCount { get; }
        public bool VerifyFalsePositives { get; }
        public int BitArrayLength { get; }

        public BloomFilterSpellCheckerOptions() : this(Language.English, 16) { }
        public BloomFilterSpellCheckerOptions(Language language, int? hashingFunctionsCount = null, bool? verifyFalsePositives = null, int? bitArrayLength = null) : base(language)
        {
            if (hashingFunctionsCount <= 0)
                throw new ApplicationException("The hashing functions count must be greater than 0");
            // Optimal value determined as per reference and hardcoded error 0.1%
            // https://en.wikipedia.org/wiki/Bloom_filter#Optimal_number_of_hash_functions
            HashingFunctionsCount = hashingFunctionsCount ?? (int)Math.Ceiling(-Math.Log2(0.001));
            var dictionarySize = 338882;
            if (Language == Language.Spanish)
                dictionarySize = 42605;
            BitArrayLength = bitArrayLength ?? (int)Math.Ceiling((HashingFunctionsCount * dictionarySize) / Math.Log(2));
            VerifyFalsePositives = verifyFalsePositives ?? false;
        }
    }
}
