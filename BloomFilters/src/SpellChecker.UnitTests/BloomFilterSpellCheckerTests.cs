using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SpellChecker.UnitTests
{
    [TestClass]
    public class BloomFilterSpellCheckerTests
    {

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
            var options = new BloomFilterSpellCheckerOptions(Language.English, 16);
            var expectedSetElements = new List<int>() { 3, 6, 8, 9, 17, 18, 23, 25, 34, 47, 59, 62, 70, 73, 80, 82, 86, 89, 93, 99, 100, 102, 105, 109, 118, 144, 146, 150, 155, 169, 170, 171, 182, 188, 193, 198, 209, 213, 222, 228, 234, 237, 243, 250, 251, 253 };
            // Act
            var filter = (BloomFilterSpellChecker)await BloomFilterSpellChecker.InitializeAsync(options);
            // Assert
            var setElements = filter.BitArray.Select((ba, i) => (BitValue: ba, Index: i)).Where(ba => ba.BitValue).Select(ba => ba.Index);
            Assert.IsTrue(Enumerable.SequenceEqual(expectedSetElements, setElements));
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
        public async Task BloomFilterSpellChecker_GetWordHash_MatchesHashingFunctionsCount(int hashingFunctions)
        {
            // Arrange
            var options = new BloomFilterSpellCheckerOptions(Language.English, hashingFunctions);
            var filter = (BloomFilterSpellChecker)await BloomFilterSpellChecker.InitializeAsync(options);
            var fullHash = new byte[] { 93, 65, 64, 42, 188, 75, 42, 118, 185, 113, 157, 145, 16, 23, 197, 146 };
            // Act
            var result = filter.GetWordHash("hello");
            // Assert
            Assert.IsTrue(Enumerable.SequenceEqual(fullHash.Take(hashingFunctions), result));
        }
    }
}
