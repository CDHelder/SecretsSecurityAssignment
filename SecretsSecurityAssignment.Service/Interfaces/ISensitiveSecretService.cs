using CSharpFunctionalExtensions;
using SecretsSecurityAssignment.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSecurityAssignment.Service.Interfaces
{
    public interface ISensitiveSecretService
    {
        public Result<SensitiveSecret> GetSensitiveSecret(int id);
        public Result<List<SensitiveSecret>> GetAllSensitiveSecrets();
        public Result CreateSensitiveSecret(SensitiveSecret sensitiveSecret);
        public Result DeleteSensitiveSecret(int sensitiveSecretId);
    }
}
