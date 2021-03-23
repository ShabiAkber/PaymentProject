using System;

namespace PaymentProcedureData.Entities.common
{
    public abstract class CreateBy : IsDelete
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
