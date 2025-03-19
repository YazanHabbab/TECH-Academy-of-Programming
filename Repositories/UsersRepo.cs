using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TECH_Academy_of_Programming.Data;
using TECH_Academy_of_Programming.Interfaces;
using TECH_Academy_of_Programming.Models;

namespace TECH_Academy_of_Programming.Repositories
{
    public class UsersRepo : IUsersRepo
    {
        private AppDbContext _context;
        private UserManager<User> _userManager;
        private IAuthService _authService;

        public UsersRepo(AppDbContext context, UserManager<User> userManager, IAuthService authService)
        {
            _context = context;
            _userManager = userManager;
            _authService = authService;
        }

        public async Task<RegisterResult> register(UserRegisterModel userRegisterModel)
        {
            var fullName = userRegisterModel.firstName.Trim() + "_" + userRegisterModel.lastName.Trim();
            if (await _userManager.FindByNameAsync(fullName) is not null)
            {
                return new RegisterResult { Message = "Username already exists" };
            }
            if (await _userManager.FindByEmailAsync(userRegisterModel.Email) is not null)
            {
                return new RegisterResult { Message = "Email already exists" };
            }

            User user = new User
            {
                firstName = userRegisterModel.firstName,
                lastName = userRegisterModel.lastName,
                UserName = fullName,
                Email = userRegisterModel.Email,
                created_at = DateTime.Now,
                updated_at = DateTime.Now,
            };

            var createResult = await _userManager.CreateAsync(user, userRegisterModel.Password);

            if (!createResult.Succeeded)
            {
                return new RegisterResult { Message = "Something went wrong, Cannot create user" };
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "user");

            if (!roleResult.Succeeded)
            {
                return new RegisterResult { Message = "Something went wrong, Cannot add user to role" };
            }

            return new RegisterResult { Message = "Registered Successfully!", Succeeded = true };
        }

        public async Task<AuthModel> login(UserLoginModel userLoginModel)
        {
            var user = new User();

            var fullName = userLoginModel.UserNameOrEmail.TrimStart().TrimEnd().Replace(' ', '_');

            if (await _userManager.FindByNameAsync(fullName) is not null)
                user = await _userManager.FindByNameAsync(fullName);

            else if (await _userManager.FindByEmailAsync(userLoginModel.UserNameOrEmail) is not null)
                user = await _userManager.FindByEmailAsync(userLoginModel.UserNameOrEmail);

            if (!await _userManager.CheckPasswordAsync(user, userLoginModel.Password))
                return new AuthModel { Message = "UserName or Email or Password is incorrect." };

            var JwtSecurityToken = await _authService.CreateJwtToken(user);

            var auth = new AuthModel
            {
                IsAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtSecurityToken),
                ExpiresOn = JwtSecurityToken.ValidTo,
                Message = "Authenticated"
            };

            return auth;
        }

        public async Task<User> getProfile(string ID)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == ID);
        }

        public async Task<UpdateResult> updateProfile(string ID, UpdatedUserModel updatedUserModel)
        {
            var updateResult = new UpdateResult();

            var user = _context.Users.FirstOrDefault(u => u.Id == ID);
            if (user is null)
            {
                return new UpdateResult { Message = "User does not exist." };
            }

            if (updatedUserModel.firstName != null && updatedUserModel.lastName == null)
            {
                var UserNameCheck = await _userManager.FindByNameAsync(updatedUserModel.firstName.Trim() + "_" + user.lastName);
                if (UserNameCheck != null)
                {
                    return new UpdateResult { Message = "this User Name does exist." };
                }
                else
                {
                    user.firstName = updatedUserModel.firstName;
                    user.UserName = updatedUserModel.firstName.Trim() + "_" + user.lastName;
                    user.updated_at = DateTime.Now;
                    updateResult.IsUpdated = true;
                }
            }

            else if (updatedUserModel.firstName == null && updatedUserModel.lastName != null)
            {
                var UserNameCheck = await _userManager.FindByNameAsync(user.firstName + "_" + updatedUserModel.lastName.Trim());
                if (UserNameCheck != null)
                {
                    return new UpdateResult { Message = "this User Name does exist." };
                }
                else
                {
                    user.lastName = updatedUserModel.lastName;
                    user.UserName = user.firstName + "_" + updatedUserModel.lastName.Trim();
                    user.updated_at = DateTime.Now;
                    updateResult.IsUpdated = true;
                }
            }

            else if (updatedUserModel.firstName != null && updatedUserModel.lastName != null)
            {
                var UserNameCheck = await _userManager.FindByNameAsync(updatedUserModel.firstName.Trim() + "_" + updatedUserModel.lastName.Trim());
                if (UserNameCheck != null)
                {
                    return new UpdateResult { Message = "this User Name does exist." };
                }
                else
                {
                    user.firstName = updatedUserModel.firstName;
                    user.lastName = updatedUserModel.lastName;
                    user.UserName = updatedUserModel.firstName.Trim() + "_" + updatedUserModel.lastName.Trim();
                    user.updated_at = DateTime.Now;
                    updateResult.IsUpdated = true;
                }
            }

            if (updatedUserModel.Email is not null)
            {
                var EmailCheck = await _userManager.FindByEmailAsync(updatedUserModel.Email);
                if (EmailCheck != null)
                {
                    return new UpdateResult { Message = "this Email does exist." };
                }
                else
                {
                    user.Email = updatedUserModel.Email;
                    user.updated_at = DateTime.Now;
                    updateResult.IsUpdated = true;
                }
            }

            if (!string.IsNullOrEmpty(updatedUserModel.oldPassword) && !string.IsNullOrEmpty(updatedUserModel.newPassword))
            {
                if (!await _userManager.CheckPasswordAsync(user, updatedUserModel.oldPassword))
                {
                    return new UpdateResult { Message = "Current password is not correct." };
                }
                else
                {
                    await _userManager.ChangePasswordAsync(user, updatedUserModel.oldPassword, updatedUserModel.newPassword);
                    user.updated_at = DateTime.Now;
                    updateResult.IsUpdated = true;
                }
            }

            await _userManager.UpdateAsync(user);

            updateResult.Succeeded = true;
            return updateResult;
        }

        public async Task<DeleteResult> delete(string ID)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == ID);
            if (user == null)
            {
                return new DeleteResult { Message = "User does not exist." };
            }

            var result = await _userManager.DeleteAsync(user);
            return new DeleteResult { IsDeleted = result.Succeeded, Message = result.Errors.Count() != 0 ? "Could not delete successfully." : "" };
        }

        public async Task<IEnumerable<User>> getUsers(int page)
        {
            if (page <= 0) page = 1;

            return await _context.Users.OrderBy(u => u.UserName).Skip((page - 1) * 5).Take(5).ToListAsync();
        }
    }
}