using System;
using System.Threading.Tasks;
using DutyCycle.Infrastructure.EntityFramework;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DutyCycle.API.Filters
{
    public class TransactionFilter : IAsyncActionFilter
    {
        private readonly DutyCycleDbContext _dbContext;

        public TransactionFilter(DutyCycleDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await _dbContext.Database.BeginTransactionAsync();

            var actionExecutedContext = await next();

            if (actionExecutedContext.Exception == null)
            {
                await _dbContext.Database.CommitTransactionAsync();
            }
            else
            {
                await _dbContext.Database.RollbackTransactionAsync();
            }
        }
    }
}