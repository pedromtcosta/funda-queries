using FundaQueries.Models;
using System.Collections.Generic;

namespace FundaQueries.Services
{
    public interface IFeedsService
    {
        ICollection<Feed> GetAllFeeds();
    }
}
