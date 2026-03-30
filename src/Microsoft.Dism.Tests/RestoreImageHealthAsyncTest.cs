// Copyright (c). All rights reserved.
//
// Licensed under the MIT license.

using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.Dism.Tests
{
    public class RestoreImageHealthAsyncTest : DismTestBase
    {
        public RestoreImageHealthAsyncTest(TestWimTemplate template, ITestOutputHelper testOutput)
            : base(template, testOutput)
        {
        }

        [Fact]
        public async Task RestoreImageHealthAsync_CompletesSuccessfully()
        {
            using DismSession session = DismApi.OpenOnlineSession();

            await DismApi.RestoreImageHealthAsync(session, limitAccess: true, cancellationToken: TestContext.Current.CancellationToken);
        }

        [Fact]
        public async Task RestoreImageHealthAsync_WithCancelledToken_ThrowsOperationCanceledException()
        {
            using DismSession session = DismApi.OpenOnlineSession();
            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));

            await Should.ThrowAsync<OperationCanceledException>(
                () => DismApi.RestoreImageHealthAsync(session, limitAccess: true, cancellationToken: cts.Token));
        }

        [Fact]
        public async Task RestoreImageHealthAsync_ReportsProgress()
        {
            using DismSession session = DismApi.OpenOnlineSession();
            bool progressReported = false;
            var progress = new SynchronousProgress<DismProgress>(_ => progressReported = true);
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));

            try
            {
                await DismApi.RestoreImageHealthAsync(session, limitAccess: true, progress: progress, cancellationToken: cts.Token);
            }
            catch (OperationCanceledException)
            {
            }

            progressReported.ShouldBeTrue();
        }
    }
}
