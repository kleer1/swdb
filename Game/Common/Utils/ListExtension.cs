namespace SWDB.Game.Common.Utils
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
    }
}