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
        private readonly IJWTService jWTService;

        public SensitiveSecretService(IUnitOfWork unitOfWork, IJWTService jWTService)
        {
            this.unitOfWork = unitOfWork;
            this.jWTService = jWTService;
        }

        public Result Create(string content, string name)
        {
            var userId = jWTService.GetUserIdFromTokenInHttpContext();

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

        public Result<List<SensitiveSecret>> GetAll()
        {
            var sensitiveSecrets = unitOfWork.SensitiveSecretRepository.GetAll();

            if (sensitiveSecrets == null)
                return Result.Failure<List<SensitiveSecret>>("Couln't get any SensitiveSecrets from database");

            return Result.Success(sensitiveSecrets);
        }

        public Result<SensitiveSecret> Get(int id)
        {
            var sensitiveSecret = unitOfWork.SensitiveSecretRepository.GetById(id);

            if (sensitiveSecret == null)
                return Result.Failure<SensitiveSecret>($"Couldn't find SensitiveSecret with id: {id}");

            return Result.Success(sensitiveSecret);
        }

        public Result Update(SensitiveSecret secret)
        {
            unitOfWork.SensitiveSecretRepository.Update(secret);

            if (unitOfWork.SaveChanges(1) == false)
                return Result.Failure($"{secret.Name} couldn't be saved");

            return Result.Success();
        }
    }
}
