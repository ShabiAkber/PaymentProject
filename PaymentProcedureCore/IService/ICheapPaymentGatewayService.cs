using System.Threading.Tasks;

namespace PaymentProcedureCore.IService
{
    public interface ICheapPaymentGatewayService
    {
        Task<bool> AnalysisPaymentByThisGatewayProcessed(double amount, string cc_number);
        Task<bool> AnalysisPaymentByThisGatewayPending(double amount, string cc_number);
        Task<bool> AnalysisPaymentByThisGatewayFailed(double amount, string cc_number);
    }
}