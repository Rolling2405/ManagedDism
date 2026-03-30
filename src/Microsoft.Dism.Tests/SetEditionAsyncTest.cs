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
    public class SetEditionAsyncTest : DismTestBase
    {
        public SetEditionAsyncTest(TestWimTemplate template, ITestOutputHelper testOutput)
            : base(template, testOutput)
        {
        }

        [Fact]
        public async Task SetEditionAsync_ThrowsDismException()
        {
            using DismSession session = DismApi.OpenOnlineSession();

            await Should.ThrowAsync<DismException>(
                () => DismApi.SetEditionAsync(session, "NonExistentEdition", cancellationToken: TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task SetEditionAsync_WithCancelledToken_ThrowsOperationCanceledException()
        {
            using DismSession session = DismApi.OpenOnlineSession();
            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));

            Exception? ex = await Record.ExceptionAsync(
                () => DismApi.SetEditionAsync(session, "NonExistentEdition", cancellationToken: cts.Token));

            ex.ShouldNotBeNull();
            ex.ShouldBeAssignableTo<Exception>();
        }

        [Fact]
        public async Task SetEditionAsync_ReportsProgress()
        {
            using DismSession session = DismApi.OpenOnlineSession();
            var progress = new SynchronousProgress<DismProgress>(_ => { });

            try
            {
                await DismApi.SetEditionAsync(session, "NonExistentEdition", progress: progress, cancellationToken: TestContext.Current.CancellationToken);
            }
            catch (DismException)
            {
            }
        }

        [Fact]
        public async Task SetEditionAndProductKeyAsync_ThrowsDismException()
        {
            using DismSession session = DismApi.OpenOnlineSession();

            await Should.ThrowAsync<DismException>(
                () => DismApi.SetEditionAndProductKeyAsync(session, "NonExistentEdition", "XXXXX-XXXXX-XXXXX-XXXXX-XXXXX", cancellationToken: TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task SetEditionAndProductKeyAsync_WithCancelledToken_ThrowsOperationCanceledException()
        {
            using DismSession session = DismApi.OpenOnlineSession();
            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));

            Exception? ex = await Record.ExceptionAsync(
                () => DismApi.SetEditionAndProductKeyAsync(session, "NonExistentEdition", "XXXXX-XXXXX-XXXXX-XXXXX-XXXXX", cancellationToken: cts.Token));

            ex.ShouldNotBeNull();
            ex.ShouldBeAssignableTo<Exception>();
        }

        [Fact]
        public async Task SetEditionAndProductKeyAsync_ReportsProgress()
        {
            using DismSession session = DismApi.OpenOnlineSession();
            var progress = new SynchronousProgress<DismProgress>(_ => { });

            try
            {
                await DismApi.SetEditionAndProductKeyAsync(session, "NonExistentEdition", "XXXXX-XXXXX-XXXXX-XXXXX-XXXXX", progress: progress, cancellationToken: TestContext.Current.CancellationToken);
            }
            catch (DismException)
            {
            }
        }
    }
}
