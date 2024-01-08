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
    }
}