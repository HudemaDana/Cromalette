using BApp.DataAccess.Data;
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
                    CurrentXP = 0 // Assuming the user starts with 0 XP
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

        public async Task UpdateUserLevelOnColorSave(UserColor userColor)
        {
            // Get the user's current UserLevel
            var userLevel = await _dbContext.UserLevels
                .Include(ul => ul.Level)
                .FirstOrDefaultAsync(ul => ul.UserId == userColor.UserId);

            if (userLevel == null)
            {
                // Create a new UserLevel for the user with Level 1
                userLevel = new UserLevel
                {
                    UserId = userColor.UserId,
                    LevelId = 1,
                    CurrentXP = 0 // Assuming the user starts with 0 XP
                };
                _dbContext.UserLevels.Add(userLevel);
            }

            // Update current XP based on color difficulty
            userLevel.CurrentXP += (int)userColor.Difficulty;

            // Get the next level
            var nextLevel = await _dbContext.Levels.FirstOrDefaultAsync(l => l.Id == userLevel.LevelId + 1);

            // While the user has enough XP to reach the next level, level up
            while (nextLevel != null && userLevel.CurrentXP >= nextLevel.LevelTotalXP)
            {
                // Update UserLevel with new level
                userLevel.LevelId = nextLevel.Id;

                // Get the next level again to check if the user can level up further
                nextLevel = await _dbContext.Levels.FirstOrDefaultAsync(l => l.Id == userLevel.LevelId + 1);
            }

            // Save changes to the database
            await _dbContext.SaveChangesAsync();
        }
    }
}
