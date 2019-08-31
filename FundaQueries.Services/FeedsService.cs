using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FundaQueries.Models;
using FundaQueries.Services.ResponseModels;

namespace FundaQueries.Services
{
    public class FeedsService : IFeedsService
    {
        public class RequestUriBuilder
        {
            const string BaseRequestUrl = @"http://partnerapi.funda.nl/feeds/Aanbod.svc/json/ac1b0b1572524640a0ecc54de453ea9f/?type=koop&zo=/amsterdam/{tuin}&page={page}&pagesize=25";
            private int _page = 1;
            private bool _withTuin;

            private RequestUriBuilder()
            {

            }

            public RequestUriBuilder WithTuin()
            {
                _withTuin = true;
                return this;
            }

            public RequestUriBuilder AtPage(int page)
            {
                _page = page;
                return this;
            }

            public string Build()
            {
                var url = BaseRequestUrl.Replace("{page}", _page.ToString());
                url = _withTuin ? url.Replace("{tuin}", "tuin/") : url.Replace("{tuin}", "tuin");
                return url;
            }

            public static RequestUriBuilder Default => new RequestUriBuilder();
        }

        private readonly IRestClient _restClient;

        public FeedsService(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public async Task<ICollection<Feed>> GetAllFeeds()
        {
            QueryResponse queryResponse;
            var feeds = new List<Feed>();
            var currentPage = 1;

            do
            {
                var response = await _restClient.GetAsync<QueryResponse>(RequestUriBuilder.Default.AtPage(currentPage).Build());
                queryResponse = response.Value;

                var feedsToAdd = queryResponse.Objects.Select(o => new Feed
                {
                    Address = o.Adres,
                    MakelaarName = o.MakelaarNaam
                }).ToList();

                feeds.AddRange(feedsToAdd);
                currentPage++;
            } while (currentPage <= queryResponse.Paging.AantalPaginas);

            return feeds;
        }
    }
}
