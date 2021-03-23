using PaymentProcedureData.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentProcedureCore.IService
{
    public interface IPaymentService
    {
        Task<bool> InsertPayment(PaymentProcess paymentprocess);
        Task<List<Status>> InsertOrGetStatus();
    }
}