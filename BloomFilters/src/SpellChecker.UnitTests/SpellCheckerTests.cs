using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SpellChecker.UnitTests
{
    [TestClass]
    public class SpellCheckerTests
    {
        [TestMethod]
        public void SpellChecker_SourceDictionaryFilePath_DefaultLanguage_IsEnglish()
        {
            // Arrange
            var options = new TestSpellCheckerOptions();
            var checker = new TestSpellChecker(options);
            // Act
            var fileName = checker.SourceDictionaryFilePath;
            // Assert
            Assert.AreEqual("./SourceDictionaries/wordlist.en.txt", fileName);
        }

        [TestMethod]
        public void SpellChecker_SourceDictionaryFilePath_OtherLanguage_IsValid()
        {
            // Arrange
            var options = new TestSpellCheckerOptions(Language.Spanish);
            var checker = new TestSpellChecker(options);
            // Act
            var fileName = checker.SourceDictionaryFilePath;
            // Assert
            Assert.AreEqual("./SourceDictionaries/wordlist.es.txt", fileName);
        }

        [TestMethod]
        public async Task SpellChecker_LoadSourceDictionary_English_ContainsWords()
        {
            // Arrange
            var options = new TestSpellCheckerOptions(Language.English);
            var checker = new TestSpellChecker(options);
            // Act
            await checker.LoadSourceDictionaryTask;
            // Assert
            Assert.AreEqual(3, checker.WordList.Count);
            Assert.IsTrue(checker.WordList.Contains("testword1"));
            Assert.IsTrue(checker.WordList.Contains("testword2"));
            Assert.IsFalse(checker.WordList.Contains("testword3"));
        }

        [TestMethod]
        public async Task SpellChecker_LoadSourceDictionary_Spanish_ContainsWords()
        {
            // Arrange
            var options = new TestSpellCheckerOptions(Language.Spanish);
            var checker = new TestSpellChecker(options);
            // Act
            await checker.LoadSourceDictionaryTask;
            // Assert
            Assert.AreEqual(3, checker.WordList.Count);
            Assert.IsTrue(checker.WordList.Contains("prueba1"));
            Assert.IsTrue(checker.WordList.Contains("prueba2"));
            Assert.IsFalse(checker.WordList.Contains("prueba3"));
        }
    }
    public class TestSpellCheckerOptions : SpellCheckerOptions
    {
        public TestSpellCheckerOptions(Language language) : base(language) { }
        public TestSpellCheckerOptions() { }
    }
    public class TestSpellChecker : BaseSpellChecker
    {
        public TestSpellChecker(SpellCheckerOptions options) : base(options) { WordList = new List<string>(); }
        public string SourceDictionaryFilePath => GetSourceDictionaryFilePath();
        public Task LoadSourceDictionaryTask => LoadSourceDictionary();
        public List<string> WordList { get; }
        protected override void LoadWord(string word)
        {
            WordList.Add(word);
        }
    }
}
