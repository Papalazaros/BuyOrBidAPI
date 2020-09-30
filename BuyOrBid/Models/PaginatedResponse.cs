using System.Collections.Generic;

namespace BuyOrBid.Models
{
    public class PaginatedResponse<T> where T : class
    {
        public IEnumerable<T> Results { get; set; }
        public int Page { get; set; }
        public long? Total { get; set; }
        public long? Remaining { get; set; }

        public PaginatedResponse(IEnumerable<T> results, int page, long? total = null)
        {
            Results = results;
            Page = page;
            Total = total;
        }
    }
}
