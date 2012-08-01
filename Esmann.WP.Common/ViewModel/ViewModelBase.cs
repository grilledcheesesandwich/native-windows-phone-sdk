using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows.Threading;

namespace esmann.WP.Common.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public void NotifyPropertyChanged<T>(Expression<Func<T>> expression)
        {
            MemberExpression body = (MemberExpression)expression.Body;
            if (!body.Member.ReflectedType.IsPublic)
            {
                throw new ArgumentException("Member should be Public", body.Member.Name);
            }
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
    }
}
