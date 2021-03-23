using PaymentProcedureData.Entities;
using System.Threading.Tasks;

namespace PaymentProcedureData.IRepository
{
    public interface ILoginRepository
    {
        Task<string> UserIdByUserName(string userName);
        Task<bool> LoginCredentials(UserLogin userLogin);
    }
}