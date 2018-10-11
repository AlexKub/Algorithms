using System;
using System.Collections.Generic;

namespace Algorithms
{
    public static class RandomGen
    {
        static readonly Random m_Rand = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// Получение случайного набора цифр
        /// </summary>
        /// <param name="min">Минимальное значение (включительно)</param>
        /// <param name="max">Максимальное значение (не включительно)</param>
        /// <param name="count">Количество цифр</param>
        /// <returns>Возвращает случайный набор цифр в указанном диапазоне</returns>
        public static IEnumerable<int> Numbers(int min, int max, int count)
        {
            var list = new List<int>();

            for (int i = 0; i < count; i++)
            {
                list.Add(m_Rand.Next(min, max));
            }

            return list;
        }
    }
}
