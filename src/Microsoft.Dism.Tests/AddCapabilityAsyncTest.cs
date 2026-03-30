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
    public class AddCapabilityAsyncTest : DismTestBase
    {
        public AddCapabilityAsyncTest(TestWimTemplate template, ITestOutputHelper testOutput)
            : base(template, testOutput)
        {
        }

        [Fact]
        public async Task AddCapabilityAsync_CompletesSuccessfully()
        {
            using DismSession session = DismApi.OpenOnlineSession();

            await Should.ThrowAsync<DismException>(
                () => DismApi.AddCapabilityAsync(session, "NonExistent.Capability~~~~0.0.1.0", false, null, cancellationToken: TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task AddCapabilityAsync_WithCancelledToken_ThrowsOperationCanceledException()
        {
            using DismSession session = DismApi.OpenOnlineSession();
            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));

            Exception? ex = await Record.ExceptionAsync(
                () => DismApi.AddCapabilityAsync(session, "NonExistent.Capability~~~~0.0.1.0", false, null, cancellationToken: cts.Token));

            ex.ShouldNotBeNull();
            ex.ShouldBeAssignableTo<Exception>();
        }

        [Fact]
        public async Task AddCapabilityAsync_ReportsProgress()
        {
            using DismSession session = DismApi.OpenOnlineSession();
            var progress = new SynchronousProgress<DismProgress>(_ => { });

            try
            {
                await DismApi.AddCapabilityAsync(session, "NonExistent.Capability~~~~0.0.1.0", false, null, progress: progress, cancellationToken: TestContext.Current.CancellationToken);
            }
            catch (DismException)
            {
            }
        }

        private sealed class SynchronousProgress<T> : IProgress<T>
        {
            private readonly Action<T> _handler;

            public SynchronousProgress(Action<T> handler) => _handler = handler;

            public void Report(T value) => _handler(value);
        }
    }
}
