using System.Collections;

namespace SWDB.Game.Utils
{
    public static class ListExtension
    {
        public static T Pop<T>(this IList<T> list)
        {
            T r = list[0];
            list.RemoveAt(0);
            return r;
        }

        public static class ThreadSafeRandom
        {
            [ThreadStatic] private static Random? Local;

            public static Random ThisThreadsRandom
            {
                get { return Local ??= new Random(unchecked(Environment.TickCount * 31 + Environment.CurrentManagedThreadId)); }
            }
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                (list[n], list[k]) = (list[k], list[n]);
            }
        }

        public static void RemoveAll<T>(this IList<T> list, IList<T> remove)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (remove.Contains(list[i]))
                {
                    list.RemoveAt(i);
                }
            }
        }

        public static void RemoveAll<T>(this IList<T> list,T remove)
        {
            if (remove == null)
            {
                return;
            }

            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (remove.Equals(list[i]))
                {
                    list.RemoveAt(i);
                }
            }
        }

        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (items == null) throw new ArgumentNullException(nameof(items));

            if (list is List<T> asList)
            {
                asList.AddRange(items);
            }
            else
            {
                foreach (var item in items)
                {
                    list.Add(item);
                }
            }
        }

        public class CastedList<TTo, TFrom> : IList<TTo>
        {
            public IList<TFrom> BaseList;

            public CastedList(IList<TFrom> baseList)
            {
                BaseList = baseList;
            }

            public CastedList()
            {
                BaseList = new List<TFrom>();
            }

            // IEnumerable
            IEnumerator IEnumerable.GetEnumerator() { return BaseList.GetEnumerator(); }

            // IEnumerable<>
            public IEnumerator<TTo> GetEnumerator() { return new CastedEnumerator<TTo, TFrom>(BaseList.GetEnumerator()); }

            // ICollection
            public int Count { get { return BaseList.Count; } }
            public bool IsReadOnly { get { return BaseList.IsReadOnly; } }
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Possible null reference argument.
            public void Add(TTo item) { BaseList.Add((TFrom)(object)item); }
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            public void Clear() { BaseList.Clear(); }
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Possible null reference argument.
            public bool Contains(TTo item) { return BaseList.Contains((TFrom)(object)item); }
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            public void CopyTo(TTo[] array, int arrayIndex) { BaseList.CopyTo((TFrom[])(object)array, arrayIndex); }
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Possible null reference argument.
            public bool Remove(TTo item) { return BaseList.Remove((TFrom)(object)item); }
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8603 // Possible null reference return.
            public TTo Pop() { return (TTo)(object)BaseList.Pop(); }
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            // IList
            public TTo this[int index]
            {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8603 // Possible null reference return.
                get { return (TTo)(object)BaseList[index]; }
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                set { BaseList[index] = (TFrom)(object)value; }
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            }

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Possible null reference argument.
            public int IndexOf(TTo item) { return BaseList.IndexOf((TFrom)(object)item); }
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Possible null reference argument.
            public void Insert(int index, TTo item) { BaseList.Insert(index, (TFrom)(object)item); }
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            public void RemoveAt(int index) { BaseList.RemoveAt(index); }
        }

        public class CastedEnumerator<TTo, TFrom> : IEnumerator<TTo>
        {
            public IEnumerator<TFrom> BaseEnumerator;

            public CastedEnumerator(IEnumerator<TFrom> baseEnumerator)
            {
                BaseEnumerator = baseEnumerator;
            }

            // IDisposable
            public void Dispose() { BaseEnumerator.Dispose(); }

            // IEnumerator
#pragma warning disable CS8603 // Possible null reference return.
            object IEnumerator.Current => BaseEnumerator.Current;
#pragma warning restore CS8603 // Possible null reference return.
            public bool MoveNext() { return BaseEnumerator.MoveNext(); }
            public void Reset() { BaseEnumerator.Reset(); }

            // IEnumerator<>
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8603 // Possible null reference return.
            public TTo Current { get { return (TTo)(object)BaseEnumerator.Current; } }
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        }

        public static IList<TTo> CastList<TFrom, TTo>(this IList<TFrom> list)
        {
            return new CastedList<TTo, TFrom>(list);
        }
    }
}