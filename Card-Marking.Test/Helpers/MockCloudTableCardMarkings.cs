using Card_Marking.Functions.Entities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Card_Marking.Test.Helpers
{
    internal class MockCloudTableCardMarkings : CloudTable
    {

        public MockCloudTableCardMarkings(Uri tableAddress) : base(tableAddress)
        {
        }

        public MockCloudTableCardMarkings(Uri tableAbsoluteUri, StorageCredentials credentials) : base(tableAbsoluteUri, credentials)
        {
        }

        public MockCloudTableCardMarkings(StorageUri tableAddress, StorageCredentials credentials) : base(tableAddress, credentials)
        {
        }

        public override async Task<TableResult> ExecuteAsync(TableOperation operation)
        {
            return await Task.FromResult(new TableResult
            {
                HttpStatusCode = 200,
                Result = TestFactory.GetCardMarkingEntity()
            });
        }

        public override async Task<TableQuerySegment<CardMarkingEntity>> ExecuteQuerySegmentedAsync<CardMarkingEntity>(TableQuery<CardMarkingEntity> query, TableContinuationToken token)
        {
            ConstructorInfo constructor = typeof(TableQuerySegment<CardMarkingEntity>)
                   .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)[0];
            return await Task.FromResult(constructor.Invoke(new object[] { new List<CardMarkingEntity>() }) as TableQuerySegment<CardMarkingEntity>);
        }
    }
}
