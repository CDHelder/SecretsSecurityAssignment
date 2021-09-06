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
        public Result<SensitiveSecret> Get(int id);
        public Result<List<SensitiveSecret>> GetAll();
        public Result Create(string content, string name);
        public Result Delete(int sensitiveSecretId);
        public Result Update(SensitiveSecret secret);
    }
}
