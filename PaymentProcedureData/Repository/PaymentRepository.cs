using Microsoft.EntityFrameworkCore;
using PaymentProcedureData.DatabaseContext;
using PaymentProcedureData.Entities;
using PaymentProcedureData.IRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentProcedureData.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IAppDbContext context;
        public PaymentRepository(IAppDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> InsertPayment(PaymentProcess paymentprocess)
        {
            PaymentProcess _paymentProcess = new PaymentProcess();
            _paymentProcess = context.PaymentProcesses.Where(x => x.CreditCardNumber == paymentprocess.CreditCardNumber).FirstOrDefault();

            if (_paymentProcess != null)
            {
                _paymentProcess.ExpirationDate = paymentprocess.ExpirationDate;
                _paymentProcess.Amount = paymentprocess.Amount;
            }
            else
            {
                context.PaymentProcesses.Add(new PaymentProcess
                {
                    CardHolder = paymentprocess.CardHolder,
                    CreditCardNumber = paymentprocess.CreditCardNumber,
                    Amount = paymentprocess.Amount,
                    ExpirationDate = paymentprocess.ExpirationDate,
                    SecurityCode = paymentprocess.SecurityCode,
                    IsDeleted = false,
                    CreatedBy = "Super Admin"
                });
            }

            return await context.Instance.SaveChangesAsync() > 0;
        }

        public async Task<List<Status>> InsertOrGetStatus()
        {
            context.Statuses.Add(new Status
            {
                StatusCode = "Pending"
            });
            context.Statuses.Add(new Status
            {
                StatusCode = "Failed"
            });
            context.Statuses.Add(new Status
            {
                StatusCode = "Processed"
            });

            return await context.Statuses.CountAsync() == 0 ? await context.Instance.SaveChangesAsync() > 0 ? await context.Statuses.ToListAsync() : new List<Status>() : await context.Statuses.ToListAsync();
        }

        public async Task<bool> UpdatePaymentStatus(string status, string ccNo)
        {
            context.PaymentStatuses.Add(new PaymentStatus 
            {
                StatusId = context.Statuses.Where(x => x.StatusCode == status).FirstOrDefault().Id,
                PaymentProcessId = context.PaymentProcesses.Where(x => x.CreditCardNumber == ccNo).FirstOrDefault().Id
            });

            return await context.Instance.SaveChangesAsync() > 0;
        }
    }
}