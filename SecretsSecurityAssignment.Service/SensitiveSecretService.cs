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
    public class SensitiveSecretService : ISensitiveSecretService
    {
        private readonly IUnitOfWork unitOfWork;

        public SensitiveSecretService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Result CreateSensitiveSecret(SensitiveSecret sensitiveSecret)
        {
            unitOfWork.SensitiveSecretRepository.Create(sensitiveSecret);

            if (unitOfWork.SaveChanges(1) == false)
                return Result.Failure($"{sensitiveSecret.Name} couldn't be saved");

            return Result.Success();
        }

        public Result DeleteSensitiveSecret(int sensitiveSecretId)
        {
            var sensitiveSecret = GetSensitiveSecret(sensitiveSecretId).Value;

            if (sensitiveSecret == null)
                return Result.Failure($"Couldn't find SensitiveSecret with id: {sensitiveSecretId}");

            unitOfWork.SensitiveSecretRepository.Delete(sensitiveSecret);

            if (unitOfWork.SaveChanges(1) == false)
                return Result.Failure($"{sensitiveSecret.Name} couldn't be saved");

            return Result.Success();
        }

        public Result<List<SensitiveSecret>> GetAllSensitiveSecrets()
        {
            var sensitiveSecrets = unitOfWork.SensitiveSecretRepository.GetAll();

            if (sensitiveSecrets == null)
                return Result.Failure<List<SensitiveSecret>>("Couln't get any SensitiveSecrets from database");

            return Result.Success(sensitiveSecrets);
        }

        public Result<SensitiveSecret> GetSensitiveSecret(int id)
        {
            var sensitiveSecret = unitOfWork.SensitiveSecretRepository.GetById(id);

            if (sensitiveSecret == null)
                return Result.Failure<SensitiveSecret>($"Couldn't find SensitiveSecret with id: {id}");

            return Result.Success(sensitiveSecret);
        }
    }
}
