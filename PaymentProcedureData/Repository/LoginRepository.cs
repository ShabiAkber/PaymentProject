using Microsoft.EntityFrameworkCore;
using PaymentProcedureData.DatabaseContext;
using PaymentProcedureData.Entities;
using PaymentProcedureData.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentProcedureData.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly IAppDbContext context;
        public LoginRepository(IAppDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> LoginCredentials(UserLogin userLogin)
        {
            return await context.UserLogins.Where(x => x.UserName == userLogin.UserName && x.Password == userLogin.Password).AnyAsync();
        }

        public async Task<string> UserIdByUserName(string userName)
        {
            return await context.UserLogins.Where(x => x.UserName == userName).Select(x => x.Id).FirstOrDefaultAsync();
        }
    }
}