using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TestVPN.ViewModels
{
    public class NotificationBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] String property
           = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            RaisePropertyChanged(property);
            return true;
        }

        protected void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }

    public class NotificationBase<T> : NotificationBase where T : class, new()
    {
        protected T This;

        public static implicit operator T(NotificationBase<T> thing) { return thing.This; }

        public NotificationBase(T thing = null)
        {
            This = (thing == null) ? new T() : thing;
        }
    }
}
