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
        public Result<StateSecret> GetStateSecret(int id);
        public Result<List<StateSecret>> GetAllStateSecrets();
        public Result CreateStateSecret(StateSecret stateSecret);
        public Result DeleteStateSecret(int stateSecretId);
    }
}
