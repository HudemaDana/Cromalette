using BApp.DataAccess.Data;
using BApp.Domain.DTOs;
using BApp.Domain.Enums;
using BApp.Domain.Models;
using BApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BApp.Services
{
    public class UserColorService : IUserColorService
    {
        private readonly ApplicationDbContext _dbContext;

        public UserColorService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<UserColor>> GetAllUserColors()
        {
            return await _dbContext.UserColors.ToListAsync();
        }

        public async Task<List<UserColor>> GetUserColorsByUserId(int userId)
        {
            var userColorList = await _dbContext.UserColors.Where(uc => uc.UserId == userId).ToListAsync();
            foreach (var userColor in userColorList)
            {
                var colorDifficulty = _dbContext.ColorDifficulties.FirstOrDefault(cd => cd.Id == userColor.ColorDifficultyId);
                colorDifficulty.UserColors = null;
                userColor.ColorDifficulty = colorDifficulty;
            }
            return userColorList;
        }

        public async Task<UserColor> GetUserColorByHexValueAndUserId(string hexValue, int userId)
        {
            return await _dbContext.UserColors.FirstOrDefaultAsync(uc => uc.ColorHexValue == hexValue && uc.UserId == userId);
        }

        public async Task AddUserColor(UserColorDTO userColorDto)
        {
            var colorWithHex = await _dbContext.UserColors.FirstOrDefaultAsync(uc => uc.ColorHexValue == userColorDto.ColorHexValue);
            var colorDifficulty = await GetColorDifficulty(userColorDto);

            var userColor = new UserColor()
            {
                UserId = userColorDto.UserId,
                ColorHexValue = userColorDto.ColorHexValue,
                ColorDifficultyId = colorDifficulty.Id,
                SavingDate = DateTime.UtcNow,
                ColorDifficulty = colorDifficulty
            };

            await UpdateColorDifficulty(colorDifficulty);

            await _dbContext.UserColors.AddAsync(userColor);

            await UpdateUserLevel(userColorDto.UserId, (int)colorDifficulty.Status);

            await _dbContext.SaveChangesAsync();
        }

        private async Task UpdateUserLevel(int userId, int colorXP)
        {
            var userLevel = await _dbContext.UserLevels.FirstOrDefaultAsync(ul => ul.UserId == userId);
            if (userLevel is not null)
            {
                var currentLevel = await _dbContext.Levels.FirstOrDefaultAsync(cl => cl.Id == userLevel.LevelId);
                if (currentLevel is not null)
                {
                    userLevel.CurrentXP += colorXP;
                    while (userLevel.CurrentXP > currentLevel.LevelTotalXP && currentLevel != null)
                    {
                        userLevel.LevelId += 1;
                        currentLevel = await _dbContext.Levels.FirstOrDefaultAsync(cl => cl.Id == userLevel.LevelId);
                    }
                }
            }
        }

        private async Task UpdateColorDifficulty(ColorDifficulty colorDifficulty)
        {
            colorDifficulty.FindingCount += 1;
            colorDifficulty.Status = await ClassifyRuleAsync(colorDifficulty.FindingCount);
        }

        private async Task<DifficultyStatus> ClassifyRuleAsync(int nr)
        {
            double err = 0.01;
            double lowerBoundaryRareDifficulty = 0.1;
            double lowerBoundaryHardDifficulty = 0.2;

            var avg = await _dbContext.ColorDifficulties.AverageAsync(cd => cd.FindingCount);
            return nr switch
            {
                _ when nr <= Math.Floor(lowerBoundaryRareDifficulty * avg) || nr > Math.Ceiling(avg + err) => DifficultyStatus.Easy,
                _ when nr >= Math.Floor(avg - err) && nr <= Math.Ceiling(avg + err) => DifficultyStatus.Medium,
                _ when nr > Math.Floor(lowerBoundaryRareDifficulty * avg) && nr < Math.Ceiling(lowerBoundaryHardDifficulty * avg) => DifficultyStatus.Rare,
                _ when nr >= Math.Ceiling(lowerBoundaryHardDifficulty * avg) && nr < Math.Floor(avg - err) => DifficultyStatus.Hard,
                _ => DifficultyStatus.Unknown,
            };
        }

        private async Task<ColorDifficulty> GetColorDifficulty(UserColorDTO userColorDto)
        {
            var colorDifficulty = _dbContext.ColorDifficulties.FirstOrDefault(cd => cd.ColorHexValue == userColorDto.ColorHexValue);

            if (colorDifficulty is null)
            {
                var newColorDifficulty = new ColorDifficulty()
                {
                    ColorHexValue = userColorDto.ColorHexValue,
                    Status = DifficultyStatus.Easy,
                    FindingCount = 0
                };

                await _dbContext.ColorDifficulties.AddAsync(newColorDifficulty);
                await _dbContext.SaveChangesAsync();

                colorDifficulty = _dbContext.ColorDifficulties.First(cd => cd.ColorHexValue == userColorDto.ColorHexValue);
            }

            return colorDifficulty;
        }


    }
}
