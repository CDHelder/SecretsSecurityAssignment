using CSharpFunctionalExtensions;
using SecretsSecurityAssignment.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSecurityAssignment.Service.Interfaces
{
    public interface IStateSecretService
    {
        public Result<StateSecret> Get(int id);
        public Result<List<StateSecret>> GetAll();
        public Result Create(string content, string name);
        public Result Delete(int stateSecretId);
        public Result Update(StateSecret secret);

    }
}
