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
    public class StateSecretService : IStateSecretService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IJWTService jWTService;

        public StateSecretService(IUnitOfWork unitOfWork, IJWTService jWTService)
        {
            this.unitOfWork = unitOfWork;
            this.jWTService = jWTService;
        }

        [AuthorizeUser(UserType.GovermentEmployee)]
        public Result Create(string content, string name)
        {
            if (string.IsNullOrWhiteSpace(content) || string.IsNullOrWhiteSpace(name))
                Result.Failure("Please enter a name and content");

            var userId = jWTService.GetUserIdFromTokenInHttpContext();

            if (unitOfWork.UserRepository.GetById(userId).Blocked == true)
            {
                return Result.Failure("User is banned");
            }

            StateSecret stateSecret = new StateSecret
            {
                Content = content,
                Name = name,
                UserId = userId
            };

            unitOfWork.StateSecretRepository.Create(stateSecret);

            if (unitOfWork.SaveChanges(1) == false)
                return Result.Failure($"{stateSecret.Name} couldn't be saved");

            return Result.Success();
        }

        [AuthorizeUser(UserType.Admin)]
        public Result Delete(int stateSecretId)
        {
            var stateSecret = Get(stateSecretId).Value;

            if (stateSecret == null)
                return Result.Failure($"Couldn't find TopSecret with id: {stateSecretId}");

            unitOfWork.StateSecretRepository.Delete(stateSecret);

            if (unitOfWork.SaveChanges(1) == false)
                return Result.Failure($"{stateSecret.Name} couldn't be saved");

            return Result.Success();
        }

        [AuthorizeUser(UserType.GovermentEmployee)]
        public Result<List<StateSecret>> GetAll()
        {
            var userId = jWTService.GetUserIdFromTokenInHttpContext();

            if (unitOfWork.UserRepository.GetById(userId).Blocked == true)
            {
                return Result.Failure<List<StateSecret>>("User is banned");
            }

            var stateSecrets =  unitOfWork.StateSecretRepository.GetAll();

            if (stateSecrets == null)
                return Result.Failure<List<StateSecret>>("Couln't get any StateSecrets from database");

            return Result.Success(stateSecrets);
        }

        [AuthorizeUser(UserType.GovermentEmployee)]
        public Result<StateSecret> Get(int id)
        {
            var userId = jWTService.GetUserIdFromTokenInHttpContext();

            if (unitOfWork.UserRepository.GetById(userId).Blocked == true)
            {
                return Result.Failure<StateSecret>("User is banned");
            }

            var stateSecret = unitOfWork.StateSecretRepository.GetById(id);

            if (stateSecret == null)
                return Result.Failure<StateSecret>($"Couldn't find StateSecret with id: {id}");

            return Result.Success(stateSecret);
        }

        public Result Update(StateSecret secret)
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

            unitOfWork.StateSecretRepository.Update(secret);

            if (unitOfWork.SaveChanges(1) == false)
                return Result.Failure($"{secret.Name} couldn't be saved");

            return Result.Success();
        }
    }
}
