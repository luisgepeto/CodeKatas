using System;
using System.Collections.Generic;
using System.Linq;

using CommandLine;

namespace SpellChecker.Demo
{
    public class DemoOptions
    {
        [Option('l', "language", Default = "en", HelpText = "Select the language for the spellchecker. Available options: 'en' (English) and 'es' (Spanish)")]
        public string Language { get; set; }
        [Option('t', "text", HelpText = "A string to be spell checked.")]
        public string Text { get; set; }
        [Option('d', "diagnostics", HelpText = "Run performance diagnostics")]
        public bool RunDiagnostics { get; set; }
        //TODO We could add options to read from file        
        public Language ParsedLanguage
        {
            get
            {
                _ = TryParseLanguage(Language, out Language language);
                return language;
            }
        }

        public bool IsValid(out List<string> errorMessages)
        {
            errorMessages = new List<string>();
            if (!RunDiagnostics && string.IsNullOrEmpty(Text))
                errorMessages.Add("A non empty text to spell check needs to be provided.");
            if (!TryParseLanguage(Language, out _))
                errorMessages.Add("A valid language ('es', 'en') needs to be provided.");
            return !errorMessages.Any();
        }

        private bool TryParseLanguage(string language, out Language parsedLanguage)
        {
            parsedLanguage = SpellChecker.Language.English;
            try
            {
                parsedLanguage = language.GetEnum<Language>();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
