using System.IO;
using System.Linq;
using System.Threading.Tasks;

using SpellChecker.Interfaces;

namespace SpellChecker
{
    public abstract class BaseSpellChecker : ISpellChecker
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
        protected virtual async Task LoadSourceDictionaryAsync()
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

        public SpellCheckResult Check(string text)
        {
            //TODO Verify all kinds of line breaks and symbols?
            var aggregateIndex = 0;
            var notFoundWords = text.Split(" ").Select(w =>
            {
                var startIndex = aggregateIndex;
                var length = w.Length;
                aggregateIndex += length + 1;
                var canCheck = !string.IsNullOrWhiteSpace(w.Replace("\r\n", ""));
                var sanitizedWord = w.ToLowerInvariant();
                var isFound = true;
                if (canCheck)
                    isFound = CheckWord(sanitizedWord);
                return (StartIndex: startIndex, Length: length, SanitizedWord: sanitizedWord, IsFound: isFound);
            }).Where(w => !w.IsFound).ToDictionary(r => r.StartIndex, r => (r.SanitizedWord, r.Length));
            return new SpellCheckResult(text, notFoundWords);
        }

        public abstract bool CheckWord(string word);
    }
}
