using CSharpFunctionalExtensions;
using SecretsSecurityAssignment.Core;
using SecretsSecurityAssignment.Core.Data.Service;
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

        public Result Create(string content, string name)
        {
            var userId = jWTService.GetUserIdFromTokenInHttpContext();

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

        public Result<List<StateSecret>> GetAll()
        {
            var stateSecrets =  unitOfWork.StateSecretRepository.GetAll();

            if (stateSecrets == null)
                return Result.Failure<List<StateSecret>>("Couln't get any StateSecrets from database");

            return Result.Success(stateSecrets);
        }

        public Result<StateSecret> Get(int id)
        {
            var stateSecret = unitOfWork.StateSecretRepository.GetById(id);

            if (stateSecret == null)
                return Result.Failure<StateSecret>($"Couldn't find StateSecret with id: {id}");

            return Result.Success(stateSecret);
        }

        public Result Update(StateSecret secret)
        {
            unitOfWork.StateSecretRepository.Update(secret);

            if (unitOfWork.SaveChanges(1) == false)
                return Result.Failure($"{secret.Name} couldn't be saved");

            return Result.Success();
        }
    }
}
