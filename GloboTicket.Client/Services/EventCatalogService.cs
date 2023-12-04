using GloboTicket.Web.Extensions;
using GloboTicket.Web.Models.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GloboTicket.Web.Services
{
    public class EventCatalogService : IEventCatalogService
    {        
        private readonly HttpClient _httpClient;
        //private readonly IConfiguration _config;        
        //private readonly ITokenAcquisition _tokenAcquisition;        

        public EventCatalogService(HttpClient httpClient)//, IConfiguration config) //ITokenAcquisition tokenAcquisition, 
        {            
            //_tokenAcquisition = tokenAcquisition;
            _httpClient = httpClient;
            //_config = config;                      
        }

        public async Task<IEnumerable<Event>> GetAll()
        {
            //await PrepareAuthenticatedClient();
            var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/api/events");            
            return await response.ReadContentAs<List<Event>>();
        }

        public async Task<IEnumerable<Event>> GetByCategoryId(Guid categoryid)
        {
            //await PrepareAuthenticatedClient();            
            var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}api/events/?categoryId={categoryid}");
            return await response.ReadContentAs<List<Event>>();
        }

        public async Task<Event> GetEvent(Guid id)
        {
            //await PrepareAuthenticatedClient();            
            var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}api/events/{id}");
            return await response.ReadContentAs<Event>();
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            //await PrepareAuthenticatedClient();
            var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}api/categories");            
            return await response.ReadContentAs<List<Category>>();
        }

        public async Task<PriceUpdate> UpdatePrice(PriceUpdate priceUpdate)
        {
            //await PrepareAuthenticatedClient();            
            var response = await _httpClient.PostAsJson($"{_httpClient.BaseAddress}api/events/eventpriceupdate", priceUpdate);
            return await response.ReadContentAs<PriceUpdate>();
        }

        // private async Task PrepareAuthenticatedClient()
        // {
        //     var scopes = _config.GetSection("EventCatalog:EventCatalogScopes").Get<string>().Split(" ", System.StringSplitOptions.RemoveEmptyEntries);
        //     var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);
        //     _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        //     _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        // }
    }
}
