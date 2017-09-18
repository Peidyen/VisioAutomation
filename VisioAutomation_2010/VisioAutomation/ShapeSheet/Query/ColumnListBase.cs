using System.Collections.Generic;

namespace VisioAutomation.ShapeSheet.Query
{
    public class ColumnListBase<T> : IEnumerable<T> where T : ColumnBase
    {
        protected IList<T> _items;
        protected Dictionary<string, T> map_name_to_item;

        internal ColumnListBase() : this(0)
        {
        }

        internal ColumnListBase(int capacity)
        {
            this._items = new List<T>(capacity);
            this.map_name_to_item = new Dictionary<string, T>(capacity);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return (this._items).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public T this[int index] => this._items[index];

        public T this[string name] => this.map_name_to_item[name];

        public bool Contains(string name) => this.map_name_to_item.ContainsKey(name);

        protected string normalize_name(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = string.Format("Col{0}", this._items.Count);
            }
            return name;
        }

        public int Count => this._items.Count;

        protected void check_duplicate_column_name(string name)
        {
            if (this.map_name_to_item.ContainsKey(name))
            {
                throw new System.ArgumentException("Duplicate Column Name");
            }
        }
    }
}