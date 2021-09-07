using Card_Marking.Functions.Functions;
using Card_Marking.Test.Helpers;
using System;
using Xunit;

namespace Card_Marking.Test.Test
{
    public class ScheduledFunctionsTest
    {
        [Fact]
        public void ScheduledFunction_Should_Log_Message()
        {
            //Arrange
            MockCloudTableCardMarkings mockCardMarkings = new MockCloudTableCardMarkings(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            MockCloudTableTime mockTime = new MockCloudTableTime(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            ListLogger logger = (ListLogger)TestFactory.CreateLogger(LoggerTypes.List);

            //Act
            _ = ScheduledFunctions.Run(null, logger, mockCardMarkings, mockTime);
            string message = logger.Logs[0];

            //Asert
            Assert.Contains("Recieved a new consolidation process, starts at:", message);
        }
    }
}
