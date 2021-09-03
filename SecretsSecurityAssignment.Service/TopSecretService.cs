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
    public class TopSecretService : ITopSecretService
    {
        private readonly IUnitOfWork unitOfWork;

        public TopSecretService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Result CreateTopSecret(TopSecret topSecret)
        {
            unitOfWork.TopSecretRepository.Create(topSecret);

            if (unitOfWork.SaveChanges(1) == false)
                return Result.Failure($"{topSecret.Name} couldn't be saved");

            return Result.Success();
        }

        public Result DeleteTopSecret(int topSecretId)
        {
            var topSecret = GetTopSecret(topSecretId).Value;

            if (topSecret == null)
                return Result.Failure($"Couldn't find TopSecret with id: {topSecretId}");

            unitOfWork.TopSecretRepository.Delete(topSecret);

            if (unitOfWork.SaveChanges(1) == false)
                return Result.Failure($"{topSecret.Name} couldn't be saved");

            return Result.Success();
        }

        public Result<List<TopSecret>> GetAllTopSecrets()
        {
            var topSecrets = unitOfWork.TopSecretRepository.GetAll();

            if (topSecrets == null)
                return Result.Failure<List<TopSecret>>("Couln't get any TopSecrets from database");

            return Result.Success(topSecrets);
        }

        public Result<TopSecret> GetTopSecret(int id)
        {
            var topSecret = unitOfWork.TopSecretRepository.GetById(id);

            if (topSecret == null)
                return Result.Failure<TopSecret>($"Couldn't find TopSecret with id: {id}");

            return Result.Success(topSecret);
        }
    }
}
