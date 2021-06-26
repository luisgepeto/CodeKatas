using SpellChecker.Interfaces;

namespace SpellChecker.Demo
{
    public class Program
    {
        static void Main(string[] args)
        {
            var options = new BloomFilterSpellCheckerOptions();
            ISpellChecker spellChecker = new BloomFilterSpellChecker(options);
        }
    }
}
