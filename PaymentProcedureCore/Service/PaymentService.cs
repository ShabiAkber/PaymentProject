using PaymentProcedureCore.IService;
using PaymentProcedureData.Entities;
using PaymentProcedureData.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentProcedureCore.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository paymentRepository;
        public PaymentService(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        public async Task<bool> InsertPayment(PaymentProcess paymentprocess)
        {
            return await paymentRepository.InsertPayment(paymentprocess);
        }

        public async Task<List<Status>> InsertOrGetStatus()
        {
            return await paymentRepository.InsertOrGetStatus();
        }
    }
}