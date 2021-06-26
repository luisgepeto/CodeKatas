using System.Collections.Generic;

namespace SpellChecker.Interfaces
{
    public interface ISpellChecker
    {
        Dictionary<string, bool> CheckByWords(string text);
    }
}
