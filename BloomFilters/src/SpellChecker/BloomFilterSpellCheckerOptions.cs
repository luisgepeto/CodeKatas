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
            //TODO Since MD5 generates 16 bytes, we might generate extra hashing functions by recursively hashing
            // We use GetHashCode as default hashing function
            if (hashingFunctionsCount <= 0 || hashingFunctionsCount > 17)
                throw new ApplicationException("The hashing functions count must be between 1 and 17");
            // Optimal value determined by analysis summary
            if (!hashingFunctionsCount.HasValue) hashingFunctionsCount = 1;
            HashingFunctionsCount = hashingFunctionsCount.Value;
            if (!verifyFalsePositives.HasValue) verifyFalsePositives = false;
            VerifyFalsePositives = verifyFalsePositives.Value;
            // Optimal value determined by analysis summary
            if (!bitArrayLength.HasValue) bitArrayLength = 10000000;
            BitArrayLength = bitArrayLength.Value;
        }
    }
}
