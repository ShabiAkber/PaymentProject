using PaymentProcedureCore.IService;
using PaymentProcedureData.Entities;
using PaymentProcedureData.IRepository;
using System.Threading.Tasks;

namespace PaymentProcedureCore.Service
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository loginRepository;
        public LoginService(ILoginRepository loginRepository)
        {
            this.loginRepository = loginRepository;
        }

        public async Task<bool> LoginCredentials(UserLogin userLogin)
        {
            return await loginRepository.LoginCredentials(userLogin);
        }

        public async Task<string> UserIdByUserName(string userName)
        {
            return await loginRepository.UserIdByUserName(userName);
        }
    }
}