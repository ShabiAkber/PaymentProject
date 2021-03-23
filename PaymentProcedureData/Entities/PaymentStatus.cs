using PaymentProcedureData.Entities.common;
using System;

namespace PaymentProcedureData.Entities
{
    public class PaymentStatus : IsDelete
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string StatusId { get; set; }
        public virtual Status Status { get; set; }

        public string PaymentProcessId { get; set; }
        public virtual PaymentProcess PaymentProcess { get; set; }
    }
}