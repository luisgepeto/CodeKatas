using CommandLine;

namespace SpellChecker.Demo
{
    public class DemoOptions
    {
        [Option(Default = "en", HelpText = "Select the language for the spellchecker.")]
        public string Language { get; set; }
        [Option(HelpText = "A string to be spell checked.")]
        public string Text { get; set; }
        [Option(HelpText = "The file path to be spell checked")]
        public string FilePath { get; set; }
        public Language ParsedLanguage
        {
            get
            {
                return SpellChecker.Language.English;
            }
        }
    }
}
