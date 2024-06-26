using Microsoft.AspNetCore.Mvc.Filters;
using UPBank.DTOs;

namespace UPBank.Accounts.Filters
{
    public class AddNumberOriginToTransactionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.RouteData.Values.TryGetValue("number", out var numberValue) &&
                context.ActionArguments.TryGetValue("requestedTransaction", out var requestedTransaction))
            {
                var transactionDto = requestedTransaction as TransactionCreationDTO;
                var originNumber = numberValue as string;

                if (transactionDto != null && originNumber != null)
                {
                    transactionDto.OriginNumber = originNumber;
                    context.ActionArguments.Remove("requestedTransaction");
                    context.ActionArguments.Add("requestedTransaction", transactionDto);
                }
            }
        }
    }
}
