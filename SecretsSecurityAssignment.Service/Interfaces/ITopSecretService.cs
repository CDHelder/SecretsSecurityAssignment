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
        public Result<TopSecret> Get(int id);
        public Result<List<TopSecret>> GetAll();
        public Result Create(string content, string name);
        public Result Delete(int topSecretId);
        public Result Update(TopSecret secret);

    }
}
