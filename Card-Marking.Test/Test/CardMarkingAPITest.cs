using Card_Marking.Common.Models;
using Card_Marking.Functions.Entities;
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
    public class CardMarkingAPITest
    {

        private readonly ILogger logger = TestFactory.CreateLogger();

        [Fact]
        public async void CreateCardMarking_Should_Return_200()
        {
            //Arrenge (Preparamos la prueba unitaria)

            MockCloudTableCardMarkings mockCardMarkings = new MockCloudTableCardMarkings(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            CardMarking cardMarkingRequest = TestFactory.GetCardMarkingRequest();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(cardMarkingRequest);

            //Act (Cuando ejecutamos)

            IActionResult response = await CardMarkingAPI.CreatedCardMarking(request, mockCardMarkings, logger);

            //Assert (Verificamos si la prueba unitaria da el resultado correcto)

            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async void UpdateCardMarking_Should_Return_200()
        {
            //Arrenge (Preparamos la prueba unitaria)

            MockCloudTableCardMarkings mockCardMarkings = new MockCloudTableCardMarkings(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            CardMarking cardMarkingRequest = TestFactory.GetCardMarkingRequest();
            Guid cardMarkingId = Guid.NewGuid();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(cardMarkingId, cardMarkingRequest);

            //Act (Cuando ejecutamos)

            IActionResult response = await CardMarkingAPI.UpdateCardMarking(request, mockCardMarkings, cardMarkingId.ToString(), logger);

            //Assert (Verificamos si la prueba unitaria da el resultado correcto
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async void GetAllCardMarking_Should_Return_200()
        {
            //Arrenge (Preparamos la prueba unitaria)

            MockCloudTableCardMarkings mockCardMarking = new MockCloudTableCardMarkings(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            DefaultHttpRequest request = TestFactory.CreateHttpRequest();

            //Act (Cuando ejecutamos)
            IActionResult response = await CardMarkingAPI.GetAllCardMarking(request, mockCardMarking, logger);

            //Assert (Verificamos si la prueba unitaria da el resultado correcto
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public void GetCardMarkingById_Should_Return_200()
        {

            //Arrenge (Preparamos la prueba unitaria)

            CardMarkingEntity mockCardMarkingEntity = TestFactory.GetCardMarkingEntity();
            Guid cardMakingId = Guid.NewGuid();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(cardMakingId);

            //Act (Cuando ejecutamos)

            IActionResult response = CardMarkingAPI.GetCardMarkingById(request, mockCardMarkingEntity, cardMakingId.ToString(), logger);

            //Assert (Verificamos si la prueba unitaria da el resultado correcto
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async void DeleteCardMarking_Should_Return_200()
        {
            //Arrenge (Preparamos la prueba unitaria)

            MockCloudTableCardMarkings mockCardMarking = new MockCloudTableCardMarkings(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            CardMarkingEntity mockCardMarkingEntity = TestFactory.GetCardMarkingEntity();
            Guid cardMakingId = Guid.NewGuid();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(cardMakingId);

            //Act (Cuando ejecutamos)

            IActionResult response = await CardMarkingAPI.DeleteCardMarking(request, mockCardMarkingEntity, mockCardMarking, cardMakingId.ToString(), logger);

            //Assert (Verificamos si la prueba unitaria da el resultado correcto

            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

    }
}
