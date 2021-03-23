using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentProcedureCore.IService;
using PaymentProcedureData.Entities;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PaymentProcedureAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService paymentService;
        private readonly ICheapPaymentGatewayService cheapPaymentGatewayService;
        private readonly IExpensivePaymentGatewayService expensivePaymentGatewayService;
        private readonly IPremiumPaymentService premiumPaymentService;

        public PaymentController(IPaymentService paymentService, ICheapPaymentGatewayService cheapPaymentGatewayService, IExpensivePaymentGatewayService expensivePaymentGatewayService, IPremiumPaymentService premiumPaymentService)
        {
            this.paymentService = paymentService;
            this.cheapPaymentGatewayService = cheapPaymentGatewayService;
            this.expensivePaymentGatewayService = expensivePaymentGatewayService;
            this.premiumPaymentService = premiumPaymentService;
        }

        [HttpPost, Route("insertpaymentprocessdata")]
        public async Task<System.Web.Http.IHttpActionResult> InsertPaymentProcessData(PaymentProcess paymentprocess)
        {
            bool inserted = false;
            if (ValidateCreditCard(paymentprocess.CreditCardNumber))
            {
                if (IsCreditCardInfoValid(paymentprocess.ExpirationDate, paymentprocess.SecurityCode))
                {
                    if (paymentprocess.Amount > 0)
                    {
                        inserted = await paymentService.InsertPayment(paymentprocess);
                        if (inserted)
                        {
                            if (paymentprocess.Amount <= 20)
                            {
                                if (await cheapPaymentGatewayService.AnalysisPaymentByThisGatewayProcessed(paymentprocess.Amount, paymentprocess.CreditCardNumber))
                                    return (System.Web.Http.IHttpActionResult)Ok();
                                else if (await cheapPaymentGatewayService.AnalysisPaymentByThisGatewayPending(paymentprocess.Amount, paymentprocess.CreditCardNumber))
                                    return (System.Web.Http.IHttpActionResult)Ok();
                                else if (await cheapPaymentGatewayService.AnalysisPaymentByThisGatewayFailed(paymentprocess.Amount, paymentprocess.CreditCardNumber))
                                    return (System.Web.Http.IHttpActionResult)Ok();
                            }
                            else if (paymentprocess.Amount >= 21 && paymentprocess.Amount <= 500)
                            {
                                if (await expensivePaymentGatewayService.AnalysisPaymentByThisGatewayProcessed(paymentprocess.Amount, paymentprocess.CreditCardNumber))
                                    return (System.Web.Http.IHttpActionResult)Ok();
                                else if (await expensivePaymentGatewayService.AnalysisPaymentByThisGatewayPending(paymentprocess.Amount, paymentprocess.CreditCardNumber))
                                    return (System.Web.Http.IHttpActionResult)Ok();
                                else if (await expensivePaymentGatewayService.AnalysisPaymentByThisGatewayFailed(paymentprocess.Amount, paymentprocess.CreditCardNumber))
                                    return (System.Web.Http.IHttpActionResult)Ok();
                            }
                            else if (paymentprocess.Amount > 500)
                            {
                                if (await premiumPaymentService.AnalysisPaymentByThisGatewayProcessed(paymentprocess.Amount, paymentprocess.CreditCardNumber))
                                    return (System.Web.Http.IHttpActionResult)Ok();
                                else if (await premiumPaymentService.AnalysisPaymentByThisGatewayPending(paymentprocess.Amount, paymentprocess.CreditCardNumber))
                                    return (System.Web.Http.IHttpActionResult)Ok();
                                else if (await premiumPaymentService.AnalysisPaymentByThisGatewayFailed(paymentprocess.Amount, paymentprocess.CreditCardNumber))
                                    return (System.Web.Http.IHttpActionResult)Ok();
                            }

                            return (System.Web.Http.IHttpActionResult)Ok();
                        }
                        else if (!inserted)
                            return (System.Web.Http.IHttpActionResult)BadRequest();
                        else
                            return (System.Web.Http.IHttpActionResult)StatusCode(500);
                    }
                    else
                        return (System.Web.Http.IHttpActionResult)BadRequest("Amount can never be negative or zero");
                }
                else
                    return (System.Web.Http.IHttpActionResult)BadRequest("Expiry Date or CVV no is invalid");
            }
            else
                return (System.Web.Http.IHttpActionResult)BadRequest("Credit card is invalid");
        }

        [HttpGet, Route("getstatuscode")]
        public async Task<List<Status>> GetStatusCode()
        {
            return await paymentService.InsertOrGetStatus();
        }

        public bool ValidateCreditCard(string creditCardNumber)
        {
            //Build your Regular Expression
            Regex expression = new Regex(@"^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14}|6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|(?:2131|1800|35\d{3})\d{11})$");
            //Return if it was a match or not
            return expression.IsMatch(creditCardNumber);
        }

        public static bool IsCreditCardInfoValid(string expiryDate, string cvv)
        {
            var monthCheck = new Regex(@"^(0[1-9]|1[0-2])$");
            var yearCheck = new Regex(@"^20[0-9]{2}$");
            var cvvCheck = new Regex(@"^\d{3}$");

            if (!cvvCheck.IsMatch(cvv)) // <2>check cvv is valid as "999"
                return false;

            var dateParts = expiryDate.Split('/'); //expiry date in from MM/yyyy            
            if (!monthCheck.IsMatch(dateParts[0]) || !yearCheck.IsMatch(dateParts[1])) // <3 - 6>
                return false; // ^ check date format is valid as "MM/yyyy"

            var year = int.Parse(dateParts[1]);
            var month = int.Parse(dateParts[0]);
            var lastDateOfExpiryMonth = DateTime.DaysInMonth(year, month); //get actual expiry date
            var cardExpiry = new DateTime(year, month, lastDateOfExpiryMonth, 23, 59, 59);

            //check expiry greater than today & within next 6 years <7, 8>>
            return (cardExpiry > DateTime.Now && cardExpiry < DateTime.Now.AddYears(6));
        }
    }
}