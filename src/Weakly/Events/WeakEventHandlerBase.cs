﻿using System;
using System.Reflection;

namespace Weakly
{
    /// <summary>
    /// Base class for weak event handler.
    /// </summary>
    /// <typeparam name="TSubscriber">The type of the event subscriber.</typeparam>
    /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
    public abstract class WeakEventHandlerBase<TSubscriber, TEventArgs> : IWeakEventHandler
        where TSubscriber : class
    {
        private readonly WeakReference<TSubscriber> _subscriber;
        private readonly Action<TSubscriber, object, TEventArgs> _weakHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakEventHandlerBase&lt;TSubscriber, TEventArgs&gt;"/> class.
        /// </summary>
        /// <param name="subscriber">The event subscriber.</param>
        /// <param name="weakHandler">The weak handler.</param>
        protected WeakEventHandlerBase(TSubscriber subscriber, Action<TSubscriber, object, TEventArgs> weakHandler)
        {
            if (subscriber == null)
                throw new ArgumentNullException("subscriber");
            if (weakHandler == null)
                throw new ArgumentNullException("weakHandler");
            if (weakHandler.GetMethodInfo().IsClosure())
                throw new ArgumentException("Cannot create weak event handler from closure.", "weakHandler");

            _subscriber = new WeakReference<TSubscriber>(subscriber);
            _weakHandler = weakHandler;
        }

        /// <summary>
        /// Removes the event handler from the event source.
        /// </summary>
        protected abstract void RemoveEventHandler();

        /// <summary>
        /// Method called when the event is raised.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <typeparamref name="TEventArgs"/> instance containing the event data.</param>
        /// <remarks>Register this method on the source event.</remarks>
        protected void OnEvent(object sender, TEventArgs args)
        {
            TSubscriber subscriber;
            if (_subscriber.TryGetTarget(out subscriber))
            {
                _weakHandler(subscriber, sender, args);
            }
            else
            {
                RemoveEventHandler();
            }
        }

        /// <summary>
        /// Unregisters the event handler from the event source.
        /// </summary>
        public void Dispose()
        {
            RemoveEventHandler();
        }
    }

    /// <summary>
    /// Base class for weak event handler.
    /// </summary>
    /// <typeparam name="TSource">The type of the event source.</typeparam>
    /// <typeparam name="TSubscriber">The type of the event subscriber.</typeparam>
    /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
    public abstract class WeakEventHandlerBase<TSource, TSubscriber, TEventArgs> : IWeakEventHandler, IDisposable
        where TSource : class
        where TSubscriber : class
    {
        private readonly WeakReference<TSource> _source; 
        private readonly WeakReference<TSubscriber> _subscriber;
        private readonly Action<TSubscriber, object, TEventArgs> _weakHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakEventHandlerBase&lt;TSubscriber, TEventArgs&gt;"/> class.
        /// </summary>
        /// <param name="source">The event source.</param>
        /// <param name="subscriber">The event subscriber.</param>
        /// <param name="weakHandler">The weak handler.</param>
        protected WeakEventHandlerBase(TSource source, TSubscriber subscriber, Action<TSubscriber, object, TEventArgs> weakHandler)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (subscriber == null)
                throw new ArgumentNullException("subscriber");
            if (weakHandler == null)
                throw new ArgumentNullException("weakHandler");
            if (weakHandler.GetMethodInfo().IsClosure())
                throw new ArgumentException("Cannot create weak event handler from closure.", "weakHandler");

            _source = new WeakReference<TSource>(source);
            _subscriber = new WeakReference<TSubscriber>(subscriber);
            _weakHandler = weakHandler;
        }

        private void RemoveEventHandler()
        {
            TSource source;
            if (_source.TryGetTarget(out source))
            {
                RemoveEventHandler(source);
            }
        }

        /// <summary>
        /// Removes the event handler from the event source.
        /// </summary>
        /// <param name="source">The event source.</param>
        protected abstract void RemoveEventHandler(TSource source);

        /// <summary>
        /// Method called when the event is raised.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <typeparamref name="TEventArgs"/> instance containing the event data.</param>
        /// <remarks>Register this method on the source event.</remarks>
        protected void OnEvent(object sender, TEventArgs args)
        {
            TSubscriber subscriber;
            if (_subscriber.TryGetTarget(out subscriber))
            {
                _weakHandler(subscriber, sender, args);
            }
            else
            {
                RemoveEventHandler();
            }
        }

        /// <summary>
        /// Unregisters the event handler from the event source.
        /// </summary>
        public void Dispose()
        {
            RemoveEventHandler();
        }
    }
}