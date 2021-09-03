using CSharpFunctionalExtensions;
using SecretsSecurityAssignment.Core.Data.Service;
using SecretsSecurityAssignment.Core.UserEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSecurityAssignment.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IJWTService jWTService;

        public UserService(IUnitOfWork unitOfWork, IJWTService jWTService)
        {
            this.unitOfWork = unitOfWork;
            this.jWTService = jWTService;
        }

        public Result Block(int userId)
        {
            var user = unitOfWork.UserRepository.GetById(userId);

            if (user == null)
                return Result.Failure($"Couldn't find user with id: {userId}");

            user.Blocked = true;
            unitOfWork.UserRepository.Update(user);

            if (unitOfWork.SaveChanges(1) == false)
            {
                return Result.Failure($"Couldn't save data for user: {user.UserName} of type: {user.UserType}");
            }

            return Result.Success();
        }

        public Result Block(int[] userIds)
        {
            var users = unitOfWork.UserRepository.GetById(userIds);

            if (users == null)
                return Result.Failure($"Couldn't find users with ids: {string.Join(", ", userIds)}");

            foreach (var user in users)
            {
                user.Blocked = true;
            }
            unitOfWork.UserRepository.Update(users);

            if (unitOfWork.SaveChanges(users.Count()) == false)
            {
                List<string> errorUsers = new();
                foreach (var user in users)
                {
                    errorUsers.Add($"{user.UserName} of type: {user.UserType}\n");
                }
                return Result.Failure("Couldn't save data for users:\n" + errorUsers);
            }

            return Result.Success();
        }

        public Result<string> Login(string username, string password)
        {
            var user = unitOfWork.UserRepository.Get(filter: u => u.UserName == username);

            if (user == null)
                return Result.Failure<string>($"Couldn't find a user with username: {username}");
            else if (user.Blocked == true)
                return Result.Failure<string>($"User is not allowed to login");
            else if (user.HashedPassword != jWTService.ComputeHash(password, user.Salt))
                return Result.Failure<string>($"{password} is the incorrect password");

            var jwt = jWTService.GenerateJWT(new List<Claim>
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.UserType.ToString())
            });

            return Result.Success(jwt);
        }

        public Result Register(string username, string password, UserType userType)
        {
            var salt = jWTService.GenerateSalt();

            unitOfWork.UserRepository.Create(new User
            {
                Blocked = false,
                UserType = userType,
                Salt = salt,
                HashedPassword = jWTService.ComputeHash(password, salt),
                SecurityKey = new Guid().ToString(),
                UserName = username
            });

            if (unitOfWork.SaveChanges(1) == false)
            {
                return Result.Failure($"Couldn't save the user: {username}");
            }

            return Result.Success();

        }

        public Result Unblock(int userId)
        {
            var user = unitOfWork.UserRepository.GetById(userId);

            if (user == null)
                return Result.Failure($"Couldn't find user with id: {userId}");

            user.Blocked = false;
            unitOfWork.UserRepository.Update(user);

            if (unitOfWork.SaveChanges(1) == false)
            {
                return Result.Failure($"Couldn't save data for user: {user.UserName} of type: {user.UserType}");
            }

            return Result.Success();
        }

        public Result Unblock(int[] userIds)
        {
            var users = unitOfWork.UserRepository.GetById(userIds);

            if (users == null)
                return Result.Failure($"Couldn't find users with ids: {string.Join(", ", userIds)}");

            foreach (var user in users)
            {
                user.Blocked = false;
            }
            unitOfWork.UserRepository.Update(users);

            if (unitOfWork.SaveChanges(users.Count()) == false)
            {
                List<string> errorUsers = new();
                foreach (var user in users)
                {
                    errorUsers.Add($"{user.UserName} of type: {user.UserType}\n");
                }
                return Result.Failure("Couldn't save data for users:\n" + errorUsers);
            }

            return Result.Success();
        }
    }
}
