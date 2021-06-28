using System;

namespace SpellChecker
{
    public class BloomFilterSpellCheckerOptions : SpellCheckerOptions
    {
        public BloomFilterSpellCheckerOptions() : this(Language.English, 16) { }
        public BloomFilterSpellCheckerOptions(Language language, int? hashingFunctionsCount = null, bool? verifyFalsePositives = null, int? bitArrayLength = null) : base(language)
        {
            //TODO Since MD5 generates 16 bytes, we might generate extra hashing functions by recursively hashing
            if (hashingFunctionsCount <= 0 || hashingFunctionsCount > 16)
                throw new ApplicationException("The hashing functions count must be between 1 and 16");
            if (!hashingFunctionsCount.HasValue) hashingFunctionsCount = 16;
            HashingFunctionsCount = hashingFunctionsCount.Value;
            if (!verifyFalsePositives.HasValue) verifyFalsePositives = false;
            VerifyFalsePositives = verifyFalsePositives.Value;
            // Optimal size determined by equation m = (k * n) / ln(2) 
            // https://en.wikipedia.org/wiki/Bloom_filter#Probability_of_false_positives
            if (!bitArrayLength.HasValue) bitArrayLength = 7822454;
            BitArrayLength = bitArrayLength.Value;
        }
        public int HashingFunctionsCount { get; }
        public bool VerifyFalsePositives { get; }
        public int BitArrayLength { get; }
    }
}
