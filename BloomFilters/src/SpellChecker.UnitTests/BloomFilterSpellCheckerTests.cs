using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SpellChecker.UnitTests
{
    [TestClass]
    public class BloomFilterSpellCheckerTests
    {
        [TestMethod]
        public async Task BloomFilterSpellChecker_FalsePositives_Statistics()
        {
            // Arrange
            var options = new BloomFilterSpellCheckerOptions(Language.English, bitArrayLength: 1000);
            var filter = await BloomFilterSpellChecker.InitializeAsync(options);
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var random = new Random();
            // Act
            foreach (var i in Enumerable.Range(0, 1000000))
            {
                var randomString = new string(Enumerable.Range(0, 5).Select(s => chars[random.Next(chars.Length)]).ToArray());
                var result = filter.Check(randomString);
                if (!result.ErrorsByStartIndex.Any())
                    Debug.WriteLine("Random string found: " + randomString);
            }
        }

        [TestMethod]
        public async Task BloomFilterSpellChecker_Check_NonExistingWords_ReturnsExpected()
        {
            // Arrange
            var options = new BloomFilterSpellCheckerOptions(Language.English, 16);
            var filter = await BloomFilterSpellChecker.InitializeAsync(options);
            var stringToCheck = "this Is not a    testworD1   \r\n inclUded ";
            // Act
            var result = filter.Check(stringToCheck);
            // Assert
            Assert.AreEqual(stringToCheck, result.OriginalText);
            Assert.AreEqual(5, result.ErrorsByStartIndex.Count());
            Assert.AreEqual("this", result.ErrorsByStartIndex[0].SanitizedWord);
            Assert.AreEqual("this", stringToCheck.Substring(0, result.ErrorsByStartIndex[0].Length).ToLowerInvariant());
            Assert.AreEqual("is", result.ErrorsByStartIndex[5].SanitizedWord);
            Assert.AreEqual("is", stringToCheck.Substring(5, result.ErrorsByStartIndex[5].Length).ToLowerInvariant());
            Assert.AreEqual("not", result.ErrorsByStartIndex[8].SanitizedWord);
            Assert.AreEqual("not", stringToCheck.Substring(8, result.ErrorsByStartIndex[8].Length).ToLowerInvariant());
            Assert.AreEqual("a", result.ErrorsByStartIndex[12].SanitizedWord);
            Assert.AreEqual("a", stringToCheck.Substring(12, result.ErrorsByStartIndex[12].Length).ToLowerInvariant());
            Assert.AreEqual("included", result.ErrorsByStartIndex[32].SanitizedWord);
            Assert.AreEqual("included", stringToCheck.Substring(32, result.ErrorsByStartIndex[32].Length).ToLowerInvariant());
        }

        [TestMethod]
        public async Task BloomFilterSpellChecker_CheckWord_Exists_ReturnsTrue()
        {
            // Arrange
            var options = new BloomFilterSpellCheckerOptions(Language.English, 16);
            var filter = (BloomFilterSpellChecker)await BloomFilterSpellChecker.InitializeAsync(options);
            // Act
            var result = filter.CheckWord("testword1");
            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task BloomFilterSpellChecker_CheckWord_DoesntExists_ReturnsFalse()
        {
            // Arrange
            var options = new BloomFilterSpellCheckerOptions(Language.English, 16);
            var filter = (BloomFilterSpellChecker)await BloomFilterSpellChecker.InitializeAsync(options);
            // Act
            var result = filter.CheckWord("testword999");
            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task BloomFilterSpellChecker_InitializeAsync_DictionaryIsLoaded()
        {
            // Arrange
            var options = new BloomFilterSpellCheckerOptions(Language.English, 16, bitArrayLength: 50);
            var resultArray = new bool[50];
            // Act
            var filter = (BloomFilterSpellChecker)await BloomFilterSpellChecker.InitializeAsync(options);
            filter.BitArray.CopyTo(resultArray, 0);
            // Assert            
            Assert.IsTrue(resultArray.Any(r => r));
        }


        [TestMethod]
        public async Task BloomFilterSpellChecker_InitializeAsync_IsNotNull()
        {
            // Arrange
            var options = new BloomFilterSpellCheckerOptions();
            // Act
            var filter = await BloomFilterSpellChecker.InitializeAsync(options);
            // Assert
            Assert.IsNotNull(filter);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(17)]
        public void BloomFilterSpellChecker_NumberOfHashingFunctions_OutOfRange_ThrowsException(int hashingFunctionsCount)
        {
            // Act
            try
            {
                var options = new BloomFilterSpellCheckerOptions(Language.English, hashingFunctionsCount);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsTrue(ex is ApplicationException);
                Assert.IsTrue(ex.Message.Contains("The hashing functions count must be between 1 and 16"));
            }
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(5)]
        [DataRow(16)]
        public void BloomFilterSpellChecker_NumberOfHashingFunctions_InRange_IsValid(int hashingFunctionsCount)
        {
            // Act
            var options = new BloomFilterSpellCheckerOptions(Language.English, hashingFunctionsCount);
            Assert.IsNotNull(options);
            Assert.AreEqual(hashingFunctionsCount, options.HashingFunctionsCount);
        }

        [TestMethod]
        [DataRow(5)]
        [DataRow(16)]
        public async Task BloomFilterSpellChecker_GetWordHash_LimitToMaxBitArray(int hashingFunctions)
        {
            // Arrange
            int maxBitArrayLength = 50;
            var options = new BloomFilterSpellCheckerOptions(Language.English, hashingFunctions, bitArrayLength: maxBitArrayLength);
            var filter = (BloomFilterSpellChecker)await BloomFilterSpellChecker.InitializeAsync(options);
            // Act
            var result = filter.GetWordHash("hello");
            // Assert
            Assert.IsTrue(result.All(r => r < maxBitArrayLength));
        }

        [TestMethod]
        public async Task BloomFilterSpellChecker_GetWordHash_UsesRandom()
        {
            // Arrange
            var options = new BloomFilterSpellCheckerOptions(Language.English, bitArrayLength: 10);
            var filter = (BloomFilterSpellChecker)await BloomFilterSpellChecker.InitializeAsync(options);
            // Act
            var result = filter.GetWordHash("hello");
            // Assert
            Assert.IsTrue(result.All(r => r < 10));
        }
    }
}
