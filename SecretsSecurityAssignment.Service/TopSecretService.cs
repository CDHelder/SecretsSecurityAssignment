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
        private readonly IJWTService jWTService;

        public TopSecretService(IUnitOfWork unitOfWork, IJWTService jWTService)
        {
            this.unitOfWork = unitOfWork;
            this.jWTService = jWTService;
        }

        public Result Create(string content, string name)
        {
            var userId = jWTService.GetUserIdFromTokenInHttpContext();

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

        public Result<List<TopSecret>> GetAll()
        {
            var topSecrets = unitOfWork.TopSecretRepository.GetAll();

            if (topSecrets == null)
                return Result.Failure<List<TopSecret>>("Couln't get any TopSecrets from database");

            return Result.Success(topSecrets);
        }

        public Result<TopSecret> Get(int id)
        {
            var topSecret = unitOfWork.TopSecretRepository.GetById(id);

            if (topSecret == null)
                return Result.Failure<TopSecret>($"Couldn't find TopSecret with id: {id}");

            return Result.Success(topSecret);
        }

        public Result Update(TopSecret secret)
        {
            unitOfWork.TopSecretRepository.Update(secret);

            if (unitOfWork.SaveChanges(1) == false)
                return Result.Failure($"{secret.Name} couldn't be saved");

            return Result.Success();
        }
    }
}
