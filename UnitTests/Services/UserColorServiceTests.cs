using BApp.DataAccess.Data;
using BApp.Domain.DTOs;
using BApp.Domain.Enums;
using BApp.Domain.Models;
using BApp.Services;
using Microsoft.EntityFrameworkCore;


namespace BApp.Tests.Services
{
    [TestClass]
    public class UserColorServiceTests
    {
        private ApplicationDbContext _dbContext;
        private UserColorService _userColorService;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _userColorService = new UserColorService(_dbContext);
        }

        [TestMethod]
        public async Task GetAllUserColors_ShouldReturnAllUserColors()
        {
            // Arrange
            _dbContext.UserColors.Add(new UserColor { UserId = 1, ColorHexValue = "#FFFFFF" });
            _dbContext.UserColors.Add(new UserColor { UserId = 2, ColorHexValue = "#000000" });
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _userColorService.GetAllUserColors();

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public async Task GetUserColorsByUserId_ShouldReturnUserColorsForGivenUserId()
        {
            // Arrange
            var userId = 1;
            _dbContext.UserColors.Add(new UserColor { UserId = userId, ColorHexValue = "#FFFFFF", ColorDifficultyId = 1 });
            _dbContext.ColorDifficulties.Add(new ColorDifficulty { Id = 1, ColorHexValue = "#FFFFFF", Status = DifficultyStatus.Easy });
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _userColorService.GetUserColorsByUserId(userId);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(userId, result[0].UserId);
        }

        [TestMethod]
        public async Task GetUserColorByHexValueAndUserId_ShouldReturnUserColorForGivenHexAndUserId()
        {
            // Arrange
            var userId = 1;
            var hexValue = "#FFFFFF";
            _dbContext.UserColors.Add(new UserColor { UserId = userId, ColorHexValue = hexValue });
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _userColorService.GetUserColorByHexValueAndUserId(hexValue, userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(hexValue, result.ColorHexValue);
        }

        [TestMethod]
        public async Task AddUserColor_ShouldAddNewUserColor()
        {
            // Arrange
            var userColorDto = new UserColorDTO
            {
                UserId = 1,
                ColorHexValue = "#FFFFFF"
            };

            // Act
            await _userColorService.AddUserColor(userColorDto);

            // Assert
            var result = await _dbContext.UserColors.FirstOrDefaultAsync(uc => uc.ColorHexValue == userColorDto.ColorHexValue && uc.UserId == userColorDto.UserId);
            Assert.IsNotNull(result);
        }


        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}
