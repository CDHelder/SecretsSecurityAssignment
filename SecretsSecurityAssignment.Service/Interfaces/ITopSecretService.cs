using CSharpFunctionalExtensions;
using SecretsSecurityAssignment.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSecurityAssignment.Service.Interfaces
{
    public interface ITopSecretService
    {
        public Result<TopSecret> GetTopSecret(int id);
        public Result<List<TopSecret>> GetAllTopSecrets();
        public Result CreateTopSecret(TopSecret topSecret);
        public Result DeleteTopSecret(int topSecretId);
    }
}
