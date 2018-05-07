using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QueueTestApp
{
    //объект очереди с ожиданием элемента при пустой очереди
    public class AwaitQueue<T>
    {
        //блокировка ожидания хотя бы одного элемента
        private object waitLocker = new object();

        //собственно, сам контейнер очереди
        private Queue<T> queue = new Queue<T>();

        //свойство, возвращающее список элементов очереди (для отображения на форме)
        public List<T> QueueList
        {
            get
            {
                List<T> res = new List<T>();

                foreach (T item in queue)
                    res.Add(item);

                return res;
            }
        }

        //операция добавления элемента в очередь
        public void push(T value)
        {
            lock (waitLocker)
            {
                queue.Enqueue(value);
                Monitor.Pulse(waitLocker);
            }
        }

        //операция изъятия элеменнта из очереди
        public T pop()
        {
            T res = default(T);

            lock (waitLocker)
            {
                if (queue.Count == 0)
                    Monitor.Wait(waitLocker);
                if (queue.Count != 0)
                    res = queue.Dequeue();
            }

            return res;
        }
    }
}