using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using ServerlessTeam9FunctionApp.Models;

namespace ServerlessTeam9FunctionApp
{
    public static class CreateRating
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        [FunctionName("CreateRating")]
        public static async Task<IActionResult> Run(

            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB(databaseName: "%databaseName%", collectionName: "%collectionName%",
    ConnectionStringSetting = @"_ConnectionStringSetting"
    )]IAsyncCollector<RatingModel> documentsOut,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string _userid = null;
            string _productid = null; 
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody); 
            _userid = _userid ?? data?.userId;
            _productid = _productid ?? data?.productId; 
            //var result = await _httpClient.GetAsync($"");
            var result = await _httpClient.GetAsync($"https://serverlessohuser.trafficmanager.net/api/GetUser/?userid={_userid}");
            //int r = int.Parse(data?.rating);
            RatingModel iceCreamRating = null;
            if (result.IsSuccessStatusCode && (data?.rating >= 0 && data?.rating <= 5))
            {

                var resultContentProduct = await _httpClient.GetAsync($"https://serverlessohproduct.trafficmanager.net/api/GetProduct/?productId={_productid}");
                if (resultContentProduct.IsSuccessStatusCode)
                {
                    iceCreamRating = new RatingModel
                    {
                        userId = _userid,
                        productId = _productid,
                        locationName = data?.locationName,
                        rating = data?.rating,
                        userNotes = data?.userNotes,
                        timestamp = DateTime.Now
                    };
                   await documentsOut.AddAsync(iceCreamRating);
                }

            }
            return new OkObjectResult(iceCreamRating);
        }
    }
}
