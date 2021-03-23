using PaymentProcedureData.Entities;
using System.Threading.Tasks;

namespace PaymentProcedureCore.IService
{
    public interface ILoginService
    {
        Task<string> UserIdByUserName(string userName);
        Task<bool> LoginCredentials(UserLogin userLogin);
    }
}