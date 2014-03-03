﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Weakly.MVVM
{
    /// <summary>
    /// Wraps a ViewModel method (with guard) in an <see cref="ICommand"/>.
    /// </summary>
    public sealed class WeakCommand : ICommand
    {
        private readonly WeakReference _targetReference;
        private readonly MethodInfo _method;
        private readonly WeakFunc<bool> _canExecute;
        private readonly WeakEventSource _canExecuteChangedSource = new WeakEventSource();
        private readonly string _guardName;

        /// <summary>
        /// Creates a new <see cref="WeakCommand"/> from the specified <paramref name="action"/>.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>The new <see cref="WeakCommand"/>.</returns>
        public static WeakCommand Create(Action action)
        {
            return CreateInternal(action);
        }

        /// <summary>
        /// Creates a new <see cref="WeakCommand"/> from the specified <paramref name="action"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="action">The action.</param>
        /// <returns>The new <see cref="WeakCommand"/>.</returns>
        public static WeakCommand Create<TResult>(Func<TResult> action)
        {
            return CreateInternal(action);
        }

        private static WeakCommand CreateInternal(Delegate action)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            if (action.Target == null)
                throw new ArgumentException("Method cannot be static.", "action");
            if (action.IsClosure())
                throw new ArgumentException("A closure cannot be used.", "action");

            return new WeakCommand(action.Target, action.Method);
        }

        private WeakCommand(object target, MethodInfo method)
        {
            _targetReference = new WeakReference(target);
            _method = method;

            _guardName = "Can" + _method.Name;
            var guard = target.GetType().GetMethod("get_" + _guardName);
            var inpc = target as INotifyPropertyChanged;
            if (inpc == null || guard == null) return;

            WeakEventHandler.Register<PropertyChangedEventArgs>(inpc, "PropertyChanged", OnPropertyChanged);
            _canExecute = new WeakFunc<bool>(inpc, guard);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.PropertyName) || e.PropertyName == _guardName)
            {
                if (UIContext.CheckAccess())
                    _canExecuteChangedSource.Raise(this, EventArgs.Empty);
                else
                    Task.Factory.StartNew(() => _canExecuteChangedSource.Raise(this, EventArgs.Empty),
                        CancellationToken.None, TaskCreationOptions.None, UIContext.TaskScheduler);
            }
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            var target = _targetReference.Target;
            if (target == null) return;

            var execute = DynamicDelegate.From(_method);
            var returnValue = execute(target, new object[0]);
            if (returnValue == null) return;

            var coTask = returnValue as ICoTask;
            if (coTask != null)
            {
                returnValue = new[] { coTask };
            }

            var enumerable = returnValue as IEnumerable<ICoTask>;
            if (enumerable != null)
            {
                returnValue = enumerable.GetEnumerator();
            }

            var enumerator = returnValue as IEnumerator<ICoTask>;
            if (enumerator != null)
            {
                var context = new CoroutineExecutionContext
                {
                    Source = this,
                    Target = target,
                };

                Coroutine.ExecuteAsync(enumerator, context);
            }
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute.Invoke();
        }

        /// <summary>
        /// Occurs when changes occur that affect whether the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { _canExecuteChangedSource.Add(value); }
            remove { _canExecuteChangedSource.Remove(value); }
        }
    }
}
