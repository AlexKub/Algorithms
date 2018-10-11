using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
    public abstract class TwoWayGraph : Graph
    {

        public TwoWayGraph() : base()
        {

        }

        public TwoWayGraph(int capacity) : base(capacity)
        {

        }
        /// <summary>
        /// Двунаправленная связь
        /// </summary>
        /// <param name="node_1">Узел 1</param>
        /// <param name="node_2">Узел 2</param>
        protected void Connect_Both(int node_1, int node_2)
        {
            Connect_OneWay(node_1, node_2);

            Connect_OneWay(node_2, node_1);
        }

        protected internal override IEnumerable<int> GetConnected(int node)
        {
            if (!m_dataMatrix.ContainsKey(node))
                return Enumerable.Empty<int>();

            var arr = m_dataMatrix[node];

            if (arr.Length == 0)
                return Enumerable.Empty<int>();

            List<int> connections = new List<int>();
            for (int i = 0; i < arr.Length; i++)
            {
                var l = arr[i];
                if (l == LongZero) //связей нет в текущем диапазоне
                    continue;

                var firstIndex = i * LongLength; //начальный индекс текущего диапазона

                for (int j = 0; j < LongLength; j++)
                {
                    if ((l & m_longs[j]) != LongZero) //есть флаг
                    {
                        connections.Add(firstIndex + j);
                    }
                }
            }

            return connections;
        }
    }
}
