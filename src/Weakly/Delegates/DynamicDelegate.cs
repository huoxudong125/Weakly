﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Weakly
{
    /// <summary>
    /// Helper to create dynamic delegate functions.
    /// </summary>
    public static class DynamicDelegate
    {
        /// <summary>
        /// Create a dynamic delegate from the specified method.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns>The dynamic delegate.</returns>
        public static Func<object, object[], object> From(MethodInfo method)
        {
            var action = Cache.GetValueOrNull(method.MethodHandle);
            if (action != null) return action;
            action = CompileFunction(method);
            Cache.AddOrReplace(method.MethodHandle, action);
            return action;
        }

        private static Func<object, object[], object> CompileFunction(MethodInfo method)
        {
            var parameterInfos = method.GetParameters();

            var instance = Expression.Parameter(typeof(object), "instance");
            var parameters = Expression.Parameter(typeof(object[]), "parameters");
            var instructions = new List<Expression>();

            var checkParametersLength = CheckParametersLength(parameters, parameterInfos.Length);
            instructions.Add(checkParametersLength);

            var typedInstance = ConvertInstance(instance, method);
            var parameterArray = ConvertParameters(parameters, parameterInfos);
            var methodCall = Expression.Call(typedInstance, method, parameterArray);

            if (method.ReturnType != typeof(void))
            {
                var convertedResult = Expression.Convert(methodCall, typeof(object));
                instructions.Add(convertedResult);
            }
            else
            {
                instructions.Add(methodCall);
                instructions.Add(Expression.Constant(null, typeof(object)));
            }

            var body = Expression.Block(instructions);
            return Expression.Lambda<Func<object, object[], object>>(body, instance, parameters).Compile();
        }

        private static Expression ConvertInstance(Expression instance, MethodBase method)
        {
            Expression typedInstance = null;
            if (!method.IsStatic)
            {
                typedInstance = Expression.Convert(instance, method.DeclaringType);
            }
            return typedInstance;
        }

        private static Expression[] ConvertParameters(ParameterExpression parameters, ParameterInfo[] parameterInfos)
        {
            var parameterArray = new Expression[parameterInfos.Length];
            for (var i = 0; i < parameterInfos.Length; i++)
            {
                var p = Expression.ArrayIndex(parameters, Expression.Constant(i, typeof (int)));
                parameterArray[i] = Expression.Convert(p, parameterInfos[i].ParameterType);
            }
            return parameterArray;
        }

        private static ConditionalExpression CheckParametersLength(Expression parameters, int length)
        {
            return Expression.IfThen(
                Expression.NotEqual(
                    Expression.ArrayLength(parameters),
                    Expression.Constant(length, typeof (int))),
                Expression.Throw(Expression.Constant(new TargetParameterCountException())));
        }

        #region Inner Types

        private static class Cache
        {
            private static readonly IDictionary<RuntimeMethodHandle, Func<object, object[], object>> Storage = new Dictionary<RuntimeMethodHandle, Func<object, object[], object>>();

            public static Func<object, object[], object> GetValueOrNull(RuntimeMethodHandle key)
            {
                Func<object, object[], object> func;
                lock (Storage)
                {
                    Storage.TryGetValue(key, out func);
                }
                return func;
            }

            public static void AddOrReplace(RuntimeMethodHandle key, Func<object, object[], object> func)
            {
                lock (Storage)
                {
                    Storage[key] = func;
                }
            }
        }

        #endregion
    }
}