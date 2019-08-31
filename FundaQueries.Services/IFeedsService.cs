using FundaQueries.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FundaQueries.Services
{
    public interface IFeedsService
    {
        Task<ICollection<Feed>> GetAllFeeds(bool onlyPropertiesWithTuin = false);
    }
}
