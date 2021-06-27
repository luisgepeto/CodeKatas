using System;
using System.Collections.Generic;
using System.Linq;
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
            var bloomFilterOptions = new BloomFilterSpellCheckerOptions(demoOptions.ParsedLanguage);
            ISpellChecker filter = await BloomFilterSpellChecker.InitializeAsync(bloomFilterOptions);
            var result = filter.Check(demoOptions.Text);
            if (!result.ErrorsByStartIndex.Any())
                Console.WriteLine("Your text was error free! Congratulations!");
            else
            {
                foreach (var error in result.ErrorsByStartIndex)
                {
                    Console.WriteLine("The following word was not found in the dictionary: " + result.OriginalText.Substring(error.Key, error.Value.Length));
                }
            }
        }
    }
}
