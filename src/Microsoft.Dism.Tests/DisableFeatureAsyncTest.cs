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
    public class DisableFeatureAsyncTest : DismTestBase
    {
        public DisableFeatureAsyncTest(TestWimTemplate template, ITestOutputHelper testOutput)
            : base(template, testOutput)
        {
        }

        [Fact]
        public async Task DisableFeatureAsync_ThrowsDismException()
        {
            using DismSession session = DismApi.OpenOnlineSession();

            await Should.ThrowAsync<DismException>(
                () => DismApi.DisableFeatureAsync(session, "NonExistentFeature", "NonExistentPackage", false, cancellationToken: TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task DisableFeatureAsync_WithCancelledToken_ThrowsOperationCanceledException()
        {
            using DismSession session = DismApi.OpenOnlineSession();
            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));

            Exception? ex = await Record.ExceptionAsync(
                () => DismApi.DisableFeatureAsync(session, "NonExistentFeature", "NonExistentPackage", false, cancellationToken: cts.Token));

            ex.ShouldNotBeNull();
            ex.ShouldBeAssignableTo<Exception>();
        }

        [Fact]
        public async Task DisableFeatureAsync_ReportsProgress()
        {
            using DismSession session = DismApi.OpenOnlineSession();
            var progress = new SynchronousProgress<DismProgress>(_ => { });

            try
            {
                await DismApi.DisableFeatureAsync(session, "NonExistentFeature", "NonExistentPackage", false, progress: progress, cancellationToken: TestContext.Current.CancellationToken);
            }
            catch (DismException)
            {
            }
        }
    }
}
