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
    }
}