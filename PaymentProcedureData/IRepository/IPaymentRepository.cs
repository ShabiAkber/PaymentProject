using PaymentProcedureData.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentProcedureData.IRepository
{
    public interface IPaymentRepository
    {
        Task<bool> InsertPayment(PaymentProcess paymentprocess);
        Task<List<Status>> InsertOrGetStatus();
        Task<bool> UpdatePaymentStatus(string status, string ccNo);
    }
}