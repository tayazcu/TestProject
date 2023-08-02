using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Cryptography.Xml;
using System;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.AspNetCore.Authorization;

namespace Project.WebApi.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class WebApi2ConfigController : Controller
    {
        string WebApi2Url = string.Empty;
        private readonly IConfiguration _configuration;
        private readonly ILogger<WebApi2ConfigController> _logger;

        public WebApi2ConfigController(IConfiguration configuration, ILogger<WebApi2ConfigController> logger)
        {
            _configuration = configuration;
            _logger = logger;
            WebApi2Url = _configuration["SiteSetting:ServiceUrls:WebApi2"];
        }

        [HttpGet(template: "GetTestData")]
        public IActionResult Get()
        {
            string result = Call();
            _logger.LogError($"-------------------------- WebApi2ConfigController Get result {result}");
            return new JsonResult(result);
        }

        private string Call()
        {
            string result = "Not Result";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(WebApi2Url);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync("/api/Test/Get").Result;
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var dataObjects = response.Content.ReadAsAsync<string>().Result;
                result = dataObjects;
                _logger.LogError($"-------------------------- Call Success {dataObjects}");
            }
            else
            {
                _logger.LogError("-------------------------- Call Faild");
                _logger.LogError("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            client.Dispose();

            return result;
        }
    }
}
