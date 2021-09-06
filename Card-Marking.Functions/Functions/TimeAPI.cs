using Card_Marking.Common.Responses;
using Card_Marking.Functions.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Card_Marking.Functions.Functions
{
    public static class TimeAPI
    {
        [FunctionName("GetTimesByDate")]
        public static async Task<IActionResult> GetTimesByDate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "times/{date}")] HttpRequest req,
            [Table("times", Connection = "AzureWebJobsStorage")] CloudTable timesTable,
            DateTime date,
            ILogger log)
        {
            log.LogInformation($"Received times of the day: {date}");

            string filter = TableQuery.GenerateFilterConditionForDate("DateWorked", QueryComparisons.Equal, date.ToUniversalTime().AddHours(-5));
            TableQuery<TimeEntity> query = new TableQuery<TimeEntity>().Where(filter);
            TableQuerySegment<TimeEntity> timeEntities = await timesTable.ExecuteQuerySegmentedAsync(query, null);

            List<TimeEntity> consolidatedEntities = timeEntities.Results;

            string message = "Retrieved times of the day.";
            log.LogInformation(message);
            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = consolidatedEntities
            });
        }
    }
}
