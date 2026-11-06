namespace ClinicApp.Models
{
    public class WaitingQueue<T>
    {
        private Queue<T> _queue = new Queue<T>();

        public int Count => _queue.Count;

        public bool IsEmpty => _queue.Count == 0;

        public void Enqueue(T item)
        {
            _queue.Enqueue(item);
        }

        public T Dequeue()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("The waiting queue is empty. Cannot dequeue.");
            }
            return _queue.Dequeue();
        }

        public T Peek()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("The waiting queue is empty. Cannot peek.");
            }
            return _queue.Peek();
        }

        public T[] ToArray()
        {
            return _queue.ToArray();
        }
    }
}