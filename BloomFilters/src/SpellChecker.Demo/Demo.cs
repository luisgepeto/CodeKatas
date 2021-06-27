using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using CommandLine;

using SpellChecker.Interfaces;

namespace SpellChecker.Demo
{
    public class Demo
    {
        public static async Task Main(string[] args)
        {
            await Parser.Default.ParseArguments<DemoOptions>(args)
                .WithParsedAsync(o =>
                {
                    if (!o.IsValid(out List<string> errorMessages))
                    {
                        foreach (var error in errorMessages)
                            Console.WriteLine(error);
                        return Task.CompletedTask;
                    }
                    else
                        return CheckSpelling(o);
                });
        }

        public static async Task CheckSpelling(DemoOptions demoOptions)
        {
            var options = new BloomFilterSpellCheckerOptions(demoOptions.ParsedLanguage);
            ISpellChecker filter = await BloomFilterSpellChecker.InitializeAsync(options);
        }
    }
}
