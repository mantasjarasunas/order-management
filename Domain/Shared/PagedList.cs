using System.Collections.Generic;

namespace Domain.Shared
{
    public class PagedList<T>
    {
        public IEnumerable<T> Items { get; set; } = [];

        public int TotalCount { get; set; }
    }
}