using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Algorithms
{
    /// <summary>
    /// Граф
    /// </summary>
    public abstract class Graph
    {
        /// <summary>
        /// Количество используемых бит long'а
        /// </summary>
        protected const int LongLength = 63;
        protected const long LongOne = 1L;
        protected const long LongZero = 0L;

        int m_nodeCount = 0;
        int m_longCount = 1;
        int m_curentMaxIndex = LongLength;

        /// <summary>
        /// Массив готовых long'ов для побитового сравнения
        /// </summary>
        protected IReadOnlyList<long> m_longs;
        protected readonly SortedDictionary<int, long[]> m_dataMatrix = new SortedDictionary<int, long[]>();

        /// <summary>
        /// Количество узлов
        /// </summary>
        public int NodeCount { get { return m_nodeCount; } }

        /// <summary>
        /// Количество связей
        /// </summary>
        public int ConnectionsCount { get; private set; }

        public Graph() { ToDefault(); SetLongs(); }

        /// <summary>
        /// Ожидаемый размер
        /// </summary>
        /// <param name="capacity">Размер</param>
        public Graph(int capacity) : this()
        {
            if (capacity < 0)
                throw new IndexOutOfRangeException("Размерность массива не может быть меньше нуля");

            if (capacity > LongLength)
            {
                //получаем ожидаемое long'ов, в которые влезает индекс
                m_longCount = CalculateLongCount(capacity);
                m_curentMaxIndex = m_longCount * LongLength;
            }
        }

        /// <summary>
        /// Добавление нового узла
        /// </summary>
        /// <returns>Возвращает ID узла</returns>
        protected int Add()
        {
            //добавление новых узлов делаем последовательно
            m_nodeCount++;

            if (m_nodeCount > m_curentMaxIndex)
            {
                var newCapacity = CalculateLongCount(m_nodeCount);
                    ResizeLongArrays(newCapacity);
            }

            m_dataMatrix.Add(m_nodeCount, new long[m_longCount]);

            return m_nodeCount;
        }

        /// <summary>
        /// Однонаправленная связь
        /// </summary>
        /// <param name="node_from">Начальный узел</param>
        /// <param name="node_to">Конечный узел</param>
        protected void Connect_OneWay(int node_from, int node_to)
        {
            //получение long'a для элемента
            m_dataMatrix[node_from] //берём набор long'ов по индексу элемента
                [node_to > LongLength ? CalculateLongCount(m_nodeCount) - 1 : 0] //и переходим по индексу long'a, в зависимости от индекса

                //проставляем флаг
                |= (LongOne << ((node_to > LongLength) //проверяем, выходит ли индекс элемента за число используемых битов long'а
                ? (node_to % LongLength) //для больших индексов берём остаток от деления
                : node_to)); //для небольших элементов берём текущий индекс

            ConnectionsCount++;
        }
        
        /// <summary>
        /// Получение списка всех соседних узлов
        /// </summary>
        /// <param name="node">Узел для проверки</param>
        /// <returns>Возвращает список соседних узлов или пустой список</returns>
        protected virtual internal IEnumerable<int> GetConnected(int node)
        {
            if (!m_dataMatrix.ContainsKey(node))
                return Enumerable.Empty<int>();

            var arr = m_dataMatrix[node];

            if(arr.Length == 0)
                return Enumerable.Empty<int>();

            List<int> connections = new List<int>();
            for(int i = 0; i < arr.Length; i++)
            {
                var l = arr[i];
                if (l == LongZero) //связей нет в текущем диапазоне
                    continue;

                var firstIndex = i * LongLength; //начальный индекс текущего диапазона

                for(int j = 0; j < LongLength; j++)
                {
                    if((l & m_longs[j]) != LongZero) //есть флаг
                    {
                        connections.Add(firstIndex + j);
                    }
                }
            }

            return connections;
        }

        void ResizeLongArrays(int newCapacity)
        {
            if (m_dataMatrix.Count == 0)
                return;

            long[] arr = null;
            var oldMatrix = new Dictionary<int, long[]>(m_dataMatrix);
            foreach (var kvp in oldMatrix)
            {
                arr = kvp.Value;
                Array.Resize(ref arr, newCapacity);

                m_dataMatrix[kvp.Key] = arr;
            }

            m_longCount = newCapacity;
            m_curentMaxIndex = m_longCount * LongLength;
        }

        int CalculateLongCount(int nodeIndex, int count = 1)
        {
            var temp = nodeIndex - LongLength;

            if (temp > 0)
                count += CalculateLongCount(temp, count);

            return count;
        }

        protected virtual void ToDefault()
        {
            m_nodeCount = 0;
            m_longCount = 1;
            m_curentMaxIndex = LongLength;
        }

        public virtual void Clear()
        {
            m_dataMatrix.Clear();

            ToDefault();
        }

        void SetLongs()
        {
            var longs = new long[LongLength];

            for (int i = 0; i < LongLength; i++)
                longs[i] = 1L << i;

            m_longs = new List<long>(longs).AsReadOnly();
        }
    }
}
