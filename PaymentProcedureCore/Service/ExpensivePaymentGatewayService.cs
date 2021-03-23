using PaymentProcedureCore.IService;
using PaymentProcedureData.IRepository;
using System.Threading.Tasks;

namespace PaymentProcedureCore.Service
{
    public class ExpensivePaymentGatewayService : IExpensivePaymentGatewayService
    {
        private readonly IPaymentRepository paymentRepository;
        public ExpensivePaymentGatewayService(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        public async Task<bool> AnalysisPaymentByThisGatewayFailed(double amount, string ccNo)
        {
            return await paymentRepository.UpdatePaymentStatus("Failed", ccNo);
        }

        public async Task<bool> AnalysisPaymentByThisGatewayPending(double amount, string ccNo)
        {
            return await paymentRepository.UpdatePaymentStatus("Pending", ccNo);
        }

        public async Task<bool> AnalysisPaymentByThisGatewayProcessed(double amount, string ccNo)
        {
            return await paymentRepository.UpdatePaymentStatus("Processed", ccNo);
        }
    }
}