using System.Collections.Generic;

using SpellChecker.Interfaces;

namespace SpellChecker
{
    public class BloomFilterSpellChecker : BaseSpellChecker, ISpellChecker
    {
        public BloomFilterSpellChecker(BloomFilterSpellCheckerOptions options) : base(options)
        {

        }

        public Dictionary<string, bool> CheckByWords(string text)
        {
            throw new System.NotImplementedException();
        }

        protected override void LoadWord(string word)
        {
            throw new System.NotImplementedException();
        }
    }
}
