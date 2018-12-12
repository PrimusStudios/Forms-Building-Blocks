using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Forms.BuildingBlocks.ViewModels
{
    public abstract class BindingBase : INotifyPropertyChanged
    {
        /// <summary>
        ///     Event for when the property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Raises Property changed events.
        /// </summary>
        protected virtual bool SetProperty<T>(ref T refProperty, T newValue,
            [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(refProperty, newValue))
                return false;

            refProperty = newValue;
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

            return true;
        }

        /// <summary>
        ///     Raises Property changed events and invokes an action for when the property changes.
        /// </summary>
        protected virtual bool SetProperty<T>(ref T refProperty, T newValue, Action onPropertyChanged,
            [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(refProperty, newValue))
                return false;

            refProperty = newValue;
            onPropertyChanged?.Invoke();
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

            return true;
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }
    }
}