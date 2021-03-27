using System;
using System.Threading.Tasks;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace API.Helpers
{
    public class LogProductActivity : IAsyncActionFilter
    {
         public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            var userId = resultContext.HttpContext.User.GetUserId();
            var uow = resultContext.HttpContext.RequestServices.GetService<IUnitOfWork>();
            // var user = await uow.UserRepository.GetUserByIdAsync(userId);
            var order = await uow.OrderRepository.GetOpenOrderByAppUserIdAsync(userId);
            order.LastUpdate = DateTime.UtcNow;
            await uow.Complete();
        }
    }
}