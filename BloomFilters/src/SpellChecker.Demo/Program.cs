using CommandLine;

using SpellChecker.Interfaces;

namespace SpellChecker.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<DemoOptions>(args)
                   .WithParsed(o =>
                   {
                       CheckSpelling(o);
                   });
        }

        public static void CheckSpelling(DemoOptions demoOptions)
        {
            var options = new BloomFilterSpellCheckerOptions(demoOptions.ParsedLanguage);
            ISpellChecker spellChecker = new BloomFilterSpellChecker(options);
        }
    }
}
