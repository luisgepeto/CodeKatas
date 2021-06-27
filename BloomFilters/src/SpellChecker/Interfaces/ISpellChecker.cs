namespace SpellChecker.Interfaces
{
    public interface ISpellChecker
    {
        SpellCheckResult Check(string text);
    }
}
