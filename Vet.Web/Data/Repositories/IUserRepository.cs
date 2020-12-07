using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Web.Data.Entities;

namespace Vet.Web.Data.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Adds a new UserEntity to the database
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<IdentityResult> AddUserAsync(User user, string password);

        /// <summary>
        /// Adds the given user to the given role
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        Task AddUserToRoleAsync(User user, string roleName);

        /// <summary>
        /// Checks if the given role exists, if it doesn't it then creates it
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        Task CheckRoleAsync(string roleName);

        /// <summary>
        /// Confirms the email of a given user with a ConfirmEmailToken
        /// </summary>
        /// <param name="user"></param>
        /// <param name="token">ConfirmEmailToken</param>
        /// <returns></returns>
        Task<IdentityResult> ConfirmEmailAsync(User user, string token);

        /// <summary>
        /// Changes the Password of a given user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);

        /// <summary>
        /// Get's a list with all the roles in the database
        /// </summary>
        /// <returns></returns>
        Task<IQueryable<IdentityRole>> GetRolesAsync();

        /// <summary>
        /// generates an EmailConfirmationToken
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string> GenerateEmailConfirmationTokenAsync(User user);

        /// <summary>
        /// generates a PasswordResetToken
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string> GeneratePasswordResetTokenAsync(User user);

        /// <summary>
        /// Searches for a User in the database with the given email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<User> GetUserByEmailAsync(string email);

        /// <summary>
        /// Searches for a User in the database with the given user Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<User> GetUserByIdAsync(string userId);

        /// <summary>
        /// Generates a list of a select number of users based on the role of the requesting user
        /// </summary>
        /// <param name="role">role of the requesting user</param>
        /// <returns></returns>
        Task<IList<User>> GetUsersInRoleAsync(string role);

        /// <summary>
        /// Searches the database for the role of a given user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string> GetUserRoleAsync(User user);

        /// <summary>
        /// Checks if a given user belongs to a given role
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        Task<bool> IsUserInRoleAsync(User user, string roleName);

        /// <summary>
        /// Checks if the login information in the given model matches the one on the database.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<SignInResult> LoginAsync(Models.Account.LoginViewModel model);

        /// <summary>
        /// Drops the this.User session
        /// </summary>
        /// <returns></returns>
        Task LogoutAsync();

        /// <summary>
        /// Resets the password of a given user if the reset token is valid
        /// </summary>
        /// <param name="user"></param>
        /// <param name="token"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);

        /// <summary>
        /// updates the information of a user in the database by comparing the id of the database user and the id of the given user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IdentityResult> UpdateUserAsync(User user);

        /// <summary>
        /// Deletes a given user from the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IdentityResult> DeleteUserAsync(User user);
    }
}
