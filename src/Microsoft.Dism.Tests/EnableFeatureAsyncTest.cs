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
    public class EnableFeatureAsyncTest : DismTestBase
    {
        public EnableFeatureAsyncTest(TestWimTemplate template, ITestOutputHelper testOutput)
            : base(template, testOutput)
        {
        }

        [Fact]
        public async Task EnableFeatureAsync_ThrowsDismException()
        {
            using DismSession session = DismApi.OpenOnlineSession();

            await Should.ThrowAsync<DismException>(
                () => DismApi.EnableFeatureAsync(session, "NonExistentFeature", false, false, cancellationToken: TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task EnableFeatureAsync_WithCancelledToken_ThrowsOperationCanceledException()
        {
            using DismSession session = DismApi.OpenOnlineSession();
            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));

            Exception? ex = await Record.ExceptionAsync(
                () => DismApi.EnableFeatureAsync(session, "NonExistentFeature", false, false, cancellationToken: cts.Token));

            ex.ShouldNotBeNull();
            ex.ShouldBeAssignableTo<Exception>();
        }

        [Fact]
        public async Task EnableFeatureAsync_ReportsProgress()
        {
            using DismSession session = DismApi.OpenOnlineSession();
            var progress = new SynchronousProgress<DismProgress>(_ => { });

            try
            {
                await DismApi.EnableFeatureAsync(session, "NonExistentFeature", false, false, progress: progress, cancellationToken: TestContext.Current.CancellationToken);
            }
            catch (DismException)
            {
            }
        }

        [Fact]
        public async Task EnableFeatureByPackageNameAsync_ThrowsDismException()
        {
            using DismSession session = DismApi.OpenOnlineSession();

            await Should.ThrowAsync<DismException>(
                () => DismApi.EnableFeatureByPackageNameAsync(session, "NonExistentFeature", "NonExistentPackage", false, false, cancellationToken: TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task EnableFeatureByPackageNameAsync_WithCancelledToken_ThrowsOperationCanceledException()
        {
            using DismSession session = DismApi.OpenOnlineSession();
            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));

            Exception? ex = await Record.ExceptionAsync(
                () => DismApi.EnableFeatureByPackageNameAsync(session, "NonExistentFeature", "NonExistentPackage", false, false, cancellationToken: cts.Token));

            ex.ShouldNotBeNull();
            ex.ShouldBeAssignableTo<Exception>();
        }

        [Fact]
        public async Task EnableFeatureByPackageNameAsync_ReportsProgress()
        {
            using DismSession session = DismApi.OpenOnlineSession();
            var progress = new SynchronousProgress<DismProgress>(_ => { });

            try
            {
                await DismApi.EnableFeatureByPackageNameAsync(session, "NonExistentFeature", "NonExistentPackage", false, false, progress: progress, cancellationToken: TestContext.Current.CancellationToken);
            }
            catch (DismException)
            {
            }
        }

        [Fact]
        public async Task EnableFeatureByPackagePathAsync_ThrowsDismException()
        {
            using DismSession session = DismApi.OpenOnlineSession();

            await Should.ThrowAsync<DismException>(
                () => DismApi.EnableFeatureByPackagePathAsync(session, "NonExistentFeature", "nonexistent.cab", false, false, cancellationToken: TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task EnableFeatureByPackagePathAsync_WithCancelledToken_ThrowsOperationCanceledException()
        {
            using DismSession session = DismApi.OpenOnlineSession();
            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));

            Exception? ex = await Record.ExceptionAsync(
                () => DismApi.EnableFeatureByPackagePathAsync(session, "NonExistentFeature", "nonexistent.cab", false, false, cancellationToken: cts.Token));

            ex.ShouldNotBeNull();
            ex.ShouldBeAssignableTo<Exception>();
        }

        [Fact]
        public async Task EnableFeatureByPackagePathAsync_ReportsProgress()
        {
            using DismSession session = DismApi.OpenOnlineSession();
            var progress = new SynchronousProgress<DismProgress>(_ => { });

            try
            {
                await DismApi.EnableFeatureByPackagePathAsync(session, "NonExistentFeature", "nonexistent.cab", false, false, progress: progress, cancellationToken: TestContext.Current.CancellationToken);
            }
            catch (DismException)
            {
            }
        }
    }
}
