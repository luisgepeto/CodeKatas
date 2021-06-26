using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SpellChecker.UnitTests
{
    [TestClass]
    public class SpellCheckerOptionsTests
    {
        [TestMethod]
        public void SpellCheckerOptions_SourceDictionaryFilePath_DefaultLanguage_IsEnglish()
        {
            // Arrange
            var options = new TestOptions();
            // Act
            var fileName = options.SourceDictionaryFilePath;
            // Assert
            Assert.AreEqual("./SourceDictionaries/wordlist.en.txt", fileName);
        }

        [TestMethod]
        public void SpellCheckerOptions_SourceDictionaryFilePath_OtherLanguage_IsValid()
        {
            // Arrange
            var options = new TestOptions(Language.Spanish);
            // Act
            var fileName = options.SourceDictionaryFilePath;
            // Assert
            Assert.AreEqual("./SourceDictionaries/wordlist.es.txt", fileName);
        }
    }

    public class TestOptions : SpellCheckerOptions
    {
        public TestOptions() { }
        public TestOptions(Language language) : base(language) { }
        public string SourceDictionaryFilePath => GetSourceDictionaryFilePath();
    }
}
