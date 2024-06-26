using Microsoft.AspNetCore.Mvc.Filters;
using UPBank.DTOs;

namespace UPBank.Accounts.Filters
{
    public class PopulateOriginNumberActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.TryGetValue("requestedTransaction", out object? requestedTransactionObj) &&
            requestedTransactionObj is TransactionCreationDTO requestedTransaction)
            {
                if (context.RouteData.Values.TryGetValue("number", out object? numberObj) && numberObj is string number)
                    requestedTransaction.OriginNumber = number;
            }

            await next();
        }
    }
}
