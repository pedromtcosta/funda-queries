using CSharpFunctionalExtensions;
using FluentAssertions;
using FundaQueries.Controllers;
using FundaQueries.Dto;
using FundaQueries.Models;
using FundaQueries.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FundaQueries.Spec
{
    public class MakelaarsControllerSpec
    {
        [Fact]
        public async Task Should_return_top_10_makelaars()
        {
            var feeds = FeedsBuilder.Start
                .Makelaar("Makelaar1").HasFeeds(88)
                .Makelaar("Makelaar2").HasFeeds(57)
                .Makelaar("Makelaar3").HasFeeds(14)
                .Makelaar("Makelaar4").HasFeeds(75)
                .Makelaar("Makelaar5").HasFeeds(1)
                .Makelaar("Makelaar6").HasFeeds(20)
                .Makelaar("Makelaar7").HasFeeds(100)
                .Makelaar("Makelaar8").HasFeeds(68)
                .Makelaar("Makelaar9").HasFeeds(3)
                .Makelaar("Makelaar10").HasFeeds(44)
                .Makelaar("Makelaar11").HasFeeds(74)
                .Build();

            var feedsService = new Mock<IFeedsService>();
            feedsService.Setup(f => f.GetAllFeeds(false)).Returns(Task.FromResult(Result.Ok<ICollection<Feed>>(feeds)));

            var controller = new MakelaarsController(feedsService.Object);

            var result = await controller.Top10();
            var makelaars = result.GetValue<MakelaarDto[]>();

            result.ShouldHaveStatusCode(System.Net.HttpStatusCode.OK);
            makelaars.Length.Should().Be(10);
            makelaars[0].Should().BeEquivalentTo(new MakelaarDto { Name = "Makelaar7", PropertiesForSale = 100 });
            makelaars[1].Should().BeEquivalentTo(new MakelaarDto { Name = "Makelaar1", PropertiesForSale = 88 });
            makelaars[2].Should().BeEquivalentTo(new MakelaarDto { Name = "Makelaar4", PropertiesForSale = 75 });
            makelaars[3].Should().BeEquivalentTo(new MakelaarDto { Name = "Makelaar11", PropertiesForSale = 74 });
            makelaars[4].Should().BeEquivalentTo(new MakelaarDto { Name = "Makelaar8", PropertiesForSale = 68 });
            makelaars[5].Should().BeEquivalentTo(new MakelaarDto { Name = "Makelaar2", PropertiesForSale = 57 });
            makelaars[6].Should().BeEquivalentTo(new MakelaarDto { Name = "Makelaar10", PropertiesForSale = 44 });
            makelaars[7].Should().BeEquivalentTo(new MakelaarDto { Name = "Makelaar6", PropertiesForSale = 20 });
            makelaars[8].Should().BeEquivalentTo(new MakelaarDto { Name = "Makelaar3", PropertiesForSale = 14 });
            makelaars[9].Should().BeEquivalentTo(new MakelaarDto { Name = "Makelaar9", PropertiesForSale = 3 });
        }

        [Fact]
        public async Task Should_return_500_error_if_api_request_fails()
        {
            var feedsService = new Mock<IFeedsService>();
            feedsService.Setup(f => f.GetAllFeeds(false)).Returns(Task.FromResult(Result.Fail<ICollection<Feed>>("Error")));

            var controller = new MakelaarsController(feedsService.Object);
            var result = await controller.Top10();

            result.ShouldHaveStatusCode(System.Net.HttpStatusCode.InternalServerError);
        }
    }
}
