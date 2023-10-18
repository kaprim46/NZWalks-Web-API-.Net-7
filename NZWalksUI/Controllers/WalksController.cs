using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NZWalksUI.Models;
using NZWalksUI.Models.DTO;

namespace NZWalksUI.Controllers
{
    public class WalksController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public WalksController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index()
        {
            List<WalksDTO> response = new();
            try
            {
                //Get All Wlaks from Web Api
                var client = _httpClientFactory.CreateClient();
                var httpResponseMessage = await client.GetAsync("https://localhost:7094/api/walks");
                httpResponseMessage.EnsureSuccessStatusCode();

                var stringResponse = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<WalksDTO>>();
                response.AddRange(stringResponse);
            }
            catch (Exception ex)
            {
                //Log the exception
            }
            return View(response);
        }

        public async Task<IActionResult> Create()
        {
            List<RegionDTO> responseRegions = new();
            List<DifficultyDTO> responseDifficulties = new();
            //Get All Regions from Web Api
            var client = _httpClientFactory.CreateClient();
            var httpResponseMessage = await client.GetAsync("https://localhost:7094/api/regions");
            httpResponseMessage.EnsureSuccessStatusCode();
            var stringRegionResponse = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDTO>>();
            responseRegions.AddRange(stringRegionResponse);

            httpResponseMessage = await client.GetAsync("https://localhost:7094/api/difficulty");
            httpResponseMessage.EnsureSuccessStatusCode();
            var stringWalksResponse = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<DifficultyDTO>>();
            responseDifficulties.AddRange(stringWalksResponse);

            AddWalksViewModel addWalks = new()
            {
                RegionsList = responseRegions.Select(q => new SelectListItem
                {
                    Text = q.Name,
                    Value = q.Id.ToString()
                }),

                DifficultiesList = responseDifficulties.Select(q => new SelectListItem
                {
                    Text = q.Name,
                    Value = q.Id.ToString()
                }),

                walksCreateDTO = new()
            };
            return View(addWalks);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddWalksViewModel addWalks)
        {
            var client = _httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7094/api/walks"),
                Content = new StringContent(JsonSerializer.Serialize(addWalks.walksCreateDTO), Encoding.UTF8, "application/json")
            };
            var httpResponseMessage = await client.SendAsync(httpRequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode();
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<WalksDTO>();
            if (response is not null)
            {
                return RedirectToAction(nameof(Index));
            }
            List<RegionDTO> responseRegions = new();
            List<DifficultyDTO> responseDifficulties = new();
            //Get All Regions from Web Api
            var clients = _httpClientFactory.CreateClient();
            var httpResponseMessageDD = await client.GetAsync("https://localhost:7094/api/regions");
            httpResponseMessageDD.EnsureSuccessStatusCode();
            var stringRegionResponse = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDTO>>();
            responseRegions.AddRange(stringRegionResponse);

            httpResponseMessageDD = await clients.GetAsync("https://localhost:7094/api/difficulty");
            httpResponseMessageDD.EnsureSuccessStatusCode();
            var stringResponse = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<DifficultyDTO>>();
            responseDifficulties.AddRange(stringResponse);

            AddWalksViewModel addWalksVM = new()
            {
                RegionsList = responseRegions.Select(q => new SelectListItem
                {
                    Text = q.Name,
                    Value = q.Id.ToString()
                }),

                DifficultiesList = responseDifficulties.Select(q => new SelectListItem
                {
                    Text = q.Name,
                    Value = q.Id.ToString()
                }),

                walksCreateDTO = addWalks.walksCreateDTO
            };
            return View(addWalksVM);
        }

