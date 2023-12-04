using AutoMapper;
using Dapr.Client;
using GloboTicket.Services.Marketing.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace GloboTicket.Services.Marketing
{
    [Route("scheduled")]
    [ApiController]
    public class ScheduledController : ControllerBase
    {
        private readonly ILogger<ScheduledController> logger;
        private readonly DaprClient daprClient;       
        private readonly BasketChangeEventRepository basketChangeEventRepository;
        private readonly IMapper mapper;

        public ScheduledController(ILogger<ScheduledController> logger, Dapr.Client.DaprClient daprClient, 
                            BasketChangeEventRepository basketChangeEventRepository, IMapper mapper)
        {
            this.logger = logger;
            this.daprClient = daprClient;            
            this.basketChangeEventRepository = basketChangeEventRepository;
            this.mapper = mapper;
        }

        [HttpPost("", Name = "Scheduled")]
        public async void OnSchedule()
        {
            logger.LogInformation("scheduled endpoint called");
            var startDate = DateTime.Now;
            int max = 10;
            var formattedDate = $"{startDate.Year}/{startDate.Month}/{startDate.Day} {startDate.Hour}:{startDate.Minute}:{startDate.Second}";
            var s = $"/api/basketevent?fromDate={formattedDate}&max={max}";
            var response = await daprClient.InvokeMethodAsync<List<GloboTicket.Services.Marketing.Models.BasketChangeEvent>>(HttpMethod.Get, "shoppingbasket", s);
            foreach (var basketChangeEvent in response)
            {
                
                await basketChangeEventRepository.AddBasketChangeEvent(mapper.Map<GloboTicket.Services.Marketing.Entities.BasketChangeEvent>(basketChangeEvent));
            }
        }
    }
}
