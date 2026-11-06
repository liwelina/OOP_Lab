using ClinicApp.Interfaces;

namespace ClinicApp.Managers
{
    public class Repository<T> where T : IIdentifiable
    {
        private List<T> _items = new List<T>();

        public int Count => _items.Count;

        public void Add(T item)
        {
            _items.Add(item);
        }

        public T? GetById(int id)
        {
            foreach (var item in _items)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }
            return default;
        }

        public T[] GetAll()
        {
            return _items.ToArray();
        }

        public bool Remove(int id)
        {
            var item = GetById(id);
            if (item == null)
            {
                return false;
            }

            _items.Remove(item);
            return true;
        }
    }
}