// -----------------------------------------------------------------------
// <copyright file="Disposable.cs" company="Anorisoft">
// Copyright (c) Anorisoft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Anori.Disposables
{
    using System;

    using JetBrains.Annotations;

    /// <summary>
    ///     The Disposable class.
    /// </summary>
    public sealed class Disposable : IDisposable
    {
        /// <summary>
        ///     The action
        /// </summary>
        [NotNull]
        private readonly Action action;

        /// <summary>
        ///     The disposed
        /// </summary>
        [NotNull]
        private readonly SingleSet disposed = new SingleSet();

        /// <summary>
        ///     Initializes a new instance of the <see cref="Disposable" /> class.
        /// </summary>
        /// <param name="action">The action.</param>
        public Disposable([NotNull] Action action)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
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

            this.action();
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }
    }
}