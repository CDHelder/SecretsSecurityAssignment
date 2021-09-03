using CSharpFunctionalExtensions;
using SecretsSecurityAssignment.Core.UserEntities;

namespace SecretsSecurityAssignment.Service
{
    public interface IUserService
    {
        public Result<string> Login(string username, string password);
        public Result Register(string username, string password, UserType userType);
        public Result Block(int userId);
        public Result Block(int[] userIds);
        public Result Unblock(int userId);
        public Result Unblock(int[] userIds);
    }
}