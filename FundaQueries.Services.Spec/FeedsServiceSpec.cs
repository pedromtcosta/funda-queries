using FizzWare.NBuilder;
using FluentAssertions;
using FundaQueries.Services.ResponseModels;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using RequestUriBuilder = FundaQueries.Services.FeedsService.RequestUriBuilder;

namespace FundaQueries.Services.Spec
{
    public class FeedsServiceSpec
    {
        [Fact]
        public async Task Should_return_list_of_feeds()
        {
            var objects = Builder<ListedObject>.CreateListOfSize(10).Build().ToArray();
            var queryResponse = new QueryResponse
            {
                Objects = objects,
                Paging = new Paging
                {
                    AantalPaginas = 1,
                    HuidigePagina = 1
                }
            };

            var restClient = new Mock<IRestClient>();
            var threadService = new Mock<IThreadService>();

            restClient.Setup(c => c.GetAsync<QueryResponse>(RequestUriBuilder.Default.Build()))
                .Returns(Task.FromResult(RestResponse<QueryResponse>.Ok(queryResponse)));

            var service = new FeedsService(restClient.Object, threadService.Object);

            var result = await service.GetAllFeeds();
            var feeds = result.Value;
            feeds.Should().HaveCount(10);
        }

        [Theory]
        [InlineData(100, 25)]
        [InlineData(101, 25)]
        [InlineData(106, 25)]
        public async Task Should_return_list_of_feeds_when_have_paging(int totalItems, int pageSize)
        {
            var totalPages = (int)Math.Ceiling((decimal)totalItems / pageSize);

            var restClient = new Mock<IRestClient>();
            var threadService = new Mock<IThreadService>();

            for (int i = 1; i <= totalPages; i++)
            {
                var quantityOfItemsToCreate = i * pageSize > totalItems ? (pageSize - (i * pageSize - totalItems)) : pageSize;
                var objects = Builder<ListedObject>.CreateListOfSize(quantityOfItemsToCreate).Build().ToArray();
                var queryResponse = new QueryResponse
                {
                    Objects = objects,
                    Paging = new Paging
                    {
                        AantalPaginas = totalPages,
                        HuidigePagina = i
                    }
                };
                var requestUri = RequestUriBuilder.Default.AtPage(i).Build();
                restClient.Setup(c => c.GetAsync<QueryResponse>(requestUri))
                    .Returns(Task.FromResult(RestResponse<QueryResponse>.Ok(queryResponse)));
            }

            var service = new FeedsService(restClient.Object, threadService.Object);

            var result = await service.GetAllFeeds();
            var feeds = result.Value;
            feeds.Should().HaveCount(totalItems);
        }

        [Fact]
        public async Task Should_include_parameter_on_url_if_queriyng_for_properties_with_tuin()
        {
            var objects = Builder<ListedObject>.CreateListOfSize(10).Build().ToArray();
            var queryResponse = new QueryResponse
            {
                Objects = objects,
                Paging = new Paging
                {
                    AantalPaginas = 1,
                    HuidigePagina = 1
                }
            };

            var restClient = new Mock<IRestClient>();
            var threadService = new Mock<IThreadService>();

            restClient.Setup(c => c.GetAsync<QueryResponse>(RequestUriBuilder.Default.WithTuin().Build()))
                .Returns(Task.FromResult(RestResponse<QueryResponse>.Ok(queryResponse)));

            var service = new FeedsService(restClient.Object, threadService.Object);

            var result = await service.GetAllFeeds(true);
            var feeds = result.Value;
            feeds.Should().HaveCount(10);

            restClient.Verify(c => c.GetAsync<QueryResponse>(@"http://partnerapi.funda.nl/feeds/Aanbod.svc/json/ac1b0b1572524640a0ecc54de453ea9f/?type=koop&zo=/amsterdam/tuin/&page=1&pagesize=25"));
        }

        [Fact]
        public async Task Should_return_fail_if_http_request_fails()
        {
            var objects = Builder<ListedObject>.CreateListOfSize(10).Build().ToArray();
            var queryResponse = new QueryResponse
            {
                Objects = objects,
                Paging = new Paging
                {
                    AantalPaginas = 1,
                    HuidigePagina = 1
                }
            };

            var restClient = new Mock<IRestClient>();
            var threadService = new Mock<IThreadService>();

            restClient.Setup(c => c.GetAsync<QueryResponse>(RequestUriBuilder.Default.Build()))
                .Returns(Task.FromResult(RestResponse<QueryResponse>.InternalServerError()));

            var service = new FeedsService(restClient.Object, threadService.Object);

            var result = await service.GetAllFeeds();
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task Should_wait_when_request_limit_exceeded()
        {
            var objects = Builder<ListedObject>.CreateListOfSize(10).Build().ToArray();
            var queryResponse = new QueryResponse
            {
                Objects = objects,
                Paging = new Paging
                {
                    AantalPaginas = 1,
                    HuidigePagina = 1
                }
            };

            var restClient = new Mock<IRestClient>();
            var threadService = new Mock<IThreadService>();

            restClient.SetupSequence(c => c.GetAsync<QueryResponse>(RequestUriBuilder.Default.Build()))
                .Returns(Task.FromResult(RestResponse<QueryResponse>.Unauthorized("Request limit exceeded")))
                .Returns(Task.FromResult(RestResponse<QueryResponse>.Ok(queryResponse)));

            var service = new FeedsService(restClient.Object, threadService.Object);

            var result = await service.GetAllFeeds();

            threadService.Verify(t => t.Sleep(60000));
            var feeds = result.Value;
            feeds.Should().HaveCount(10);
        }
    }
}