        public async Task<IActionResult> Update(Guid id)
        {
            List<RegionDTO> responseRegions = new();
            List<DifficultyDTO> responseDifficulties = new();
            //Get All Regions from Web Api
            var client = _httpClientFactory.CreateClient();
            var httpResponseMessage = await client.GetAsync("https://localhost:7094/api/regions");
            httpResponseMessage.EnsureSuccessStatusCode();
            var stringRegionResponse = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDTO>>();
            responseRegions.AddRange(stringRegionResponse);

            httpResponseMessage = await client.GetAsync("https://localhost:7094/api/difficulty");
            httpResponseMessage.EnsureSuccessStatusCode();
            var stringWalksResponse = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<DifficultyDTO>>();
            responseDifficulties.AddRange(stringWalksResponse);

            httpResponseMessage = await client.GetAsync($"https://localhost:7094/api/walks/{id.ToString()}");
            httpResponseMessage.EnsureSuccessStatusCode();
            var stringWalkResponse = await httpResponseMessage.Content.ReadFromJsonAsync<WalksDTO>();
            

            UpdateWalksViewModel updateWalks = new()
            {
                RegionsList = responseRegions.Select(q => new SelectListItem
                {
                    Text = q.Name,
                    Value = q.Id.ToString()
                }),

                DifficultiesList = responseDifficulties.Select(q => new SelectListItem
                {
                    Text = q.Name,
                    Value = q.Id.ToString()
                }),

                WalksDTO = stringWalkResponse,
                walksUpdateDTO = new()
                {
                    Name = stringWalkResponse.Name,
                    Description = stringWalkResponse.Description,
                    LengthInKm = stringWalkResponse.LengthInKm,
                    WalkImage = stringWalkResponse.WalkImage,
                    DifficultyId = stringWalkResponse.Difficulty.Id,
                    RegionId = stringWalkResponse.Region.Id
                }
            };
            return View(updateWalks);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateWalksViewModel updateWalks)
        {
            var client = _httpClientFactory.CreateClient();
            
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7094/api/walks/{updateWalks.WalksDTO.Id.ToString()}"),
                Content = new StringContent(JsonSerializer.Serialize(updateWalks.walksUpdateDTO), Encoding.UTF8, "application/json")
            };
            var httpResponseMessage = await client.SendAsync(httpRequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode();
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<WalksDTO>();
            if (response is not null)
            {
                return RedirectToAction(nameof(Index));
            }
            List<RegionDTO> responseRegions = new();
            List<DifficultyDTO> responseDifficulties = new();
            //Get All Regions from Web Api
            var clients = _httpClientFactory.CreateClient();
            var httpResponseMessageDD = await client.GetAsync("https://localhost:7094/api/regions");
            httpResponseMessageDD.EnsureSuccessStatusCode();
            var stringRegionResponse = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDTO>>();
            responseRegions.AddRange(stringRegionResponse);

            httpResponseMessageDD = await clients.GetAsync("https://localhost:7094/api/difficulty");
            httpResponseMessageDD.EnsureSuccessStatusCode();
            var stringResponse = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<DifficultyDTO>>();
            responseDifficulties.AddRange(stringResponse);

            UpdateWalksViewModel updateWalksVM = new()
            {
                RegionsList = responseRegions.Select(q => new SelectListItem
                {
                    Text = q.Name,
                    Value = q.Id.ToString()
                }),

                DifficultiesList = responseDifficulties.Select(q => new SelectListItem
                {
                    Text = q.Name,
                    Value = q.Id.ToString()
                }),

                WalksDTO = updateWalks.WalksDTO,
                walksUpdateDTO = updateWalks.walksUpdateDTO
            };
            return View(updateWalksVM);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var client = _httpClientFactory.CreateClient();
            var httpResponseMessage = await client.GetAsync($"https://localhost:7094/api/walks/{id.ToString()}");
            httpResponseMessage.EnsureSuccessStatusCode();

            var stringResponse = await httpResponseMessage.Content.ReadFromJsonAsync<WalksDTO>();
            if (stringResponse is not null)
            {
                return View(stringResponse);
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(WalksDTO removeWalks)
        {
            var client = _httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"https://localhost:7094/api/walks/{removeWalks.Id.ToString()}"),
                Content = new StringContent(JsonSerializer.Serialize(removeWalks), Encoding.UTF8, "application/json")
            };
            var httpResponseMesaage = await client.SendAsync(httpRequestMessage);
            httpResponseMesaage.EnsureSuccessStatusCode();

            var response = await httpResponseMesaage.Content.ReadFromJsonAsync<WalksDTO>();
            if (response is not null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(removeWalks);
        }
    }
}