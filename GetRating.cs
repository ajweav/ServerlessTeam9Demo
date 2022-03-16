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
    public static class GetRating
    {
        [FunctionName("GetRating")]
        public static IActionResult Run(
       [HttpTrigger(AuthorizationLevel.Function, "get", Route = "rating/{ratingId}")] HttpRequest req,
        [CosmosDB("%databaseName%", "%collectionName%", ConnectionStringSetting = "_ConnectionStringSetting",
        SqlQuery = "Select * from ratings r where r.id = {ratingId}")]IEnumerable<RatingModel> rating,
         ILogger log)
        {
            if (rating == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(rating);
        }
    }
}
