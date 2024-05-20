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
            return await _dbContext.UserColors.Where(uc => uc.UserId == userId).ToListAsync();
        }

        public async Task<UserColor> GetUserColorByHexValueAndUserId(string hexValue, int userId)
        {
            return await _dbContext.UserColors.FirstOrDefaultAsync(uc => uc.ColorHexValue == hexValue && uc.UserId == userId);
        }

        public async Task AddUserColor(UserColorDTO userColorDto)
        {
            var colorWithHex = await _dbContext.UserColors.FirstOrDefaultAsync(uc => uc.ColorHexValue == userColorDto.ColorHexValue);
            var colorDifficultyId = await GetColorDifficultyId(userColorDto);

            var userColor = new UserColor()
            {
                UserId = userColorDto.UserId,
                ColorHexValue = userColorDto.ColorHexValue,
                //ColorDifficultyId = colorDifficultyId,
                Difficulty = DifficultyStatus.Easy,
                SavingDate = DateTime.Now,
            };

            await _dbContext.UserColors.AddAsync(userColor);
            await _dbContext.SaveChangesAsync();
        }

        private async Task<int> GetColorDifficultyId(UserColorDTO userColorDto)
        {
            var colorDifficulty = _dbContext.ColorDifficulties.FirstOrDefault(cd => cd.ColorHexValue == userColorDto.ColorHexValue);

            if (colorDifficulty is null)
            {
                var newColorDifficulty = new ColorDifficulty()
                {
                    ColorHexValue = userColorDto.ColorHexValue,
                    Status = DifficultyStatus.Easy,
                    FindingCount = 1
                };

                await _dbContext.ColorDifficulties.AddAsync(newColorDifficulty);
                await _dbContext.SaveChangesAsync();

                colorDifficulty = _dbContext.ColorDifficulties.FirstOrDefault(cd => cd.ColorHexValue == userColorDto.ColorHexValue);
            }

            return colorDifficulty.Id;
        }
    }
}
