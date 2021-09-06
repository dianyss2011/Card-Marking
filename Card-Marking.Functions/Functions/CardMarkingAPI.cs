using Card_Marking.Common.Models;
using Card_Marking.Common.Responses;
using Card_Marking.Functions.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Card_Marking.Functions.Functions
{
    public static class CardMarkingAPI
    {

        [FunctionName(nameof(CreatedCardMarking))]
        public static async Task<IActionResult> CreatedCardMarking(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "CardMarking")] HttpRequest req,
            [Table("cardmarking", Connection = "AzureWebJobsStorage")] CloudTable cardMarkingTable,
            ILogger log)
        {
            log.LogInformation("Received a new CardMarking.");


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            CardMarking cardMarking = JsonConvert.DeserializeObject<CardMarking>(requestBody);
            if (cardMarking.IdEmployee==0)
            {
                return new BadRequestObjectResult(new Response

                {
                    IsSuccess = false,
                    Message = "The request must have a IdEmployee"
                });
            }

            if (cardMarking.DateMarking == DateTime.MinValue)
            {
                return new BadRequestObjectResult(new Response

                {
                    IsSuccess = false,
                    Message = "The request must have a DateMarking"
                });
            }

            if (cardMarking.TypeMarking != 0 && cardMarking.TypeMarking != 1)
            {
                return new BadRequestObjectResult(new Response

                {
                    IsSuccess = false,
                    Message = "The request must have a TypeMarking"
                });
            }


            CardMarkingEntity cardMarkingEntity = new CardMarkingEntity
            {
                
                ETag = "*",
                IsConsolidated = false,
                PartitionKey = "CardMarking",
                RowKey = Guid.NewGuid().ToString(),
                DateMarking = cardMarking.DateMarking.ToUniversalTime(),
                IdEmployee = cardMarking.IdEmployee,
                TypeMarking =cardMarking.TypeMarking,

            };

            TableOperation addOperation = TableOperation.Insert(cardMarkingEntity);
            await cardMarkingTable.ExecuteAsync(addOperation);

            string message = "New Card Marking stored in table";
            log.LogInformation(message);


            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = cardMarkingEntity
            });

        }

        [FunctionName(nameof(GetAllCardMarking))]
        public static async Task<IActionResult> GetAllCardMarking(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "CardMarking")] HttpRequest req,
           [Table("cardmarking", Connection = "AzureWebJobsStorage")] CloudTable cardMarkingTable,
           ILogger log)

        {
            log.LogInformation("Get all Card Marking received.");

            TableQuery<CardMarkingEntity> query = new TableQuery<CardMarkingEntity>();
            TableQuerySegment<CardMarkingEntity> cardMarkings = await cardMarkingTable.ExecuteQuerySegmentedAsync(query, null);

            string message = "Retrieved all Card Markings. ";

            log.LogInformation(message);



            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = cardMarkings
            });

        }

    }
}
