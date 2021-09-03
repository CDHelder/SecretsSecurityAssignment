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

        public StateSecretService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Result CreateStateSecret(StateSecret stateSecret)
        {
            unitOfWork.StateSecretRepository.Create(stateSecret);

            if (unitOfWork.SaveChanges(1) == false)
                return Result.Failure($"{stateSecret.Name} couldn't be saved");

            return Result.Success();
        }

        public Result DeleteStateSecret(int stateSecretId)
        {
            var stateSecret = GetStateSecret(stateSecretId).Value;

            if (stateSecret == null)
                return Result.Failure($"Couldn't find TopSecret with id: {stateSecretId}");

            unitOfWork.StateSecretRepository.Delete(stateSecret);

            if (unitOfWork.SaveChanges(1) == false)
                return Result.Failure($"{stateSecret.Name} couldn't be saved");

            return Result.Success();
        }

        public Result<List<StateSecret>> GetAllStateSecrets()
        {
            var stateSecrets =  unitOfWork.StateSecretRepository.GetAll();

            if (stateSecrets == null)
                return Result.Failure<List<StateSecret>>("Couln't get any StateSecrets from database");

            return Result.Success(stateSecrets);
        }

        public Result<StateSecret> GetStateSecret(int id)
        {
            var stateSecret = unitOfWork.StateSecretRepository.GetById(id);

            if (stateSecret == null)
                return Result.Failure<StateSecret>($"Couldn't find StateSecret with id: {id}");

            return Result.Success(stateSecret);
        }
    }
}
