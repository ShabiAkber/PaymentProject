using System.Threading.Tasks;

namespace PaymentProcedureCore.IService
{
    public interface IExpensivePaymentGatewayService
    {
        Task<bool> AnalysisPaymentByThisGatewayProcessed(double amount, string ccNo);
        Task<bool> AnalysisPaymentByThisGatewayPending(double amount, string ccNo);
        Task<bool> AnalysisPaymentByThisGatewayFailed(double amount, string ccNo);
    }
}