﻿using System;

namespace Weakly.MVVM
{
    /// <summary>
    /// Executes an action when disposed.
    /// </summary>
    public sealed class DisposableAction : IDisposable
    {
        private Action _action;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableAction"/> class.
        /// </summary>
        /// <param name="action">The action to execute on dispose.</param>
        public DisposableAction(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            _action = action;
        }

        /// <summary>
        /// Executes the supplied action.
        /// </summary>
        public void Dispose()
        {
            if (_action == null) return;
            _action();
            _action = null;
        }
    }
}
