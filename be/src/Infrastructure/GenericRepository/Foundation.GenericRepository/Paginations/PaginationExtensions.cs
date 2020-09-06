namespace Foundation.GenericRepository.Paginations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class PaginationExtensions
    {
        public static PagedResults<IEnumerable<T>> PagedIndex<T>(this IQueryable<T> query,
            Pagination pagination, int pageIndex)
        {
            if (pagination.TotalItems == 0)
            {
                return new PagedResults<IEnumerable<T>>(pagination.TotalItems, pagination.PageSize)
                {
                    Data = new List<T>()
                };
            }

            if (pageIndex < pagination.MinPage || pageIndex > pagination.MaxPage)
            {
                throw new ArgumentException($"Page index must >= {pagination.MinPage} and = < {pagination.MaxPage}");
            }

            var pagedResult = new PagedResults<IEnumerable<T>>(pagination.TotalItems, pagination.PageSize)
            {
                Data = query.Skip(GetSkip(pageIndex, pagination.PageSize)).Take(pagination.PageSize)
            };

            return pagedResult;
        }

        private static int GetSkip(int pageIndex, int take)
        {
            return (pageIndex - 1) * take;
        }
    }
}
