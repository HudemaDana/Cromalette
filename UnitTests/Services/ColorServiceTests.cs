using BApp.Services;

namespace BApp.Tests.Services
{
    [TestClass]
    public class ColorServiceTests
    {
        private readonly ColorService _colorService;

        public ColorServiceTests()
        {
            _colorService = new ColorService();
        }

        [TestMethod]
        [DataRow("#FF5733", 5)]
        [DataRow("#000000", 3)]
        [DataRow("#FFFFFF", 10)]
        public async Task GenerateTints_ShouldReturnCorrectNumberOfTints(string hexColor, int count)
        {
            var result = await _colorService.GenerateTints(hexColor, count);

            Assert.AreEqual(count, result.Count);
        }

        [TestMethod]
        [DataRow("#FF5733", 5)]
        [DataRow("#000000", 3)]
        [DataRow("#FFFFFF", 10)]
        public async Task GenerateShades_ShouldReturnCorrectNumberOfShades(string hexColor, int count)
        {
            var result = await _colorService.GenerateShades(hexColor, count);

            Assert.AreEqual(count, result.Count);
        }

        [TestMethod]
        [DataRow("#FF5733", 5)]
        [DataRow("#000000", 3)]
        [DataRow("#FFFFFF", 10)]
        public async Task GenerateTones_ShouldReturnCorrectNumberOfTones(string hexColor, int count)
        {
            var result = await _colorService.GenerateTones(hexColor, count);

            Assert.AreEqual(count, result.Count);
        }

        [TestMethod]
        [DataRow("#FF5733", 1)]
        [DataRow("#FF5733", 2)]
        [DataRow("#FF5733", 3)]
        [DataRow("#FF5733", 4)]
        public async Task GeneratePalette_ShouldReturnCorrectPalette(string hexColor, int rule)
        {
            var result = await _colorService.GeneratePalette(hexColor, rule);

            switch (rule)
            {
                case 1:
                    Assert.AreEqual(2, result.Count);
                    break;
                case 2:
                case 3:
                    Assert.AreEqual(3, result.Count);
                    break;
                case 4:
                    Assert.AreEqual(4, result.Count);
                    break;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GeneratePalette_ShouldThrowArgumentExceptionForInvalidRule()
        {
            await _colorService.GeneratePalette("#FF5733", 5);
        }
    }
}
