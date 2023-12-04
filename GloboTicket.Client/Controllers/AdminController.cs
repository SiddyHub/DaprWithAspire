using GloboTicket.Web.Extensions;
using GloboTicket.Web.Models;
using GloboTicket.Web.Models.Api;
using GloboTicket.Web.Models.View;
using GloboTicket.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GloboTicket.Web.Controllers
{
    public class AdminController: Controller
    {
        private readonly IEventCatalogService eventCatalogService;
        private readonly Settings settings;
        private readonly IShoppingBasketService basketService;

        public AdminController(IEventCatalogService eventCatalogService, Settings settings, IShoppingBasketService basketService)
        {
            this.eventCatalogService = eventCatalogService;
            this.settings = settings;
            this.basketService = basketService;
        }

        public async Task<IActionResult> Index()
        {
            var allEvents = await eventCatalogService.GetAll();
            
            return View(allEvents);
        }

        public async Task<IActionResult> Details(Guid eventId)
        {
            var selectedEvent = (await eventCatalogService.GetAll()).Where(x => x.EventId == eventId).FirstOrDefault();

            return View(selectedEvent);
        }

        [HttpPost]
        public async Task<IActionResult> Details(Event eventPriceUpdateViewModel)
        {

            PriceUpdate priceUpdate = new PriceUpdate() { EventId = eventPriceUpdateViewModel.EventId, Price = eventPriceUpdateViewModel.Price };
            await eventCatalogService.UpdatePrice(priceUpdate);

            var basketId = Request.Cookies.GetCurrentBasketId(settings);
            await basketService.UpdateLinePrices(basketId, priceUpdate);                                 

            return RedirectToAction("Index");
        }
    }
}
