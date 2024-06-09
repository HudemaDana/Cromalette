using BApp.DataAccess.Data;
using BApp.Domain.Enums;
using BApp.Domain.Models;
using BApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BApp.Services
{
    public class UserLevelService : IUserLevelService
    {
        private readonly ApplicationDbContext _dbContext;

        public UserLevelService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateUserLevelOnUserSignUp(int userId)
        {
            var userLevel = await _dbContext.UserLevels
               .FirstOrDefaultAsync(ul => ul.UserId == userId);

            if (userLevel == null)
            {
                // Create a new UserLevel for the user with Level 1
                userLevel = new UserLevel
                {
                    UserId = userId,
                    LevelId = 1,
                    CurrentXP = 0
                };

                _dbContext.UserLevels.Add(userLevel);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteUserLevelOnUserDelete(int userId)
        {
            var userLevel = await _dbContext.UserLevels
               .FirstOrDefaultAsync(ul => ul.UserId == userId);

            if (userLevel is not null)
            {
                _dbContext.UserLevels.Remove(userLevel);
                await _dbContext.SaveChangesAsync();
            }
        }

        public UserLevel GetUserLevel(int userId)
        {
            var userLevel = _dbContext.UserLevels.FirstOrDefault(ul => ul.UserId == userId);
            if (userLevel is not null)
            {
                var currentLevel = _dbContext.Levels.FirstOrDefault(l => l.Id == userLevel.LevelId);
                userLevel.Level = currentLevel;
            }

            return userLevel;
        }

        public async Task UpdateUserLevelOnColorSave(UserColor userColor)
        {
            var userLevel = await _dbContext.UserLevels
                .Include(ul => ul.Level)
                .FirstOrDefaultAsync(ul => ul.UserId == userColor.UserId);

            if (userLevel == null)
            {
                userLevel = new UserLevel
                {
                    UserId = userColor.UserId,
                    LevelId = 1,
                    CurrentXP = 0
                };
                _dbContext.UserLevels.Add(userLevel);
            }

            var colorDifficulty = await _dbContext.ColorDifficulties.FirstOrDefaultAsync(cd => cd.ColorHexValue == userColor.ColorHexValue);

            userLevel.CurrentXP += (int?)colorDifficulty.Status ?? (int)DifficultyStatus.Easy;

            var nextLevel = await _dbContext.Levels.FirstOrDefaultAsync(l => l.Id == userLevel.LevelId + 1);

            while (nextLevel != null && userLevel.CurrentXP >= nextLevel.LevelTotalXP)
            {
                userLevel.LevelId = nextLevel.Id;
                nextLevel = await _dbContext.Levels.FirstOrDefaultAsync(l => l.Id == userLevel.LevelId + 1);
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
