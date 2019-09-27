using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ATFaceME.Xamarin.Core.Controls
{
    public class Grouping<K,T>:ObservableCollection<T>
    {
        public K Key { get; private set; }
        public string Sede { get { if (Key is string) { return Key.ToString()[0].ToString().ToUpper(); } else { return "Z"; } } }
        public Grouping(K key, IEnumerable<T> items)
        {
            Key = key;
            foreach (var item in items)
                this.Items.Add(item);
        }
    }
}
