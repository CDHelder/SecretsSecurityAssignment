using CSharpFunctionalExtensions;
using SecretsSecurityAssignment.Core;
using SecretsSecurityAssignment.Core.Data.Service;
using SecretsSecurityAssignment.Core.UserEntities;
using SecretsSecurityAssignment.Service.CustomAttributes;
using SecretsSecurityAssignment.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSecurityAssignment.Service
{
    public class TopSecretService : ITopSecretService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IJWTService jWTService;

        public TopSecretService(IUnitOfWork unitOfWork, IJWTService jWTService)
        {
            this.unitOfWork = unitOfWork;
            this.jWTService = jWTService;
        }

        [AuthorizeUser(UserType.SecretAgent)]
        public Result Create(string content, string name)
        {
            if (string.IsNullOrWhiteSpace(content) || string.IsNullOrWhiteSpace(name))
                Result.Failure("Please enter a name and content");

            var userId = jWTService.GetUserIdFromTokenInHttpContext();

            if (unitOfWork.UserRepository.GetById(userId).Blocked == true)
            {
                return Result.Failure("User is banned");
            }

            TopSecret topSecret = new TopSecret
            {
                Content = content,
                Name = name,
                UserId = userId
            };

            unitOfWork.TopSecretRepository.Create(topSecret);

            if (unitOfWork.SaveChanges(1) == false)
                return Result.Failure($"{topSecret.Name} couldn't be saved");

            return Result.Success();
        }

        [AuthorizeUser(UserType.Admin)]
        public Result Delete(int topSecretId)
        {
            var topSecret = Get(topSecretId).Value;

            if (topSecret == null)
                return Result.Failure($"Couldn't find TopSecret with id: {topSecretId}");

            unitOfWork.TopSecretRepository.Delete(topSecret);

            if (unitOfWork.SaveChanges(1) == false)
                return Result.Failure($"{topSecret.Name} couldn't be saved");

            return Result.Success();
        }

        [AuthorizeUser(UserType.SecretAgent)]
        public Result<List<TopSecret>> GetAll()
        {
            var userId = jWTService.GetUserIdFromTokenInHttpContext();

            if (unitOfWork.UserRepository.GetById(userId).Blocked == true)
            {
                return Result.Failure<List<TopSecret>>("User is banned");
            }

            var topSecrets = unitOfWork.TopSecretRepository.GetAll();

            if (topSecrets == null)
                return Result.Failure<List<TopSecret>>("Couln't get any TopSecrets from database");

            return Result.Success(topSecrets);
        }

        [AuthorizeUser(UserType.SecretAgent)]
        public Result<TopSecret> Get(int id)
        {
            var userId = jWTService.GetUserIdFromTokenInHttpContext();

            if (unitOfWork.UserRepository.GetById(userId).Blocked == true)
            {
                return Result.Failure<TopSecret>("User is banned");
            }

            var topSecret = unitOfWork.TopSecretRepository.GetById(id);

            if (topSecret == null)
                return Result.Failure<TopSecret>($"Couldn't find TopSecret with id: {id}");

            return Result.Success(topSecret);
        }

        public Result Update(TopSecret secret)
        {
            var userId = jWTService.GetUserIdFromTokenInHttpContext();

            if (unitOfWork.UserRepository.GetById(userId).Blocked == true)
            {
                return Result.Failure("User is banned");
            }

            if (jWTService.GetUserTypeFromTokenInHttpContext() != UserType.Admin.ToString())
            {
                if (unitOfWork.SensitiveSecretRepository.Get(filter: a => a.Id == secret.Id).UserId != userId)
                {
                    return Result.Failure("Secrets can only be changed by the original creator");
                }
            }

            unitOfWork.TopSecretRepository.Update(secret);

            if (unitOfWork.SaveChanges(1) == false)
                return Result.Failure($"{secret.Name} couldn't be saved");

            return Result.Success();
        }
    }
}
