using BApp.DataAccess.Data;
using BApp.Domain.Enums;
using BApp.Domain.Models;
using BApp.Services;
using Microsoft.EntityFrameworkCore;

namespace BApp.Tests.Services
{
    [TestClass]
    public class UserLevelServiceTests
    {
        private ApplicationDbContext _dbContext;
        private UserLevelService _userLevelService;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _userLevelService = new UserLevelService(_dbContext);
        }

        [TestMethod]
        public async Task CreateUserLevelOnUserSignUp_ShouldCreateUserLevel()
        {
            // Arrange
            var userId = 1;

            // Act
            await _userLevelService.CreateUserLevelOnUserSignUp(userId);

            // Assert
            var userLevel = await _dbContext.UserLevels.FirstOrDefaultAsync(ul => ul.UserId == userId);
            Assert.IsNotNull(userLevel);
            Assert.AreEqual(1, userLevel.LevelId);
            Assert.AreEqual(0, userLevel.CurrentXP);
        }

        [TestMethod]
        public async Task DeleteUserLevelOnUserDelete_ShouldDeleteUserLevel()
        {
            // Arrange
            var userId = 1;
            var userLevel = new UserLevel { UserId = userId, LevelId = 1, CurrentXP = 0 };
            _dbContext.UserLevels.Add(userLevel);
            await _dbContext.SaveChangesAsync();

            // Act
            await _userLevelService.DeleteUserLevelOnUserDelete(userId);

            // Assert
            var deletedUserLevel = await _dbContext.UserLevels.FirstOrDefaultAsync(ul => ul.UserId == userId);
            Assert.IsNull(deletedUserLevel);
        }

        [TestMethod]
        public void GetUserLevel_ShouldReturnUserLevel()
        {
            // Arrange
            var userId = 1;
            var level = new Level { Id = 1, LevelTotalXP = 10 };
            var userLevel = new UserLevel { UserId = userId, LevelId = 1, CurrentXP = 0 };
            _dbContext.Levels.Add(level);
            _dbContext.UserLevels.Add(userLevel);
            _dbContext.SaveChanges();

            // Act
            var result = _userLevelService.GetUserLevel(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.UserId);
            Assert.AreEqual(1, result.LevelId);
            Assert.AreEqual(0, result.CurrentXP);
            Assert.IsNotNull(result.Level);
        }


        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}
