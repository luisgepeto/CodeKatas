namespace SpellChecker
{
    public abstract class SpellCheckerOptions
    {
        public SpellCheckerOptions()
        {
            Language = Language.English;
        }
        public SpellCheckerOptions(Language language)
        {
            Language = language;
        }
        public Language Language { get; }
    }
}
