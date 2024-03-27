﻿using be_artwork_sharing_platform.Core.DbContext;
using be_artwork_sharing_platform.Core.Dtos.User;
using be_artwork_sharing_platform.Core.Entities;
using be_artwork_sharing_platform.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using be_artwork_sharing_platform.Core.Dtos.General;

namespace be_artwork_sharing_platform.Core.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UserService(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<GeneralServiceResponseDto> UpdateInformation(UpdateInformation updateUser, string userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id.Equals(userId));
            if (user is null)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSucceed = false,
                    StatusCode = 404,
                    Message = "User not found"
                };
            }
            if (!string.IsNullOrEmpty(updateUser.NickName) && updateUser.NickName != user.NickName)
            {
                var isExistNickName = _context.Users.FirstOrDefault(u => u.NickName == updateUser.NickName);
                if (isExistNickName is not null)
                {
                    return new GeneralServiceResponseDto()
                    {
                        IsSucceed = false,
                        StatusCode = 400,
                        Message = "NickName Already Exist"
                    };
                }
                user.NickName = updateUser.NickName;
            }

            if (!string.IsNullOrEmpty(updateUser.Email) && updateUser.Email != user.Email)
            {
                var isExistEmail = await _userManager.FindByEmailAsync(updateUser.Email);
                if (isExistEmail is not null)
                {
                    return new GeneralServiceResponseDto()
                    {
                        IsSucceed = false,
                        StatusCode = 400,
                        Message = "Email Already Exist"
                    };
                }
                user.Email = updateUser.Email;
            }

            if (!string.IsNullOrEmpty(updateUser.Address))
                user.Address = updateUser.Address;

            if (!string.IsNullOrEmpty(updateUser.PhoneNo))
                user.PhoneNumber = updateUser.PhoneNo;

            _context.Update(user);
            _context.SaveChanges();

            return new GeneralServiceResponseDto()
            {
                IsSucceed = true,
                StatusCode = 200,
                Message = "User information updated successfully"
            };
        }

        public async Task UpdateUser(UpdateStatusUser updateStatusUser, string userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id.Equals(userId));

            if (user is not null)
            {
                user.IsActive = updateStatusUser.IsActive;  
            }
            _context.Update(user);
            _context.SaveChanges();
        }

        public void ChangePassword(ChangePassword changePassword, string userID)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id.Equals(userID));
            if (user is not null)
            {
                user.PasswordHash = CheckPassword.HashPassword(changePassword.NewPassword);
            }
            _context.Update(user);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<UserInfoResult>> GetUserListAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            List<UserInfoResult> userInfoResults = new List<UserInfoResult>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userInfo = GeneralUserInfoObject(user, roles);
                userInfoResults.Add(userInfo);
            }

            return userInfoResults;
        }
        public async Task<UserInfoResult?> GetUserDetailsByUserNameAsyncs(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null) return null;

            var roles = await _userManager.GetRolesAsync(user);
            var userInfo = GeneralUserInfoObject(user, roles);
            return userInfo;
        }

        public async Task<IEnumerable<string>> GetUsernameListAsync()
        {
            var userNames = await _userManager.Users
                .Select(q => q.UserName)
                .ToListAsync();
            return userNames;
        }

        //GeneralUserInfoObject
        private UserInfoResult GeneralUserInfoObject(ApplicationUser user, IList<string> roles)
        {
            // Instead of this, You can use Automapper packages. But i don't want it in this project
            return new UserInfoResult()
            {
                Id = user.Id,
                NickName = user.NickName,
                UserName = user.UserName,
                Email = user.Email,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                CreatedAt = user.CreatedAt,
                Roles = roles
            };
        }
    }
}

