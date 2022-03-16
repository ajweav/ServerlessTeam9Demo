using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BFYOC.Function
{
    public static class GetProductID
    {
        [FunctionName("GetProductID")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string productID = req.Query["productID"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            productID = productID ?? data?.productID;

            string responseMessage = string.IsNullOrEmpty(productID)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {productID}. This HTTP triggered function executed successfully.";

            bool correctProductID = string.Equals(productID, "75542e38-563f-436f-adeb-f426f1dabb5c");
            if (correctProductID)
            {
                responseMessage = $"The product name for your product id {productID} is Starfruit Explosion and the description is This starfruit ice cream is out of this world";
            }
                

            return new OkObjectResult(responseMessage);
        }
    }
}
