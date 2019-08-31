﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FundaQueries.Dto;
using FundaQueries.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FundaQueries.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MakelaarsController : ControllerBase
    {
        private readonly IFeedsService _feedService;

        public MakelaarsController(IFeedsService feedService)
        {
            _feedService = feedService;
        }

        public IActionResult Top10()
        {
            var feeds = _feedService.GetAllFeeds();

            var makelaars = feeds.GroupBy(f => f.MakelaarName)
                .Select(g => new MakelaarDto { Name = g.Key, PropertiesForSale = g.Count() })
                .OrderByDescending(m => m.PropertiesForSale)
                .Take(10)
                .ToArray();

            return Ok(makelaars);
        }
    }
}