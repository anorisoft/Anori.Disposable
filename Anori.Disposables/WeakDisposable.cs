// -----------------------------------------------------------------------
// <copyright file="WeakDisposable.cs" company="Anorisoft">
// Copyright (c) Anorisoft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Anori.Disposables
{
    using System;

    using JetBrains.Annotations;

    /// <summary>
    ///     WeakDisposable
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public sealed class WeakDisposable : IDisposable
    {
        /// <summary>
        ///     The disposed
        /// </summary>
        [NotNull]
        private readonly SingleSet disposed = new SingleSet();

        /// <summary>
        ///     The weak disposable
        /// </summary>
        [NotNull]
        private readonly WeakReference<IDisposable> weakDisposable;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WeakDisposable" /> class.
        /// </summary>
        /// <param name="disposable">The disposable.</param>
        public WeakDisposable([NotNull] IDisposable disposable)
        {
            if (disposable == null)
            {
                throw new ArgumentNullException(nameof(disposable));
            }

            this.weakDisposable = new WeakReference<IDisposable>(disposable);
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        ///     unmanaged resources.
        /// </param>
        private void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (this.disposed.Set())
            {
                return;
            }

            if (!this.weakDisposable.TryGetTarget(out var disposable))
            {
                return;
            }

            disposable.Dispose();
        }
    }
}