using PaymentProcedureData.Entities.common;
using System;

namespace PaymentProcedureData.Entities
{
    public class UserLogin : IsDelete
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}