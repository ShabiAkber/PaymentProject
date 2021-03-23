using PaymentProcedureData.Entities.common;
using System;
using System.Collections.Generic;

namespace PaymentProcedureData.Entities
{
    public class Status : IsDelete
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string StatusCode { get; set; }

        public List<PaymentStatus> PaymentStatuses { get; set; }
    }
}