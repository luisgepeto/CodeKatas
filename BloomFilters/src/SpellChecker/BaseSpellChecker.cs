using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using SpellChecker.Interfaces;

namespace SpellChecker
{
    public abstract class BaseSpellChecker : ISpellChecker
    {
        public int WordCount { get; private set; }
        protected SpellCheckerOptions _options;

        protected BaseSpellChecker(SpellCheckerOptions options)
        {
            _options = options;
        }
        protected virtual string GetSourceDictionaryFilePath()
        {
            return $"./SourceDictionaries/wordlist.{_options.Language.GetDescription()}.txt";
        }
        protected virtual async Task LoadSourceDictionaryAsync()
        {
            await ReadSanitizedFileAsync((line) =>
            {
                //TODO Should we care about multiple words in the same line?
                //TODO Should we care about lower case and upper case differences, as well as culture differences?
                LoadWord(line);
                WordCount++;
                return true;
            });
        }
        protected abstract void LoadWord(string word);

        public async Task<SpellCheckResult> CheckAsync(string text)
        {
            //TODO Verify all kinds of line breaks and symbols?
            var aggregateIndex = 0;
            var isWordFoundTasks = text.Split(" ").Select(async w =>
            {
                var startIndex = aggregateIndex;
                var length = w.Length;
                aggregateIndex += length + 1;
                var canCheck = !string.IsNullOrWhiteSpace(w.Replace("\r\n", ""));
                var sanitizedWord = w.ToLowerInvariant();
                var isFound = true;
                if (canCheck)
                    isFound = await CheckWordAsync(sanitizedWord);
                return (StartIndex: startIndex, Length: length, SanitizedWord: sanitizedWord, IsFound: isFound);
            }).ToArray();
            var areWordsFound = await Task.WhenAll(isWordFoundTasks);
            var notFoundWords = areWordsFound.Where(w => !w.IsFound).ToDictionary(r => r.StartIndex, r => (r.SanitizedWord, r.Length));
            return new SpellCheckResult(text, notFoundWords);
        }

        public abstract Task<bool> CheckWordAsync(string word);


        protected async Task ReadSanitizedFileAsync(Func<string, bool> callback)
        {
            var filePath = GetSourceDictionaryFilePath();
            using var file = new StreamReader(filePath);
            string nextLine;
            do
            {
                nextLine = await file.ReadLineAsync();
                if (!string.IsNullOrWhiteSpace(nextLine))
                {
                    if (!callback(nextLine.Trim().ToLowerInvariant()))
                        break;
                }
            }
            while (nextLine != null);
        }
    }
}
