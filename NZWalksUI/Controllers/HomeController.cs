using System.Diagnostics;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalksUI.Models;
using NZWalksUI.Models.DTO;

namespace NZWalksUI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
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

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

