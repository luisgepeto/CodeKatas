using System.Collections.Generic;

using SpellChecker.Interfaces;

namespace SpellChecker
{
    public class BloomFilterSpellChecker : ISpellChecker
    {
        public BloomFilterSpellChecker(BloomFilterSpellCheckerOptions options)
        {
            
        }

        public Dictionary<string, bool> CheckByWords(string text)
        {
            throw new System.NotImplementedException();
        }
    }
}
