﻿using BApp.Domain.DTOs;
using BApp.Domain.Models;

namespace BApp.Services.Interfaces
{
    public interface IUserColorService
    {
        Task<List<UserColor>> GetAllUserColors();
        Task<List<UserColor>> GetUserColorsByUserId(int userId);
        Task<UserColor> GetUserColorByHexValueAndUserId(string hexValue, int userId);
        Task AddUserColor(UserColorDTO userColorDto);

        Task<UserColor> GetLastFoundUserColor(int userId);
        Task<UserColor> GetMostDifficultUserColor(int userId);
        Task<int> GetUserStrike(int userId);
    }
}
