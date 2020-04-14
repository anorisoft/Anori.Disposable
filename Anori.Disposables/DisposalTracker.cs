// -----------------------------------------------------------------------
// <copyright file="DisposalTracker.cs" company="Anorisoft">
// Copyright (c) Anorisoft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Anori.Disposables
{
    #region

    using System;
    using System.Collections.Generic;

    using JetBrains.Annotations;

    #endregion

    /// <summary>
    ///     Tracks disposable objects, and disposes them in the reverse order they were added to
    ///     the tracker.
    /// </summary>
    public sealed class DisposalTracker : IDisposable
    {
        /// <summary>
        ///     The disposed is true, when the Dispose method was called.
        /// </summary>
        [NotNull]
        private readonly SingleSet disposed = new SingleSet();

        /// <summary>
        ///     The to dispose is the stack of the objects were added to the tracker
        /// </summary>
        [NotNull]
        private readonly Stack<IDisposable> toDispose = new Stack<IDisposable>();

        /// <summary>
        ///     Finalizes an instance of the <see cref="DisposalTracker" /> class.
        /// </summary>
        ~DisposalTracker()
        {
            this.Dispose(false);
        }

        /// <summary>
        ///     Add an object where implements IDisposable to be disposed.
        /// </summary>
        /// <typeparam name="TDisposable">
        ///     The generic type <see cref="TDisposable" /> where implements IDisposable.
        /// </typeparam>
        /// <param name="disposable">
        ///     The object to be disposed.
        /// </param>
        /// <returns>
        ///     Return the <see cref="disposable" /> object of generic type <see cref="TDisposable" />.
        /// </returns>
        [NotNull]
        public TDisposable Add<TDisposable>([NotNull] TDisposable disposable)
            where TDisposable : IDisposable
        {
            if (disposable == null)
            {
                throw new ArgumentNullException(nameof(disposable));
            }

            this.toDispose.Push(disposable);
            return disposable;
        }

        /// <summary>
        ///     Adds the specified disposable.
        /// </summary>
        /// <param name="disposable">The disposable.</param>
        /// <param name="isWeak">if set to <c>true</c> [is weak].</param>
        /// <returns>Disposable object.</returns>
        /// <exception cref="ArgumentNullException">disposable is null.</exception>
        [NotNull]
        public IDisposable Add([NotNull] IDisposable disposable, bool isWeak)
        {
            if (disposable == null)
            {
                throw new ArgumentNullException(nameof(disposable));
            }

            return isWeak ? this.AddWeak(disposable) : this.Add(disposable);
        }

        /// <summary>
        ///     Adds the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">action</exception>
        [NotNull]
        public IDisposable Add([NotNull] Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return this.Add(new Disposable(action));
        }

        /// <summary>
        ///     Adds the weak.
        /// </summary>
        /// <param name="disposable">The disposable.</param>
        /// <returns></returns>
        [NotNull]
        public IDisposable AddWeak([NotNull] IDisposable disposable)
        {
            if (disposable == null)
            {
                throw new ArgumentNullException(nameof(disposable));
            }

            var weakDisposable = new WeakDisposable(disposable);
            this.toDispose.Push(weakDisposable);
            return weakDisposable;
        }

        /// <summary>
        ///     The dispose, disposes the objects in the reverse order they were added.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     The dispose. disposes the objects in the reverse order they were added.
        /// </summary>
        /// <param name="disposing">
        ///     The disposing is the condition to run the managed code.
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

            foreach (var disposable in this.toDispose)
            {
                disposable.Dispose();
            }

            this.toDispose.Clear();
        }
    }
}