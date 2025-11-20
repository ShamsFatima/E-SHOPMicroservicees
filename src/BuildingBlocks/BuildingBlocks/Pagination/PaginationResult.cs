using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Pagination
{
    public class PaginationResult<TEntity>(int pageIndex, int pageSize, long totalItems, IEnumerable<TEntity> items) where TEntity:class
    {
        public int PageIndex { get; } = pageIndex;
        public int PageSize { get; } = pageSize;
        public long TotalItems { get; } = totalItems;
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public IEnumerable<TEntity> Items { get; } = items;
    }
}
