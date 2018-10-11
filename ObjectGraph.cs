using System.Collections.Generic;
using System.Linq;

namespace Algorithms
{
    /// <summary>
    /// Граф объектов
    /// </summary>
    public class ObjectGraph<TObj> : TwoWayGraph
    {
        /// <summary>
        /// Словарь соответствия индекса и объекта, для поиска по хешу
        /// </summary>
        Dictionary<TObj, int> m_dictionary = new Dictionary<TObj, int>();
        /// <summary>
        /// Упорядоченный список объектов
        /// </summary>
        List<TObj> m_list = new List<TObj>();

        /// <summary>
        /// Получение объекта по индексу (индекс отсчитывается от 0)
        /// </summary>
        /// <param name="index">Индекс</param>
        /// <returns>Возвращает объект по указаному индексу</returns>
        public TObj this[int index] { get { return m_list[index + 1]; } } //первый элемент - пустышка. Ожидается отсчёт от 0

        public ObjectGraph() : base() { }

        public ObjectGraph(int capacity) : base(capacity)
        {
            m_list = new List<TObj>(capacity);
            m_dictionary = new Dictionary<TObj, int>(capacity);
        }

        /// <summary>
        /// Добавление объекта в Граф
        /// </summary>
        /// <param name="obj">Уникальный объект</param>
        public void Add(TObj obj)
        {
            if (m_list.Count == 0)
                m_list.Add(default(TObj)); //добавляем пустой элемент, т.к. элемента с ID 0 никогда не будет (для синхронизации)

            //добавляем сначала элемент. Если что, йобнется на стандартной ошибке
            m_dictionary.Add(obj, -1);

            base.Add();

            m_dictionary[obj] = m_list.Count;
            m_list.Add(obj);
        }

        /// <summary>
        /// Соединение двух объектов однонаправленной связью
        /// </summary>
        /// <param name="from">Начальный</param>
        /// <param name="to">Конечный</param>
        public void Connect(TObj from, TObj to)
        {
            var fromI = m_dictionary[from];
            var toI = m_dictionary[to];

            base.Connect_OneWay(fromI, toI);
        }

        /// <summary>
        /// Соединение двух объектов двунаправленной связью
        /// </summary>
        /// <param name="from">Начальный</param>
        /// <param name="to">Конечный</param>
        public void Connect_TwoWay(TObj from, TObj to)
        {
            var fromI = m_dictionary[from];
            var toI = m_dictionary[to];

            base.Connect_Both(fromI, toI);
        }

        /// <summary>
        /// Индекс объекта
        /// </summary>
        /// <returns>Возвращает индекс объекта в графе или -1</returns>
        public int IndexOf(TObj obj)
        {
            if (!m_dictionary.ContainsKey(obj))
                return -1;

            return m_dictionary[obj];
        }

        /// <summary>
        /// Получение списка связанных объектов
        /// </summary>
        /// <param name="obj">Объект для поиска</param>
        /// <returns>Возвращает список связанных объектов</returns>
        public IEnumerable<TObj> GetConnected(TObj obj)
        {
            var index = m_dictionary[obj];

            var connected = base.GetConnected(index);

            return connected.Select(i => m_list[i]);
        }

        public override void Clear()
        {
            m_dictionary.Clear();
            m_list.Clear();

            base.Clear();
        }
    }
}
