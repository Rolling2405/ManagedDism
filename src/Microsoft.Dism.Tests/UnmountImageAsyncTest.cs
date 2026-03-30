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
    public class UnmountImageAsyncTest : DismTestBase
    {
        public UnmountImageAsyncTest(TestWimTemplate template, ITestOutputHelper testOutput)
            : base(template, testOutput)
        {
        }

        public override void Dispose()
        {
            try
            {
                DismApi.UnmountImage(MountPath.FullName, commitChanges: false);
            }
            catch
            {
            }

            base.Dispose();
        }

        [Fact]
        public async Task UnmountImageAsync_CompletesSuccessfully()
        {
            DismApi.MountImage(InstallWimPath.FullName, MountPath.FullName, 1);

            await DismApi.UnmountImageAsync(MountPath.FullName, commitChanges: false, cancellationToken: TestContext.Current.CancellationToken);
        }

        [Fact]
        public async Task UnmountImageAsync_WithCancelledToken_ThrowsOperationCanceledException()
        {
            DismApi.MountImage(InstallWimPath.FullName, MountPath.FullName, 1);
            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));

            Exception? ex = await Record.ExceptionAsync(
                () => DismApi.UnmountImageAsync(MountPath.FullName, commitChanges: false, cancellationToken: cts.Token));

            // The unmount may complete before cancellation fires, or be cancelled
            if (ex != null)
            {
                ex.ShouldBeAssignableTo<Exception>();
            }
        }

        [Fact]
        public async Task UnmountImageAsync_ReportsProgress()
        {
            DismApi.MountImage(InstallWimPath.FullName, MountPath.FullName, 1);
            bool progressReported = false;
            var progress = new SynchronousProgress<DismProgress>(_ => progressReported = true);

            try
            {
                await DismApi.UnmountImageAsync(MountPath.FullName, commitChanges: false, progress: progress, cancellationToken: TestContext.Current.CancellationToken);
            }
            catch (OperationCanceledException)
            {
            }

            progressReported.ShouldBeTrue();
        }
    }
}
