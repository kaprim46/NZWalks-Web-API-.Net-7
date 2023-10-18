using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NZWalksUI.Models;
using NZWalksUI.Models.DTO;

namespace NZWalksUI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index()
        {
            List<RegionDTO> response = new();
            try
            {
                //Get All Regions from Web Api
                var client = _httpClientFactory.CreateClient();
                var httpResponseMessage = await client.GetAsync("https://localhost:7094/api/regions");
                httpResponseMessage.EnsureSuccessStatusCode();

                var stringResponse = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDTO>>();
                response.AddRange(stringResponse);
            }
            catch (Exception ex)
            {
                //Log the exception
            }
            return View(response);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddRegionViewModel addRegion)
        {
            var client = _httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7094/api/regions"),
                Content = new StringContent(JsonSerializer.Serialize(addRegion), Encoding.UTF8, "application/json")
            };
            var httpResponseMesaage = await client.SendAsync(httpRequestMessage);
            httpResponseMesaage.EnsureSuccessStatusCode();

            var response = await httpResponseMesaage.Content.ReadFromJsonAsync<RegionDTO>();
            if (response is not null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var client = _httpClientFactory.CreateClient();
            var httpResponseMessage = await client.GetAsync($"https://localhost:7094/api/regions/{id.ToString()}");
            httpResponseMessage.EnsureSuccessStatusCode();

            var stringResponse = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDTO>();
            if (stringResponse is not null)
            {
                return View(stringResponse);
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Update(RegionDTO EditRegion)
        {
            var client = _httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7094/api/regions/{EditRegion.Id.ToString()}"),
                Content = new StringContent(JsonSerializer.Serialize(EditRegion), Encoding.UTF8, "application/json")
            };
            var httpResponseMesaage = await client.SendAsync(httpRequestMessage);
            httpResponseMesaage.EnsureSuccessStatusCode();

            var response = await httpResponseMesaage.Content.ReadFromJsonAsync<RegionDTO>();
            if (response is not null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(EditRegion);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var client = _httpClientFactory.CreateClient();
            var httpResponseMessage = await client.GetAsync($"https://localhost:7094/api/regions/{id.ToString()}");
            httpResponseMessage.EnsureSuccessStatusCode();

            var stringResponse = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDTO>();
            if (stringResponse is not null)
            {
                return View(stringResponse);
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(RegionDTO removeRegion)
        {
            var client = _httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"https://localhost:7094/api/regions/{removeRegion.Id.ToString()}"),
                Content = new StringContent(JsonSerializer.Serialize(removeRegion), Encoding.UTF8, "application/json")
            };
            var httpResponseMesaage = await client.SendAsync(httpRequestMessage);
            httpResponseMesaage.EnsureSuccessStatusCode();

            var response = await httpResponseMesaage.Content.ReadFromJsonAsync<RegionDTO>();
            if (response is not null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(removeRegion);
        }
    }
}