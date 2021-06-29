using System.Collections.Generic;

namespace SpellChecker
{
    public class SpellCheckResult
    {
        public string OriginalText { get; set; }
        public Dictionary<int, (string SanitizedWord, int Length)> ErrorsByStartIndex { get; set; }

        public SpellCheckResult(string originalText, Dictionary<int, (string, int)> errorsByStartIndex)
        {
            OriginalText = originalText;
            ErrorsByStartIndex = errorsByStartIndex;
        }
    }
}
