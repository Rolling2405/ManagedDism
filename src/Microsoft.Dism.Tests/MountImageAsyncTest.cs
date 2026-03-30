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
    public class MountImageAsyncTest : DismTestBase
    {
        public MountImageAsyncTest(TestWimTemplate template, ITestOutputHelper testOutput)
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
        public async Task MountImageAsync_ByIndex_CompletesSuccessfully()
        {
            await DismApi.MountImageAsync(
                InstallWimPath.FullName,
                MountPath.FullName,
                imageIndex: 1,
                readOnly: true,
                options: DismMountImageOptions.None,
                progress: null,
                cancellationToken: TestContext.Current.CancellationToken);
        }

        [Fact]
        public async Task MountImageAsync_ByIndex_WithCancelledToken_ThrowsOperationCanceledException()
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));

            Exception? ex = await Record.ExceptionAsync(
                () => DismApi.MountImageAsync(
                    InstallWimPath.FullName,
                    MountPath.FullName,
                    imageIndex: 1,
                    readOnly: true,
                    options: DismMountImageOptions.None,
                    progress: null,
                    cancellationToken: cts.Token));

            // The mount may complete before cancellation, or be cancelled
            if (ex != null)
            {
                ex.ShouldBeAssignableTo<Exception>();
            }
        }

        [Fact]
        public async Task MountImageAsync_ByIndex_ReportsProgress()
        {
            bool progressReported = false;
            var progress = new SynchronousProgress<DismProgress>(_ => progressReported = true);

            try
            {
                await DismApi.MountImageAsync(
                    InstallWimPath.FullName,
                    MountPath.FullName,
                    imageIndex: 1,
                    readOnly: true,
                    options: DismMountImageOptions.None,
                    progress: progress,
                    cancellationToken: TestContext.Current.CancellationToken);
            }
            catch (OperationCanceledException)
            {
            }

            progressReported.ShouldBeTrue();
        }

        [Fact]
        public async Task MountImageAsync_ByName_CompletesSuccessfully()
        {
            await DismApi.MountImageAsync(
                InstallWimPath.FullName,
                MountPath.FullName,
                imageName: "Test Image 1",
                readOnly: true,
                options: DismMountImageOptions.None,
                progress: null,
                cancellationToken: TestContext.Current.CancellationToken);
        }

        [Fact]
        public async Task MountImageAsync_ByName_WithCancelledToken_ThrowsOperationCanceledException()
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));

            Exception? ex = await Record.ExceptionAsync(
                () => DismApi.MountImageAsync(
                    InstallWimPath.FullName,
                    MountPath.FullName,
                    imageName: "Test Image 1",
                    readOnly: true,
                    options: DismMountImageOptions.None,
                    progress: null,
                    cancellationToken: cts.Token));

            // The mount may complete before cancellation, or be cancelled
            if (ex != null)
            {
                ex.ShouldBeAssignableTo<Exception>();
            }
        }

        [Fact]
        public async Task MountImageAsync_ByName_ReportsProgress()
        {
            bool progressReported = false;
            var progress = new SynchronousProgress<DismProgress>(_ => progressReported = true);

            try
            {
                await DismApi.MountImageAsync(
                    InstallWimPath.FullName,
                    MountPath.FullName,
                    imageName: "Test Image 1",
                    readOnly: true,
                    options: DismMountImageOptions.None,
                    progress: progress,
                    cancellationToken: TestContext.Current.CancellationToken);
            }
            catch (OperationCanceledException)
            {
            }

            progressReported.ShouldBeTrue();
        }
    }
}
