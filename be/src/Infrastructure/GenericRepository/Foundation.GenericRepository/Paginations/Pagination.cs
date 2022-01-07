using System;

namespace Foundation.GenericRepository.Paginations
{
    public class Pagination
    {
        public long TotalItems { get; }

        public int PageSize { get; }

        public int MinPage { get; } = 1;

        public int MaxPage { get; }

        public Pagination(int totalItems, int itemsPerPage)
        {
            if (itemsPerPage < MinPage)
            {
                throw new ArgumentException( $"Number of items per page must > 0!");
            }

            this.TotalItems = totalItems;
            this.PageSize = itemsPerPage;
            this.MaxPage = this.CalculateTotalPages(totalItems, itemsPerPage);
        }

        private int CalculateTotalPages(int totalItems, int itemsPerPage)
        {
            int totalPages = totalItems / itemsPerPage;

            if (totalItems % itemsPerPage != 0)
            {
                totalPages++;
            }

            return totalPages;
        }
    }
}
