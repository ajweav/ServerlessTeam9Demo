using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using ServerlessTeam9FunctionApp.Models;

namespace ServerlessTeam9FunctionApp
{
    public static class GetRatings
    {
        [FunctionName("GetRatings")]
        public static IActionResult Run(
       [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ratingsByUser/{userId}")] HttpRequest req,
        [CosmosDB("%databaseName%", "%collectionName%", ConnectionStringSetting = "_ConnectionStringSetting",
        SqlQuery = "Select * from ratings r where r.userId = {userId}")] IEnumerable<RatingModel> ratings,
         ILogger log)
        {
            if (ratings == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(ratings);
        }
    }
}
