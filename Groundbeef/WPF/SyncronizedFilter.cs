using Groundbeef.Collections;
using Groundbeef.Events;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Controls;

namespace Groundbeef.WPF
{
    /// <summary>
    /// Syncronizes the <see cref="ICollectionView.Filter"/> property of multiple <see cref="ICollectionView"/>s with the <see cref="Predicate{object?}"/> provided.
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(true)]
    public class SyncronizedFilter : ViewModelBase
    {
        private static readonly Predicate<object?> s_defaultFilter = _ => true;
        private Predicate<object?> _filter = s_defaultFilter;
        private List<ICollectionView> _subscribers;
        private Dictionary<INotifyCollectionChanged, ICollectionView> _observableCollectionMap = new Dictionary<INotifyCollectionChanged, ICollectionView>();
        private event ValueChangedEventHandler<Predicate<object?>?>? FilterChanged;
        private string _name = String.Empty;
        private bool _hasName = false;

        /// <summary>
        /// Initializes a new instance of <see cref="SyncronizedFilter"/>.
        /// </summary>
        public SyncronizedFilter()
        {
            _subscribers = new List<ICollectionView>();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SyncronizedFilter"/>, with a specifed name.
        /// </summary>
        public SyncronizedFilter(string name) : this()
        {
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SyncronizedFilter"/>, with a specifed name and collection of subscribers.
        /// </summary>
        public SyncronizedFilter(string name, IEnumerable<ICollectionView> subscribers)
        {
            Name = name;
            _subscribers = new List<ICollectionView>(subscribers);
        }

        /// <summary>
        /// Gets or sets the name of the instance.
        /// </summary>
        /// <remarks>Can only be assigned once. Must not be null or whitespace.</remarks>
        public string Name
        {
            get => _name;
            set
            {
                if (_hasName)
                    throw new InvalidOperationException("Name can only be set once.");
                if (String.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(ExceptionResource.STRING_NULLWHITESPACE);
                _hasName = true;
                _name = value;
            }
        }

        public bool HasName => _hasName;

        /// <summary>
        /// Gets or sets the default value for the <see cref="RefreshOnObservalbeChanged"/> property assigned when initializing a new instance of <see cref="SyncronizedFilter"/>.
        /// </summary>
        public static bool DefaultRefreshOnObservalbleChanged { get; set; } = true;

        /// <summary>
        /// Gets or sets the value indicating whether to refresh subscribed <see cref="ItemsControl"/>s with a <see cref="INotifyCollectionChanged"/> as <see cref="ItemsControl.ItemsSource"/> when they change.
        /// </summary>
        public bool RefreshOnObservalbeChanged { get; set; } = DefaultRefreshOnObservalbleChanged;

        /// <summary>
        /// Gets or sets the filter syncronized across all subscribed <see cref="ICollectionView"/>s.
        /// </summary>
        [AllowNull]
        public Predicate<object?> Filter
        {
            get => _filter;
            set
            {
                Predicate<object?>? oldValue = _filter;
                Set(ref _filter, value ?? s_defaultFilter);
                OnFilterChanged(oldValue, _filter);
            }
        }

        /// <summary>
        /// Subscribes a <see cref="ItemsControl"/> to the <see cref="SyncronizedFilter"/>.
        /// If the <see cref="ItemsControl.ItemsSource"/> is a <see cref="INotifyCollectionChanged{T}"/>, then also automatically refreshes the view when changed.
        /// </summary>
        /// <param name="itemsControl"></param>
        /// <returns></returns>
        public virtual bool AddView(ItemsControl itemsControl)
        {
            // Add the collectionview to the registered values
            ICollectionView collectionView = itemsControl.GetDefaultView();
            if (_subscribers.Any(cv => ReferenceEquals(cv, collectionView)))
                return false;
            _subscribers.Add(collectionView);
            // The ItemsSource is a ObservalbeCollection or simmilar
            if (itemsControl.ItemsSource is INotifyCollectionChanged observable)
            {
                observable.CollectionChanged += OnObservaleCollectionChanged;
                _observableCollectionMap.Add(observable, collectionView);
            }
            return true;
        }

        /// <summary>
        /// Unsubscribes a <see cref="ItemsControl"/> from the <see cref="SyncronizedFilter"/>.
        /// </summary>
        /// <param name="itemsControl"></param>
        /// <returns></returns>
        public virtual bool RemoveView(ItemsControl itemsControl)
        {
            // Remove the collectionview from the registered values
            ICollectionView collectionView = itemsControl.GetDefaultView();
            int index = _subscribers.IndexOf(cv => ReferenceEquals(cv, collectionView));
            if (index == -1)
                return false;
            _subscribers.RemoveAt(index);
            // Remove event subscriber
            if (itemsControl.ItemsSource is INotifyCollectionChanged observable)
            {
                observable.CollectionChanged -= OnObservaleCollectionChanged;
                _observableCollectionMap.Remove(observable);
            }
            return true;
        }

        protected virtual void OnObservaleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (RefreshOnObservalbeChanged && sender is INotifyCollectionChanged observalbe && (e.Action & NotifyCollectionChangedAction.Add) == NotifyCollectionChangedAction.Add)
            {
                _observableCollectionMap[observalbe].Refresh();
            }
        }

        protected virtual void OnFilterChanged(Predicate<object?>? oldValue, Predicate<object?>? newValue)
        {
            for (int i = _subscribers.Count; i >= 0; i--)
            {
                if (_subscribers[i] is null)
                    _subscribers.RemoveAt(i);
                else if (!ReferenceEquals(_subscribers[i].Filter, _filter))
                {
                    _subscribers[i].Filter = _filter;
                    _subscribers[i].Refresh();
                }
            }
            FilterChanged?.Invoke(this, new ValueChangedEventArgs<Predicate<object?>?>(oldValue, newValue));
        }
    }
}
