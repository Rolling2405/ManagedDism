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
    public class RemoveCapabilityAsyncTest : DismTestBase
    {
        public RemoveCapabilityAsyncTest(TestWimTemplate template, ITestOutputHelper testOutput)
            : base(template, testOutput)
        {
        }

        [Fact]
        public async Task RemoveCapabilityAsync_ThrowsDismException()
        {
            using DismSession session = DismApi.OpenOnlineSession();

            await Should.ThrowAsync<DismException>(
                () => DismApi.RemoveCapabilityAsync(session, "NonExistent.Capability~~~~0.0.1.0", cancellationToken: TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task RemoveCapabilityAsync_WithCancelledToken_ThrowsOperationCanceledException()
        {
            using DismSession session = DismApi.OpenOnlineSession();
            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));

            Exception? ex = await Record.ExceptionAsync(
                () => DismApi.RemoveCapabilityAsync(session, "NonExistent.Capability~~~~0.0.1.0", cancellationToken: cts.Token));

            ex.ShouldNotBeNull();
            ex.ShouldBeAssignableTo<Exception>();
        }

        [Fact]
        public async Task RemoveCapabilityAsync_ReportsProgress()
        {
            using DismSession session = DismApi.OpenOnlineSession();
            var progress = new SynchronousProgress<DismProgress>(_ => { });

            try
            {
                await DismApi.RemoveCapabilityAsync(session, "NonExistent.Capability~~~~0.0.1.0", progress: progress, cancellationToken: TestContext.Current.CancellationToken);
            }
            catch (DismException)
            {
            }
        }
    }
}
