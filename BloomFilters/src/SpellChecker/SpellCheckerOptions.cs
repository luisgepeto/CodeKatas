namespace SpellChecker
{
    public abstract class SpellCheckerOptions
    {
        /*TODO 
         * We might want to construct a language configuration object in order to pass 
         * extra options such as source file configuration, and handle dictionary 
         * word loading i.e. different languages might have different sources
         */
        public Language Language { get; }

        protected SpellCheckerOptions(Language? language = null)
        {
            if (!language.HasValue)
                language = Language.English;
            Language = language.Value;
        }
    }
}

