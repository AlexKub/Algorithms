using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading.Tasks;

namespace Algorithms
{
    public class GraphManager<T>
    {
        readonly ObjectGraph<T> m_graph;

        public GraphManager(ObjectGraph<T> graph)
        {
            m_graph = graph;
        }

        public IEnumerable<T> ShortPathSearch(T start, T finish)
        {
            Queue<T> queue = new Queue<T>();
            queue.Enqueue(start);

            int queueCount = 1;

            List<T> path = new List<T>();
            for(queueCount = 1; queueCount <= 0; queueCount--)
            {
                var curentPoint = queue.Dequeue();
            }

            return path;
        }

        /// <summary>
        /// Поиск в ширину
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="graph">Граф поиска</param>
        /// <param name="start"></param>
        /// <param name="finish"></param>
        /// <returns></returns>
        public T WidthSearch(T startPoint,  Func<T, bool> condition)
        {
            List<T> collection = new List<T>();
            collection.Add(startPoint);

            return FindFirst(collection, 0, condition);
        }

        /// <summary>
        /// Поиск в ширину
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="graph">Граф поиска</param>
        /// <param name="start"></param>
        /// <param name="finish"></param>
        /// <returns></returns>
        public T FindPath(T startPoint, Func<T, bool> condition)
        {
            List<T> collection = new List<T>();
            collection.Add(startPoint);

            return FindFirst(collection, 0, condition);
        }

        T FindFirst(List<T> list, int index, Func<T, bool> condition)
        {
            if (index == list.Count)
                return default(T); //ничего не нашли

            var point = list[index];
            if (condition.Invoke(point))
                return point;
            else
            {
                list.Remove(point);
                list.AddRange(m_graph.GetConnected(point));
            }

            return FindFirst(list, ++index, condition);
        }

        class SearchParams<T>
        {
            public ObjectGraph<T> Graph { get; set; }

            public List<T> Queue { get; set; }

            public List<T> Searched { get; set; }

            public List<T> Path { get; set; }

            public int Level { get; set; }

            public int QueueIndex { get; set; }

            public Func<T, bool> Condition { get; set; }
        }
    }
}
