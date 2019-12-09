using System.ComponentModel;
using System.Windows;

namespace Hashgraph.SigningTool.Models
{
    public abstract class BindableDependencyObject<TModel> : DependencyObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected static DependencyProperty RegisterProperty<TProperty>(string propertyName, TProperty defaultValue)
        {
            return DependencyProperty.Register(propertyName, typeof(TProperty), typeof(TModel), new PropertyMetadata(defaultValue, DependencyPropertyChangeHandler));
        }
        private static void DependencyPropertyChangeHandler(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs evt)
        {
            var source = dependencyObject as BindableDependencyObject<TModel>;
            source?.PropertyChanged?.Invoke(source, new PropertyChangedEventArgs(evt.Property.Name));
        }
    }
}
