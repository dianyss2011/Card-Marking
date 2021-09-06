using Card_Marking.Functions.Entities;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Card_Marking.Functions.Functions
{
    public static class ScheduledFunctions
    {
        [FunctionName("ScheduledFunctions")]
        public static async Task Run([TimerTrigger("0 */2 * * * *")] TimerInfo myTimer, ILogger log,
            [Table("cardmarking", Connection = "AzureWebJobsStorage")] CloudTable cardMarkingTable,
            [Table("times", Connection = "AzureWebJobsStorage")] CloudTable timesTable)
        {
            log.LogInformation($"Recieved a new consolidation process, starts at: {DateTime.Now}");

            string filter = TableQuery.GenerateFilterConditionForBool("IsConsolidated", QueryComparisons.Equal, false);
            TableQuery<CardMarkingEntity> query = new TableQuery<CardMarkingEntity>().Where(filter);
            TableQuerySegment<CardMarkingEntity> cardMarking = await cardMarkingTable.ExecuteQuerySegmentedAsync(query, null);


            int newTimes = 0;
            int updatedTimes = 0;
            if (cardMarking.Results != null)
            {
                List<CardMarkingEntity> orderedByIdEmployee = cardMarking.Results.OrderBy(x => x.IdEmployee).ToList();
                CardMarkingEntity[] cardMarkingEntities = orderedByIdEmployee.OrderBy(x => x.DateMarking).ToArray();

                CardMarkingEntity firstCardMarking = null;
                for (int i = 0; i < cardMarkingEntities.Length; i++)
                {

                    if (cardMarkingEntities[i].TypeMarking == 0)
                    {
                        firstCardMarking = cardMarkingEntities[i];
                    }
                    else
                    {
                        if (firstCardMarking != null && firstCardMarking.IdEmployee == cardMarkingEntities[i].IdEmployee)
                        {
                            cardMarkingEntities[i].IsConsolidated = true;
                            firstCardMarking.IsConsolidated = true;

                            await cardMarkingTable.ExecuteAsync(TableOperation.Replace(cardMarkingEntities[i]));
                            await cardMarkingTable.ExecuteAsync(TableOperation.Replace(firstCardMarking));

                            double minutesWorkedDecimal = (cardMarkingEntities[i].DateMarking - firstCardMarking.DateMarking).TotalMinutes;

                            int minutesWorked = int.Parse(minutesWorkedDecimal.ToString());

                            DateTime dateMarking = cardMarkingEntities[i].DateMarking;
                            DateTime dateWorked = new DateTime(dateMarking.Year, dateMarking.Month, dateMarking.Day);

                            string filterTime = TableQuery.GenerateFilterConditionForDate("DateWorked", QueryComparisons.Equal, dateWorked.ToUniversalTime().AddHours(-5));
                            TableQuery<TimeEntity> queryTime = new TableQuery<TimeEntity>().Where(filterTime);
                            TableQuerySegment<TimeEntity> timeEntities = await timesTable.ExecuteQuerySegmentedAsync(queryTime, null);
                            List<TimeEntity> times = timeEntities.Results;
                            TimeEntity firstTimeEntity = null;

                            foreach (TimeEntity time in times)
                            {
                                if (time.IdEmployee == cardMarkingEntities[i].IdEmployee)
                                {
                                    firstTimeEntity = time;
                                }
                            }

                            if (firstTimeEntity != null)
                            {
                                firstTimeEntity.MinutesWorked = firstTimeEntity.MinutesWorked + minutesWorked;
                                await timesTable.ExecuteAsync(TableOperation.Replace(firstTimeEntity));
                                updatedTimes++;
                            }
                            else
                            {
                                TimeEntity timeEntity = new TimeEntity
                                {
                                    ETag = "*",
                                    PartitionKey = "Time",
                                    RowKey = Guid.NewGuid().ToString(),
                                    DateWorked = dateWorked.ToUniversalTime().AddHours(-5),
                                    IdEmployee = cardMarkingEntities[i].IdEmployee,
                                    MinutesWorked = minutesWorked
                                };
                                await timesTable.ExecuteAsync(TableOperation.Insert(timeEntity));
                                newTimes++;
                            }

                            firstCardMarking = null;
                        }
                    }
                }
            }

            log.LogInformation($"Consolidation summary, Records added: {newTimes}, records updated: {updatedTimes} at: {DateTime.Now}");
        }
    }
}
