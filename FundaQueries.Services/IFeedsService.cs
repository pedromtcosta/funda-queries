using CSharpFunctionalExtensions;
using FundaQueries.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FundaQueries.Services
{
    public interface IFeedsService
    {
        Task<Result<ICollection<Feed>>> GetAllFeeds(bool onlyPropertiesWithTuin = false);
    }
}
