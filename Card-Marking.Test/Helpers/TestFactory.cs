using Card_Marking.Common.Models;
using Card_Marking.Functions.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Card_Marking.Test.Helpers
{
    internal class TestFactory
    {
        public static CardMarkingEntity GetCardMarkingEntity()
        {
            return new CardMarkingEntity
            {
                ETag = "*",
                PartitionKey = "CardMarking",
                RowKey = Guid.NewGuid().ToString(),
                DateMarking = DateTime.UtcNow,
                IdEmployee = 1,
                IsConsolidated = false,
                TypeMarking = 0

            };

        }

        public static DefaultHttpRequest CreateHttpRequest(Guid cardMarkingId, CardMarking cardMarkingRequest)
        {
            string request = JsonConvert.SerializeObject(cardMarkingRequest);
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(request),
                Path = $"/{cardMarkingId}"

            };

        }

        public static DefaultHttpRequest CreateHttpRequest(Guid cardMarkingId)
        {
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Path = $"/{cardMarkingId}"

            };

        }

        public static DefaultHttpRequest CreateHttpRequest(CardMarking cardMarkingRequest)
        {
            string request = JsonConvert.SerializeObject(cardMarkingRequest);
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(request)

            };

        }

        public static DefaultHttpRequest CreateHttpRequest(Time time)
        {
            string request = JsonConvert.SerializeObject(time);
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(request)

            };

        }

        public static DefaultHttpRequest CreateHttpRequest()
        {
            return new DefaultHttpRequest(new DefaultHttpContext());

        }

        public static CardMarking GetCardMarkingRequest()
        {
            return new CardMarking

            {
                DateMarking = DateTime.UtcNow,
                IdEmployee = 1,
                IsConsolidated = false,
                TypeMarking = 0
            };
        }


        public static Stream GenerateStreamFromString(string stringToConvert)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(stringToConvert);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static ILogger CreateLogger(LoggerTypes type = LoggerTypes.Null)
        {
            ILogger logger;
            if (type == LoggerTypes.List)
            {
                logger = new ListLogger();

            }
            else
            {
                logger = NullLoggerFactory.Instance.CreateLogger("Null logger");
            }
            return logger;
        }

        public static Time GetTimeRequest()
        {
            return new Time

            {
                IdEmployee = 1,
                DateWorked = DateTime.UtcNow,
                MinutesWorked = 500
            };
        }
    }
}
