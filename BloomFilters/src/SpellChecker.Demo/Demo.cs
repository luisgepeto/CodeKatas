using System;
using System.Collections.Generic;

using CommandLine;

using SpellChecker.Interfaces;

namespace SpellChecker.Demo
{
    public class Demo
    {
        public static void Main(string[] args)
        {

            Parser.Default.ParseArguments<DemoOptions>(args)
                .WithParsed(o =>
                {
                    if (!o.IsValid(out List<string> errorMessages))
                    {
                        foreach (var error in errorMessages)
                            Console.WriteLine(error);
                        return;
                    }
                    else
                    {
                        CheckSpelling(o);
                    }
                });
        }

        public static void CheckSpelling(DemoOptions demoOptions)
        {
            var options = new BloomFilterSpellCheckerOptions(demoOptions.ParsedLanguage);
            ISpellChecker spellChecker = new BloomFilterSpellChecker(options);
        }
    }
}
