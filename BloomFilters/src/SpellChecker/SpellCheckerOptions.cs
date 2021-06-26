using System;
using System.ComponentModel;

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
        public Language Language { get; }
        protected void LoadSourceDictionary(Action<string> loadAction)
        {
            throw new NotImplementedException();
        }

        protected string GetSourceDictionaryFilePath()
        {
            var descriptionAttribute = (DescriptionAttribute)typeof(Language).GetField(Language.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false)[0];
            return $"./SourceDictionaries/wordlist.{descriptionAttribute.Description}.txt";
        }
    }
}

