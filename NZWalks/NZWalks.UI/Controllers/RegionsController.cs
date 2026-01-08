using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using NZWalks.UI.Models.DTO;
using System.Text.Json;
using System.Text;


namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> ShowAllRegion()
        {
            List<RegionDto> responseRegion = new List<RegionDto>();

            try
            {
                //Get All Regions from API
                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.GetAsync("https://localhost:7279/api/regions");

                httpResponseMessage.EnsureSuccessStatusCode();

                responseRegion.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());
            }
            catch (Exception)
            {

                throw;
            }

            return View(responseRegion);
        }

        public IActionResult AddRegion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRegion(AddRegionViewModel region)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                var client = httpClientFactory.CreateClient();

                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://localhost:7279/api/regions"),
                    Content = new StringContent(content: JsonSerializer.Serialize(region),
                                                encoding: System.Text.Encoding.UTF8,
                                                mediaType: "application/json")
                };



                var httpResponseMessage = await client.SendAsync(httpRequestMessage);
                httpResponseMessage.EnsureSuccessStatusCode();

                var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();

                if (response is not null)
                {
                    return RedirectToAction("ShowAllRegion", "Regions");
                }

                return View();

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditRegion(Guid id)
        {
            var client = httpClientFactory.CreateClient();
            var httpResponse = await client.GetFromJsonAsync<RegionDto>($"https://localhost:7279/api/regions/{id.ToString()}");

            if (httpResponse is not null)
            {
                return View(httpResponse);
            }
            else
                return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> EditRegion(RegionDto region)
        {
            if (!ModelState.IsValid)
            {
                return View(region);
            }

            try
            {
                var client = httpClientFactory.CreateClient();
                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"https://localhost:7279/api/regions/{region.Id}"),
                    Content = new StringContent(content: JsonSerializer.Serialize(region),
                                                encoding: System.Text.Encoding.UTF8,
                                                mediaType: "application/json")
                };



                var httpResponseMessage = await client.SendAsync(httpRequestMessage);
                httpResponseMessage.EnsureSuccessStatusCode();

                var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();

                if (response is not null)
                {
                    return RedirectToAction("EditRegion", "Regions");
                }

                return View();

            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
        public async Task<IActionResult> DeleteRegion(Guid id)
        {
            var client = httpClientFactory.CreateClient();
            var httpResponse = await client.GetFromJsonAsync<RegionDto>($"https://localhost:7279/api/regions/{id.ToString()}");

            if (httpResponse is not null)
            {
                return View(httpResponse);
            }
            else
                return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSelectedRegion(Guid id)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpResponseMessage = await client.DeleteAsync($"https://localhost:7279/api/regions/{id}");
                var response = httpResponseMessage.EnsureSuccessStatusCode();
 
                if (response is not null)
                {
                    return RedirectToAction("ShowAllRegion", "Regions");
                }
                else
                    return View();            
                
            }
            catch (Exception)
            {
                throw;
            }

            return View("EditRegion");
        }
    }
}
