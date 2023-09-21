using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System
{
    public static class Enumerables
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var x in source)
                action.Invoke(x);
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            var index = 0;
            foreach (var x in source)
                action.Invoke(x, index++);
        }
        
        public static async Task ForEach<T>(this IEnumerable<T> source, Func<T, Task> action)
        {
            foreach (var x in source)
                await action.Invoke(x);
        }

        public static async Task ForEach<T>(this IEnumerable<T> source, Func<T, int, Task> action)
        {
            var index = 0;
            foreach (var x in source)
                await action.Invoke(x, index++);
        }

        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int count)
        {
            return source.Reverse().Take(count).Reverse();
        }

        public static bool None<T>(this IEnumerable<T> source)
        {
            return !source.Any();
        }

        public static bool None<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            return !source.Any(predicate);
        }

        public static bool HasSameContentsInSameOrder<T>(this IEnumerable<T> @this, IEnumerable<T> that)
        {
            return @this.IsEquivalent(that, true);
        }

        public static bool HasSameContents<T>(this IEnumerable<T> @this, IEnumerable<T> that)
        {
            return @this.IsEquivalent(that, false);
        }

        private static bool IsEquivalent<T>(this IEnumerable<T> @this, IEnumerable<T> that, bool iCareAboutOrdering)
        {
            var these = @this?.ToList();
            var those = that?.ToList();

            if (these.IsNullOrEmpty() && those.IsNullOrEmpty())
                return true;

            if (these == null)
                return false;

            return iCareAboutOrdering 
                ? these.GetHashCodeUsingContents() == those!.GetHashCodeUsingContents() 
                : these.GetOrderedHashCodeUsingContents() == those!.GetOrderedHashCodeUsingContents();
        }
        
        public static int GetHashCodeUsingContents<T>(this IEnumerable<T> source)
        {
            return (source.ToArray() as IStructuralEquatable).GetHashCode(EqualityComparer<T>.Default);
        }

        private static int GetOrderedHashCodeUsingContents<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => x?.GetHashCode()).GetHashCodeUsingContents();
        }

        private static bool IsNullOrEmpty<T>(this IEnumerable<T>? source)
        {
            return source == null || source.None();
        }
    }
}
