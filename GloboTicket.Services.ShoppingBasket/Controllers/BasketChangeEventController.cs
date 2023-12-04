using AutoMapper;
using GloboTicket.Services.ShoppingBasket.Models;
using GloboTicket.Services.ShoppingBasket.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GloboTicket.Services.ShoppingBasket.Controllers
{
    [ApiController]
    [Route("api/basketevent")]
    public class BasketChangeEventController : Controller
    {
        private readonly IBasketChangeEventRepository basketChangeEventRepository;
        private readonly IMapper mapper;

        public BasketChangeEventController(IMapper mapper, IBasketChangeEventRepository basketChangeEventRepository)
        {
            this.mapper = mapper;
            this.basketChangeEventRepository = basketChangeEventRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents([FromQuery] DateTime fromDate, [FromQuery] int max)
        {
            var events = await basketChangeEventRepository.GetBasketChangeEvents(fromDate, max);
            return Ok(mapper.Map<List<BasketChangeEventForPublication>>(events));
        }
    }
}
