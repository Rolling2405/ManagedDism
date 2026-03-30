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
    public class CheckImageHealthAsyncTest : DismTestBase
    {
        public CheckImageHealthAsyncTest(TestWimTemplate template, ITestOutputHelper testOutput)
            : base(template, testOutput)
        {
        }

        [Fact]
        public async Task CheckImageHealthAsync_CompletesSuccessfully()
        {
            using DismSession session = DismApi.OpenOnlineSession();

            DismImageHealthState result = await DismApi.CheckImageHealthAsync(session, scanImage: false, cancellationToken: TestContext.Current.CancellationToken);

            result.ShouldBe(DismImageHealthState.Healthy);
        }

        [Fact]
        public async Task CheckImageHealthAsync_WithCancelledToken_ThrowsOperationCanceledException()
        {
            using DismSession session = DismApi.OpenOnlineSession();
            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));

            await Should.ThrowAsync<OperationCanceledException>(
                () => DismApi.CheckImageHealthAsync(session, scanImage: true, cancellationToken: cts.Token));
        }

        [Fact]
        public async Task CheckImageHealthAsync_ReportsProgress()
        {
            using DismSession session = DismApi.OpenOnlineSession();
            bool progressReported = false;
            var progress = new SynchronousProgress<DismProgress>(_ => progressReported = true);
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));

            try
            {
                await DismApi.CheckImageHealthAsync(session, scanImage: true, progress: progress, cancellationToken: cts.Token);
            }
            catch (OperationCanceledException)
            {
            }

            progressReported.ShouldBeTrue();
        }

        private sealed class SynchronousProgress<T> : IProgress<T>
        {
            private readonly Action<T> _handler;

            public SynchronousProgress(Action<T> handler) => _handler = handler;

            public void Report(T value) => _handler(value);
        }
    }
}
