using System.Threading.Tasks;

namespace SpellChecker.Interfaces
{
    public interface ISpellChecker
    {
        int WordCount { get; }
        Task<SpellCheckResult> CheckAsync(string text);
    }
}
