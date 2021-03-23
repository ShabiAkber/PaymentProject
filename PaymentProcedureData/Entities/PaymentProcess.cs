using PaymentProcedureData.Entities.common;
using System;
using System.Collections.Generic;

namespace PaymentProcedureData.Entities
{
    public class PaymentProcess : UpdateBy
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string CreditCardNumber { get; set; }
        public string CardHolder { get; set; }
        public string ExpirationDate { get; set; }
        public string SecurityCode { get; set; }
        public double Amount { get; set; }

        public List<PaymentStatus> PaymentStatuses { get; set; }
    }
}
