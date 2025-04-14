using System.Data;
using Domain.Shared;

namespace Persistence.Infrastructure
{
    public class DbRepository
    {
        private readonly IDbContext _dbContext;

        protected IDbConnection Connection => _dbContext.UnitOfWork.Transaction.Connection;
        private IDbTransaction Transaction => _dbContext.UnitOfWork.Transaction;
        
        protected DbRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected static string AddPaging(QueryParameters queryParams)
        {
            return $@"
                OFFSET {(queryParams.PageNumber - 1) * queryParams.PageSize} ROWS 
                FETCH NEXT {queryParams.PageSize} ROWS ONLY
            "; 
        }
        
        protected static string AddOrder(QueryParameters queryParams)
        {
            if (queryParams is { OrderBy: not null, OrderDirection: not null })
            {
                return $@"
                    ORDER BY {queryParams.OrderBy} {queryParams.OrderDirection}
                "; 
            }

            return string.Empty;
        }
    }
}
