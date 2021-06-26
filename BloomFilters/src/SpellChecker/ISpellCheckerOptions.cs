using System;
using System.Collections.Generic;

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


        //TODO We might want to construct a language configuration object to pass extra options such as source file configuration.
        //Also to avoid shotgun surgery when adding a new language configuration
        public Language Language { get; }
        private Dictionary<Language, string> _sourceDictionaryRelativePath = new Dictionary<Language, string>()
        {
            { Language.English, ""},
            { Language.Spanish, ""}
        };
        public List<string> ReadSourceDictionary()
        {
            throw new NotImplementedException();
        }
    }
}
