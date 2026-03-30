// Copyright (c). All rights reserved.
//
// Licensed under the MIT license.

using System;

namespace Microsoft.Dism.Tests
{
    internal sealed class SynchronousProgress<T> : IProgress<T>
    {
        private readonly Action<T> _handler;

        public SynchronousProgress(Action<T> handler) => _handler = handler;

        public void Report(T value) => _handler(value);
    }
}
