using Card_Marking.Common.Models;
using Card_Marking.Functions.Functions;
using Card_Marking.Test.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Xunit;

namespace Card_Marking.Test.Test
{
    public class TimeAPITest
    {
        private readonly ILogger logger = TestFactory.CreateLogger();

        [Fact]
        public async void CreatedTime_Should_Return_200()
        {

            //Arrenge (Preparamos la prueba unitaria)

            MockCloudTableTime mockTime = new MockCloudTableTime(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            Time time = TestFactory.GetTimeRequest();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(time);

            //Act (Cuando ejecutamos)

            IActionResult response = await TimeAPI.GetTimesByDate(request, mockTime, DateTime.UtcNow, logger);

            //Assert (Verificamos si la prueba unitaria da el resultado correcto
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

    }
}
