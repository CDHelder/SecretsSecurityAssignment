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
    public class SensitiveSecretService : ISensitiveSecretService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IJWTService jWTService;

        public SensitiveSecretService(IUnitOfWork unitOfWork, IJWTService jWTService)
        {
            this.unitOfWork = unitOfWork;
            this.jWTService = jWTService;
        }

        [AuthorizeUser(UserType.Civilian)]
        public Result Create(string content, string name)
        {
            if (string.IsNullOrWhiteSpace(content) || string.IsNullOrWhiteSpace(name))
                Result.Failure("Please enter a name and content");

            var userId = jWTService.GetUserIdFromTokenInHttpContext();

            if (unitOfWork.UserRepository.GetById(userId).Blocked == true)
            {
                return Result.Failure("User is banned");
            }

            SensitiveSecret sensitiveSecret = new SensitiveSecret
            { 
                Content = content,
                Name = name,
                UserId = userId
            };

            unitOfWork.SensitiveSecretRepository.Create(sensitiveSecret);

            if (unitOfWork.SaveChanges(1) == false)
                return Result.Failure($"{sensitiveSecret.Name} couldn't be saved");

            return Result.Success();
        }

        [AuthorizeUser(UserType.Admin)]
        public Result Delete(int sensitiveSecretId)
        {
            var sensitiveSecret = Get(sensitiveSecretId).Value;

            if (sensitiveSecret == null)
                return Result.Failure($"Couldn't find SensitiveSecret with id: {sensitiveSecretId}");

            unitOfWork.SensitiveSecretRepository.Delete(sensitiveSecret);

            if (unitOfWork.SaveChanges(1) == false)
                return Result.Failure($"{sensitiveSecret.Name} couldn't be saved");

            return Result.Success();
        }

        [AuthorizeUser(UserType.Civilian)]
        public Result<List<SensitiveSecret>> GetAll()
        {
            var userId = jWTService.GetUserIdFromTokenInHttpContext();

            if (unitOfWork.UserRepository.GetById(userId).Blocked == true)
            {
                return Result.Failure<List<SensitiveSecret>>("User is banned");
            }

            var sensitiveSecrets = unitOfWork.SensitiveSecretRepository.GetAll();

            if (sensitiveSecrets == null)
                return Result.Failure<List<SensitiveSecret>>("Couln't get any SensitiveSecrets from database");

            return Result.Success(sensitiveSecrets);
        }

        [AuthorizeUser(UserType.Civilian)]
        public Result<SensitiveSecret> Get(int id)
        {
            var userId = jWTService.GetUserIdFromTokenInHttpContext();

            if (unitOfWork.UserRepository.GetById(userId).Blocked == true)
            {
                return Result.Failure<SensitiveSecret>("User is banned");
            }

            var sensitiveSecret = unitOfWork.SensitiveSecretRepository.GetById(id);

            if (sensitiveSecret == null)
                return Result.Failure<SensitiveSecret>($"Couldn't find SensitiveSecret with id: {id}");

            return Result.Success(sensitiveSecret);
        }

        public Result Update(SensitiveSecret secret)
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

            unitOfWork.SensitiveSecretRepository.Update(secret);

            if (unitOfWork.SaveChanges(1) == false)
                return Result.Failure($"{secret.Name} couldn't be saved");

            return Result.Success();
        }
    }
}
