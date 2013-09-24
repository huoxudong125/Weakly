﻿using System;
using System.Reflection;

namespace Weakly
{
    /// <summary>
    /// Weak version of <see cref="Func&lt;TResult&gt;"/> delegate.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
    public sealed class WeakFunc<TResult> : WeakDelegate
    {
        private readonly Func<object, TResult> _openFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakFunc&lt;TResult&gt;"/> class.
        /// </summary>
        /// <param name="function">The function delegate to encapsulate.</param>
        public WeakFunc(Func<TResult> function)
            : base(function.Target, function.Method.MethodHandle)
        {
            _openFunc = OpenFunc.From<TResult>(function.Method);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakFunc&lt;TResult&gt;"/> class.
        /// </summary>
        /// <param name="target">The class instance on which the current delegate invokes the instance method.</param>
        /// <param name="method">The method represented by the delegate.</param>
        public WeakFunc(object target, MethodInfo method)
            : base(target, method.MethodHandle)
        {
            _openFunc = OpenFunc.From<TResult>(method);
        }

        /// <summary>
        /// Invokes the method represented by the current weak delegate.
        /// </summary>
        /// <returns>The return value of the method that this delegate encapsulates.</returns>
        public TResult Invoke()
        {
            var target = Target;
            if (target != null)
                return _openFunc(target);
            return default(TResult);
        }
    }

    /// <summary>
    /// Weak version of <see cref="Func&lt;T, TResult&gt;"/> delegate.
    /// </summary>
    /// <typeparam name="T">The type of the parameter of the method that this delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
    public sealed class WeakFunc<T, TResult> : WeakDelegate
    {
        private readonly Func<object, T, TResult> _openFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakFunc&lt;T, TResult&gt;"/> class.
        /// </summary>
        /// <param name="function">The function delegate to encapsulate.</param>
        public WeakFunc(Func<T, TResult> function)
            : base(function.Target, function.Method.MethodHandle)
        {
            _openFunc = OpenFunc.From<T, TResult>(function.Method);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakFunc&lt;T, TResult&gt;"/> class.
        /// </summary>
        /// <param name="target">The class instance on which the current delegate invokes the instance method.</param>
        /// <param name="method">The method represented by the delegate.</param>
        public WeakFunc(object target, MethodInfo method)
            : base(target, method.MethodHandle)
        {
            _openFunc = OpenFunc.From<T, TResult>(method);
        }

        /// <summary>
        /// Invokes the method represented by the current weak delegate.
        /// </summary>
        /// <param name="obj">The parameter of the method that this delegate encapsulates.</param>
        /// <returns>The return value of the method that this delegate encapsulates.</returns>
        public TResult Invoke(T obj)
        {
            var target = Target;
            if (target != null)
                return _openFunc(target, obj);
            return default(TResult);
        }
    }

    /// <summary>
    /// Weak version of <see cref="Func&lt;T1, T2, TResult&gt;"/> delegate.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
    public sealed class WeakFunc<T1, T2, TResult> : WeakDelegate
    {
        private readonly Func<object, T1, T2, TResult> _openFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakFunc&lt;T1, T2, TResult&gt;"/> class.
        /// </summary>
        /// <param name="function">The function delegate to encapsulate.</param>
        public WeakFunc(Func<T1, T2, TResult> function)
            : base(function.Target, function.Method.MethodHandle)
        {
            _openFunc = OpenFunc.From<T1, T2, TResult>(function.Method);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakFunc&lt;T1, T2, TResult&gt;"/> class.
        /// </summary>
        /// <param name="target">The class instance on which the current delegate invokes the instance method.</param>
        /// <param name="method">The method represented by the delegate.</param>
        public WeakFunc(object target, MethodInfo method)
            : base(target, method.MethodHandle)
        {
            _openFunc = OpenFunc.From<T1, T2, TResult>(method);
        }

        /// <summary>
        /// Invokes the method represented by the current weak delegate.
        /// </summary>
        /// <param name="arg1">The first parameter of the method that this delegate encapsulates.</param>
        /// <param name="arg2">The second parameter of the method that this delegate encapsulates.</param>
        /// <returns>The return value of the method that this delegate encapsulates.</returns>
        public TResult Invoke(T1 arg1, T2 arg2)
        {
            var target = Target;
            if (target != null)
                return _openFunc(target, arg1, arg2);
            return default(TResult);
        }
    }

    /// <summary>
    /// Weak version of <see cref="Func&lt;T1, T2, T3, TResult&gt;"/> delegate.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this delegate encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
    public sealed class WeakFunc<T1, T2, T3, TResult> : WeakDelegate
    {
        private readonly Func<object, T1, T2, T3, TResult> _openFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakFunc&lt;T1, T2, T3, TResult&gt;"/> class.
        /// </summary>
        /// <param name="function">The function delegate to encapsulate.</param>
        public WeakFunc(Func<T1, T2, T3, TResult> function)
            : base(function.Target, function.Method.MethodHandle)
        {
            _openFunc = OpenFunc.From<T1, T2, T3, TResult>(function.Method);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakFunc&lt;T1, T2, T3, TResult&gt;"/> class.
        /// </summary>
        /// <param name="target">The class instance on which the current delegate invokes the instance method.</param>
        /// <param name="method">The method represented by the delegate.</param>
        public WeakFunc(object target, MethodInfo method)
            : base(target, method.MethodHandle)
        {
            _openFunc = OpenFunc.From<T1, T2, T3, TResult>(method);
        }

        /// <summary>
        /// Invokes the method represented by the current weak delegate.
        /// </summary>
        /// <param name="arg1">The first parameter of the method that this delegate encapsulates.</param>
        /// <param name="arg2">The second parameter of the method that this delegate encapsulates.</param>
        /// <param name="arg3">The third parameter of the method that this delegate encapsulates.</param>
        /// <returns>The return value of the method that this delegate encapsulates.</returns>
        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3)
        {
            var target = Target;
            if (target != null)
                return _openFunc(target, arg1, arg2, arg3);
            return default(TResult);
        }
    }
}
