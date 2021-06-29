namespace SpellChecker.Interfaces
{
    public interface ISpellChecker
    {
        int WordCount { get; }
        SpellCheckResult Check(string text);
    }
}
