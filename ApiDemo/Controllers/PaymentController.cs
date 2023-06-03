using ApiDemo.Response_Module;
using Core.Entities;
using Core.Entities.Interface;
using Core.Entities.OrderAgregate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace ApiDemo.Controllers
{
    
    public class PaymentController : BaseController
    {
        private readonly IPaymentServices _paymentServices;
        private readonly ILogger _logger;
        private const string WhSecret="whsec_64cd6d0033dfddcdc42799bd00e992d1e73b864d3439cc95a08b86541bf941e7";

        public PaymentController( IPaymentServices paymentServices ,ILogger logger)
        {
            _paymentServices = paymentServices;
            _logger = logger;
        }


        [HttpPost("basketId")]
        public async Task<ActionResult<CustomerBasket>>CreateOrUpdatePaymentIntent( string basketId)
        {
            var basket= await _paymentServices.CreateOrUpdatePaymentIntent(basketId);
            if (basket == null)
                return BadRequest(new ApiResponse(400, "problem with your basket"));
            return Ok(basket);
        }
        [HttpPost("webHook")]
        public async Task<ActionResult> StripeWebHook()
        {
            var json=await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], WhSecret);
            PaymentIntent intent;
            Order order;
            switch (stripeEvent.Type)
            {
                case Events.PaymentIntentPaymentFailed:
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment failed: ", intent.Id);
                    order = await _paymentServices.UpdateOrderPaymentFailed(intent.Id);
                    _logger.LogInformation("Payment failed: ", order.Id);


                    break;
                case Events.PaymentIntentSucceeded:

                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment Succeeded: ", intent.Id);
                    order = await _paymentServices.UpdateOrderPaymentSucceeded(intent.Id);
                    _logger.LogInformation("Payment Succeeded: ", order.Id);
                    break;
            }
            return new EmptyResult();
        }
    }
}
