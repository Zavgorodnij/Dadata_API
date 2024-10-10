using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace Dadata_API.Controllers
{
    [EnableCors]
    public class APIController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<APIController> _logger;
        public APIController(IHttpClientFactory httpClientFactory, IMapper mapper, ILogger<APIController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Get(string adress)
        {
            _logger.LogWarning($"Получен адрес {adress}");
            var requestBody = new[] { adress };
            var jsonContent = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json");
            var client = _httpClientFactory.CreateClient("DadataClient");
            try
            {
                var response = await client.PostAsync("", jsonContent);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                var Adress_Standartization = JsonConvert.DeserializeObject<List<Models.AdressModel>>(result);
                var DTO_Obj = _mapper.Map<List<Models.AdressModelDTO>>(Adress_Standartization);
                client.Dispose();
                _logger.LogWarning("Данные передаются клиенту");
                return Json(DTO_Obj);

            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Во время выполнения запроса произошла ошибка {ex.ToString()}");
                client.Dispose();
                return Json($"API Error {ex.Message}");
            }

        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
