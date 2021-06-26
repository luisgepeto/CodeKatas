using System.Collections.Generic;
using System.Threading.Tasks;

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

        [TestMethod]
        public async Task SpellCheckerOptions_LoadSourceDictionary_English_ContainsWords()
        {
            // Arrange
            var options = new TestOptions(Language.English);
            // Act
            await options.LoadSourceDictionaryTask;
            // Assert
            Assert.AreEqual(3, options.WordList.Count);
            Assert.IsTrue(options.WordList.Contains("testword1"));
            Assert.IsTrue(options.WordList.Contains("testword2"));
            Assert.IsFalse(options.WordList.Contains("testword3"));
        }

        [TestMethod]
        public async Task SpellCheckerOptions_LoadSourceDictionary_Spanish_ContainsWords()
        {
            // Arrange
            var options = new TestOptions(Language.Spanish);
            // Act
            await options.LoadSourceDictionaryTask;
            // Assert
            Assert.AreEqual(3, options.WordList.Count);
            Assert.IsTrue(options.WordList.Contains("prueba1"));
            Assert.IsTrue(options.WordList.Contains("prueba2"));
            Assert.IsFalse(options.WordList.Contains("prueba3"));
        }
    }

    public class TestOptions : SpellCheckerOptions
    {
        public TestOptions() : this(Language.English) { }
        public TestOptions(Language language) : base(language) { WordList = new List<string>(); }
        public string SourceDictionaryFilePath => GetSourceDictionaryFilePath();
        public Task LoadSourceDictionaryTask => LoadSourceDictionary();
        public List<string> WordList { get; }
        protected override void LoadWord(string word)
        {
            WordList.Add(word);
        }
    }
}
