using PaymentProcedureCore.IService;
using PaymentProcedureData.IRepository;
using System.Threading.Tasks;

namespace PaymentProcedureCore.Service
{
    public class CheapPaymentGatewayService : ICheapPaymentGatewayService
    {
        private readonly IPaymentRepository paymentRepository;
        public CheapPaymentGatewayService(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        public async Task<bool> AnalysisPaymentByThisGatewayFailed(double amount, string cc_number)
        {
            return await paymentRepository.UpdatePaymentStatus("Failed", cc_number);
        }

        public async Task<bool> AnalysisPaymentByThisGatewayPending(double amount, string cc_number)
        {
            return await paymentRepository.UpdatePaymentStatus("Pending", cc_number);
        }

        public async Task<bool> AnalysisPaymentByThisGatewayProcessed(double amount, string cc_number)
        {
            return await paymentRepository.UpdatePaymentStatus("Processed", cc_number);
        }
    }
}