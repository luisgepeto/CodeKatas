namespace SpellChecker
{
    public class BloomFilterSpellCheckerOptions : SpellCheckerOptions
    {
        public BloomFilterSpellCheckerOptions() : this(Language.English) { }
        public BloomFilterSpellCheckerOptions(Language language) : base(language) { }

        protected override void LoadWord(string word)
        {
            throw new System.NotImplementedException();
        }
    }
}
