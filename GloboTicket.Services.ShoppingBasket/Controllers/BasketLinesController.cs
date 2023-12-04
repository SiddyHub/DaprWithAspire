using AutoMapper;
using Dapr;
using GloboTicket.Services.ShoppingBasket.Entities;
using GloboTicket.Services.ShoppingBasket.Models;
using GloboTicket.Services.ShoppingBasket.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GloboTicket.Services.ShoppingBasket.Controllers
{
    [Route("api/baskets/{userId}/basketlines")]
    [ApiController]
    public class BasketLinesController : ControllerBase
    {               
        private readonly IBasketChangeEventRepository basketChangeEventRepository;

        public BasketLinesController(IBasketChangeEventRepository basketChangeEventRepository)
        {                                    
            this.basketChangeEventRepository = basketChangeEventRepository;            
        }     

        [HttpPost]
        public async Task<IActionResult> Post(Guid userId, [FromBody] BasketLineForCreation basketLineForCreation)
        {                        
            BasketChangeEvent basketChangeEvent = new BasketChangeEvent
            {
                BasketChangeType = BasketChangeTypeEnum.Add,
                EventId = basketLineForCreation.EventId,
                InsertedAt = DateTime.Now,
                UserId = userId
            };
            await basketChangeEventRepository.AddBasketEvent(basketChangeEvent);
            return NoContent();          
        }      

        [HttpDelete("{eventId}")]
        public async Task<IActionResult> Delete(Guid userId, Guid eventId)
        {            
            //publish removal event
            BasketChangeEvent basketChangeEvent = new BasketChangeEvent
            {
                BasketChangeType = BasketChangeTypeEnum.Remove,
                EventId = eventId,
                InsertedAt = DateTime.Now,
                UserId = userId
            };
            await basketChangeEventRepository.AddBasketEvent(basketChangeEvent);

            return NoContent();
        }        
    }
}
