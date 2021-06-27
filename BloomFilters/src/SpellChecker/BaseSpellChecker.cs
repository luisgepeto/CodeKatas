using System.IO;
using System.Threading.Tasks;

namespace SpellChecker
{
    public abstract class BaseSpellChecker
    {
        protected SpellCheckerOptions _options;
        protected BaseSpellChecker(SpellCheckerOptions options)
        {
            _options = options;
        }
        protected string GetSourceDictionaryFilePath()
        {
            return $"./SourceDictionaries/wordlist.{_options.Language.GetDescription()}.txt";
        }
        protected virtual async Task LoadSourceDictionary()
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
        protected abstract void LoadWord(string word);
    }
}
