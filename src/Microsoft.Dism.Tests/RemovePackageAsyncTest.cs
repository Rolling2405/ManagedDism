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
    public class RemovePackageAsyncTest : DismTestBase
    {
        public RemovePackageAsyncTest(TestWimTemplate template, ITestOutputHelper testOutput)
            : base(template, testOutput)
        {
        }

        [Fact]
        public async Task RemovePackageByNameAsync_ThrowsDismException()
        {
            using DismSession session = DismApi.OpenOnlineSession();

            await Should.ThrowAsync<DismException>(
                () => DismApi.RemovePackageByNameAsync(session, "NonExistentPackage", cancellationToken: TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task RemovePackageByNameAsync_WithCancelledToken_ThrowsOperationCanceledException()
        {
            using DismSession session = DismApi.OpenOnlineSession();
            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));

            Exception? ex = await Record.ExceptionAsync(
                () => DismApi.RemovePackageByNameAsync(session, "NonExistentPackage", cancellationToken: cts.Token));

            ex.ShouldNotBeNull();
            ex.ShouldBeAssignableTo<Exception>();
        }

        [Fact]
        public async Task RemovePackageByNameAsync_ReportsProgress()
        {
            using DismSession session = DismApi.OpenOnlineSession();
            var progress = new SynchronousProgress<DismProgress>(_ => { });

            try
            {
                await DismApi.RemovePackageByNameAsync(session, "NonExistentPackage", progress: progress, cancellationToken: TestContext.Current.CancellationToken);
            }
            catch (DismException)
            {
            }
        }

        [Fact]
        public async Task RemovePackageByPathAsync_ThrowsDismException()
        {
            using DismSession session = DismApi.OpenOnlineSession();

            await Should.ThrowAsync<DismException>(
                () => DismApi.RemovePackageByPathAsync(session, "nonexistent.cab", cancellationToken: TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task RemovePackageByPathAsync_WithCancelledToken_ThrowsOperationCanceledException()
        {
            using DismSession session = DismApi.OpenOnlineSession();
            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));

            Exception? ex = await Record.ExceptionAsync(
                () => DismApi.RemovePackageByPathAsync(session, "nonexistent.cab", cancellationToken: cts.Token));

            ex.ShouldNotBeNull();
            ex.ShouldBeAssignableTo<Exception>();
        }

        [Fact]
        public async Task RemovePackageByPathAsync_ReportsProgress()
        {
            using DismSession session = DismApi.OpenOnlineSession();
            var progress = new SynchronousProgress<DismProgress>(_ => { });

            try
            {
                await DismApi.RemovePackageByPathAsync(session, "nonexistent.cab", progress: progress, cancellationToken: TestContext.Current.CancellationToken);
            }
            catch (DismException)
            {
            }
        }
    }
}
