using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;

namespace esmann.WP.Common.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        //internal static string GetPropertyName<T>(Expression<Func<T>> expression)
        //{
        //    MemberExpression body = (MemberExpression)expression.Body;
        //    return body.Member.Name;
        //}
        
        public void NotifyPropertyChanged<T>(Expression<Func<T>> expression)
        {
            MemberExpression body = (MemberExpression)expression.Body;
            var name = body.Member.Name;
            NotifyPropertyChanged(name);
        }

        private void NotifyPropertyChanged(String propertyName)
        {
            SmartDispatcher.BeginInvoke(() =>
            {
                PropertyChangedEventHandler handler = _PropertyChanged;
                if (null != handler)
                {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
            });
        }


        List<PropertyChangedEventHandler> delegates = new List<PropertyChangedEventHandler>();
        private event PropertyChangedEventHandler _PropertyChanged;

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                _PropertyChanged += value;
                delegates.Add(value);
            }

            remove
            {
                _PropertyChanged -= value;
                delegates.Remove(value);
            }
        }
        public int getEventsCount()
        {
            return delegates.Count;
        }
        
        public void RemoveAllEvents()
        {
            foreach (PropertyChangedEventHandler eh in delegates)
            {
                _PropertyChanged -= eh;
            }
            delegates.Clear();
        }

        //~ViewModelBase()
        //{
        //    Dispose();
        //}
        //bool isDisposeing = false;
        //public virtual void Dispose()
        //{
        //    if (!isDisposeing)
        //    {
        //        isDisposeing = true;
        //        int count = getEventsCount();
        //        RemoveAllEvents();
        //        Debug.WriteLine(String.Format("[ViewModelBase] Disposing event count: {0}", count));
        //    }
        //}

    }
}
