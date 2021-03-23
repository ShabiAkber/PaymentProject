using System;

namespace PaymentProcedureData.Entities.common
{
    public abstract class UpdateBy : CreateBy
    {
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
