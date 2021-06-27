using System.IO;
using System.Threading.Tasks;

namespace SpellChecker
{
    public abstract class SpellCheckerOptions
    {
        public SpellCheckerOptions()
        {
            Language = Language.English;
        }
        public SpellCheckerOptions(Language language) : base()
        {
            Language = language;
        }


        /*TODO 
         * We might want to construct a language configuration object in order to pass 
         * extra options such as source file configuration, and handle dictionary 
         * word loading i.e. different languages might have different sources
         */
        public Language Language { get; }
        protected async Task LoadSourceDictionary()
        {
            var filePath = GetSourceDictionaryFilePath();
            using var file = new StreamReader(filePath);
            string nextLine;
            do
            {
                nextLine = await file.ReadLineAsync();
                if (!string.IsNullOrWhiteSpace(nextLine))
                {
                    //TODO Should we care about multiple words in the same line?
                    //TODO Should we care about lower case and upper case differences, as well as culture differences?
                    LoadWord(nextLine.Trim().ToLowerInvariant());
                }
            }
            while (nextLine != null);
        }
        protected string GetSourceDictionaryFilePath()
        {
            return $"./SourceDictionaries/wordlist.{Language.GetDescription()}.txt";
        }
        protected abstract void LoadWord(string word);
    }
}

