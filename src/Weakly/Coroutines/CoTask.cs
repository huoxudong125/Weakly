﻿using System;

namespace Weakly.Coroutines
{
    /// <summary>
    /// Base class for all <see cref="ICoTask"/>.
    /// </summary>
    public abstract class CoTask : ICoTask
    {
        /// <summary>
        /// Executes the CoTask using the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public abstract void Execute(CoroutineExecutionContext context);

        /// <summary>
        /// Occurs when execution has completed.
        /// </summary>
        public event EventHandler<CoTaskCompletedEventArgs> Completed;

        /// <summary>
        /// Raises the <see cref="Completed" /> event.
        /// </summary>
        /// <param name="args">The <see cref="CoTaskCompletedEventArgs"/> instance containing the event data.</param>
        protected void OnCompleted(CoTaskCompletedEventArgs args)
        {
            var handler = Completed;
            if (handler != null)
            {
                handler(this, args);
            }
        }
    }
}
