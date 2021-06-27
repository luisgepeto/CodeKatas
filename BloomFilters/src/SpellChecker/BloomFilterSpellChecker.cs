using System.Collections.Generic;
using System.Threading.Tasks;

using SpellChecker.Interfaces;

namespace SpellChecker
{
    public class BloomFilterSpellChecker : BaseSpellChecker, ISpellChecker
    {
        public static async Task<ISpellChecker> InitializeAsync(BloomFilterSpellCheckerOptions options)
        {
            var filter = new BloomFilterSpellChecker(options);
            await filter.LoadSourceDictionaryAsync();
            return filter;
        }

        private BloomFilterSpellChecker(BloomFilterSpellCheckerOptions options) : base(options)
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
