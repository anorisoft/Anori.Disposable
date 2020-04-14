// -----------------------------------------------------------------------
// <copyright file="DisposableBase.cs" company="Anorisoft">
// Copyright (c) Anorisoft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Anori.Disposables
{
    using System;

    using JetBrains.Annotations;

    /// <summary>
    ///     The class forms a base class for all classes that use the Disopsable method.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public abstract class DisposableBase : IDisposable
    {
        /// <summary>
        ///     The disposal tracker
        /// </summary>
        private readonly DisposalTracker disposalTracker;

        /// <summary>
        ///     The disposed
        /// </summary>
        private readonly SingleSet disposed = new SingleSet();

        /// <summary>
        ///     Initializes a new instance of the <see cref="DisposableBase" /> class.
        /// </summary>
        protected DisposableBase()
        {
            this.disposalTracker = new DisposalTracker();
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="DisposableBase" /> class.
        /// </summary>
        ~DisposableBase()
        {
            this.Dispose(false);
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
        ///     Adds to disposal tracker.
        /// </summary>
        /// <typeparam name="TDisposable">The type of the disposable.</typeparam>
        /// <param name="disposable">The disposable.</param>
        /// <returns></returns>
        [NotNull]
        protected TDisposable AddToDisposalTracker<TDisposable>([NotNull] TDisposable disposable)
            where TDisposable : IDisposable
        {
            this.disposalTracker.Add(disposable);
            return disposable;
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        ///     unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (this.disposed.Set())
            {
                return;
            }

            this.disposalTracker.Dispose();
        }
    }
}