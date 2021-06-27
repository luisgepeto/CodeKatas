using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SpellChecker.UnitTests
{
    [TestClass]
    public class BloomFilterSpellCheckerTests
    {
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
    }
}
