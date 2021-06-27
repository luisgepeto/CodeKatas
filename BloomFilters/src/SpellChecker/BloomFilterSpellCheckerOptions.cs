using System;

namespace SpellChecker
{
    public class BloomFilterSpellCheckerOptions : SpellCheckerOptions
    {
        public BloomFilterSpellCheckerOptions() : this(Language.English, 16) { }
        public BloomFilterSpellCheckerOptions(Language language, int? hashingFunctionsCount = null, bool? verifyFalsePositives = null) : base(language)
        {
            //TODO Since MD5 generates 16 bytes, how could we generate more than 16 hashing functions?
            if (hashingFunctionsCount <= 0 || hashingFunctionsCount > 16)
                throw new ApplicationException("The hashing functions count must be between 1 and 16");
            if (!hashingFunctionsCount.HasValue) hashingFunctionsCount = 16;
            HashingFunctionsCount = hashingFunctionsCount.Value;
            if (!verifyFalsePositives.HasValue) verifyFalsePositives = false;
            VerifyFalsePositives = verifyFalsePositives.Value;
        }
        public int HashingFunctionsCount { get; }
        public bool VerifyFalsePositives { get; }
    }
}
