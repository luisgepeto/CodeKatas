using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                    {
                        if (!o.RunDiagnostics)
                            return CheckSpelling(o);
                        else
                            return RunDiagnostics();
                    }
                });
        }

        public static async Task CheckSpelling(DemoOptions demoOptions)
        {
            var bloomFilterOptions = new BloomFilterSpellCheckerOptions(demoOptions.ParsedLanguage);
            ISpellChecker filter = await BloomFilterSpellChecker.InitializeAsync(bloomFilterOptions);
            var result = await filter.CheckAsync(demoOptions.Text);
            if (!result.ErrorsByStartIndex.Any())
                Console.WriteLine("Your text was error free! Congratulations!");
            else
            {
                var notFound = string.Join(" ", result.ErrorsByStartIndex.Select(e => result.OriginalText.Substring(e.Key, e.Value.Length)));
                Console.WriteLine($"The following words were not found in the dictionary: {notFound}");
            }
        }

        public static async Task RunDiagnostics()
        {
            //TODO All parameters could be sent on the DemoOptions object
            var numberOfWords = 100;
            var hashingFunctionsOptions = Enumerable.Range(5, 15);
            var bitArrayLengthOptions = new List<int>() { 50000000, 5000000, 500000 };
            var testText = GenerateTestText(numberOfWords);

            var summary = new List<(int HashingFunctions, int BitArrayLength, int ErrorCount, long InitializationMilliseconds)>();
            foreach (var hashingFunctionsCount in hashingFunctionsOptions)
            {
                foreach (var bitArrayLength in bitArrayLengthOptions)
                {
                    var summaryResult = await CheckAsync(testText, hashingFunctionsCount, bitArrayLength);
                    summary.Add(summaryResult);
                }
            }
            PrintAnalysis(numberOfWords, summary);
        }

        private static string GenerateTestText(int numberOfWords)
        {
            var testText = "";
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var random = new Random();
            foreach (var i in Enumerable.Range(0, numberOfWords))
            {
                var randomString = new string(Enumerable.Range(0, 5).Select(s => chars[random.Next(chars.Length)]).ToArray());
                // We can safely assume that this text does not contain any real words
                testText += randomString + " ";
            }
            return testText;
        }

        private static async Task<(int, int, int, long)> CheckAsync(string testText, int hashingFunctionsCount, int bitArrayLength)
        {
            var options = new BloomFilterSpellCheckerOptions(Language.English, hashingFunctionsCount, false, bitArrayLength);
            var watch = new Stopwatch();
            watch.Start();
            var filter = await BloomFilterSpellChecker.InitializeAsync(options);
            watch.Stop();
            var result = await filter.CheckAsync(testText);
            var errorCount = result.ErrorsByStartIndex.Keys.Count();
            return (hashingFunctionsCount, bitArrayLength, errorCount, watch.ElapsedMilliseconds);
        }

        private static void PrintAnalysis(int numberOfWords, List<(int HashingFunctions, int BitArrayLength, int ErrorCount, long InitializationMilliseconds)> summary)
        {
            Console.WriteLine("========== ANALYSIS SUMMARY ========== ");
            foreach (var summaryResult in summary.OrderBy(s => numberOfWords - s.ErrorCount).ThenBy(s => s.BitArrayLength).ThenBy(s => s.InitializationMilliseconds).ThenBy(s => s.HashingFunctions))
            {
                Console.WriteLine($"FalsePositiveRate: {(numberOfWords - summaryResult.ErrorCount) * 100.0 / numberOfWords}% BitArrayLength: {summaryResult.BitArrayLength} InitializationMilliseconds: {summaryResult.InitializationMilliseconds} HashingFunctions: {summaryResult.HashingFunctions}");
            }
        }
    }
}
