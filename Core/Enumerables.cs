﻿using System.Collections.Generic;
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

        public static void ForEach<T>(this IEnumerable<T> source, Action<T, uint> action)
        {
            var index = (uint) 0;
            foreach (var x in source)
                action.Invoke(x, index++);
        }
        
        public static async Task ForEach<T>(this IEnumerable<T> source, Func<T, Task> action)
        {
            foreach (var x in source)
                await action.Invoke(x);
        }

        public static async Task ForEach<T>(this IEnumerable<T> source, Func<T, uint, Task> action)
        {
            var index = (uint) 0;
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

        public static void Times(this int source, Action action)
        {
            for (var i = 0; i < source; i++)
                action();
        }

        public static void Times(this int source, Action<int> action)
        {
            for (var i = 0; i < source; i++)
                action(i);
        }
    }
}
