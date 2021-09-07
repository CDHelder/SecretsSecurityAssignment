using CSharpFunctionalExtensions;
using SecretsSecurityAssignment.Core.Data.Service;
using SecretsSecurityAssignment.Core.UserEntities;
using SecretsSecurityAssignment.Service.CustomAttributes;
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

        [AuthorizeUser(UserType.Admin)]
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

        [AuthorizeUser(UserType.Admin)]
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
            var validateResult = ValidateLogin(username, password);
            if (validateResult.IsFailure)
            {
                return Result.Failure<string>(validateResult.Error);
            }

            var jwt = jWTService.GenerateJWT(new List<Claim>
            {
                new Claim(ClaimTypes.SerialNumber, validateResult.Value.Id.ToString()),
                new Claim(ClaimTypes.Name, validateResult.Value.UserName),
                new Claim(ClaimTypes.Role, validateResult.Value.UserType.ToString())
            }, validateResult.Value.SecurityKey);

            return Result.Success(jwt);
        }

        private Result<User> ValidateLogin(string username, string password)
        {
            var user = unitOfWork.UserRepository.Get(filter: u => u.UserName == username);

            if (user == null)
                return Result.Failure<User>($"Couldn't find a user with username: {username}");
            else if (user.Blocked == true)
                return Result.Failure<User>($"User is banned");
            else if (user.HashedPassword != jWTService.ComputeHash(password, user.Salt))
                return Result.Failure<User>($"{password} is the incorrect password");

            return Result.Success(user);
        }

        public Result Register(string username, string password, UserType userType)
        {
            var validateResult = ValidateRegistration(username, password, userType);
            if (validateResult.IsFailure)
            {
                return Result.Failure(validateResult.Error);
            }

            var salt = jWTService.GenerateSalt();

            var user = new User
            {
                Blocked = false,
                UserType = userType,
                Salt = salt,
                HashedPassword = jWTService.ComputeHash(password, salt),
                SecurityKey = Guid.NewGuid().ToString(),
                UserName = username
            };

            unitOfWork.UserRepository.Create(user);

            if (unitOfWork.SaveChanges(1) == false)
            {
                return Result.Failure($"Couldn't save the user: {username}");
            }

            return Result.Success();

        }

        [AuthorizeUser(UserType.Admin)]
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

        [AuthorizeUser(UserType.Admin)]
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

        private Result ValidateRegistration(string username, string password, UserType userType)
        {
            var errorMessages = new List<string>();
            if (userType == UserType.Admin)
            {
                errorMessages.Add("401: Unauthorized");
                return Result.Failure(string.Join("\n", errorMessages));
            }
            else if (username == null)
            {
                errorMessages.Add("Please enter a username");
            }
            else if (password == null)
            {
                errorMessages.Add("Please enter a password");
                return Result.Failure(string.Join("\n", errorMessages));
            }
            else if (password.Length < 8)
            {
                errorMessages.Add("Password must be 8 characters long");
            }
            else if (!password.Any(char.IsDigit))
            {
                errorMessages.Add("Password must contain atleast one number");
                return Result.Failure(string.Join("\n", errorMessages));
            }
            else if (unitOfWork.UserRepository.Get(filter: u => u.UserName == username) != null)
                return Result.Failure($"User with username: {username} already excists");

            return Result.Success();
        }

        [AuthorizeUser(UserType.Admin)]
        public Result Block(string username)
        {
            var user = unitOfWork.UserRepository.Get(filter: u => u.UserName == username);

            if (user == null)
                return Result.Failure($"Couldn't find user with username: {username}");

            user.Blocked = true;
            unitOfWork.UserRepository.Update(user);

            if (unitOfWork.SaveChanges(1) == false)
            {
                return Result.Failure($"Couldn't save data for user: {user.UserName} of type: {user.UserType}");
            }

            return Result.Success();
        }

        [AuthorizeUser(UserType.Admin)]
        public Result Unblock(string username)
        {
            var user = unitOfWork.UserRepository.Get(filter: u => u.UserName == username);

            if (user == null)
                return Result.Failure($"Couldn't find user with username: {username}");

            user.Blocked = false;
            unitOfWork.UserRepository.Update(user);

            if (unitOfWork.SaveChanges(1) == false)
            {
                return Result.Failure($"Couldn't save data for user: {user.UserName} of type: {user.UserType}");
            }

            return Result.Success();
        }
    }
}
