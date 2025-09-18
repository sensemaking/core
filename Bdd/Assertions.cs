using System;
using System.Collections.Generic;
using System.Linq;
using System.Serialization;
using NUnit.Framework;

namespace Sensemaking.Bdd
{
    public static class AssertionExtensions
    {
        public static void should_pass(this object o)
        {
            Assert.Pass();
        }

        public static void should_fail(this object o)
        {
            Assert.Fail();
        }

        public static void should_fail(this object o, string message)
        {
            Assert.Fail(message);
        }

        public static void should_be_true(this bool condition)
        {
            Assert.That(condition, Is.True);
        }

        internal static void should_be_true(this bool condition, string message)
        {
            Assert.That(condition, Is.True, message);
        }

        public static void should_be_false(this bool condition)
        {
            Assert.That(condition, Is.False);
        }

        internal static void should_be_false(this bool condition, string message)
        {
            Assert.That(condition, Is.False, message);
        }

        public static void should_be(this object actual, object expected)
        {
            Assert.That(actual, Is.EqualTo(expected), $"Expected {expected} but was {actual}");
            Assert.That(actual?.GetHashCode(), Is.EqualTo(expected?.GetHashCode()), $"Expect Hashcode to be equal when objects are equal, expected {expected} but was {actual}");
        }

        public static void should_be(this string actual, string expected)
        {
            (actual as object).should_be(expected);
        }

        public static void should_be(this int actual, int expected)
        {
            Assert.That(actual, Is.EqualTo(expected), $"Expected {expected} but was {actual}");
        }

        public static void should_be<T>(this IEnumerable<T> actual, IEnumerable<T> expected, bool iCareAboutOrdering = true)
        {
            if (iCareAboutOrdering)
                actual.HasSameContentsInSameOrder(expected).should_be_true("The two enumerables were expected to have the same contents in the same order");
            else
                actual.HasSameContents(expected).should_be_true("The two enumerables were expected to have the same contents");
        }

        public static void should_be<T, U>(this IEnumerable<T> actual, IEnumerable<U> expected, Func<T, U, bool> predicate)
        {
            Assert.That(actual.Count(), Is.EqualTo(expected.Count()), "The two enumerables were expected to be the same and satistify the predicate");
            Assert.That(expected.All(x => actual.Any(y => predicate(y, x))), Is.True, "The two enumerables were expected to be the same and satistify the predicate");
        }

        public static void should_not_be(this object actual, object expected)
        {
            Assert.That(actual, Is.Not.EqualTo(expected), $"Expected {expected} not to be {actual}");
        }

        public static void should_not_be<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
        {
            actual.HasSameContents(expected).should_be_false("The two enumerables should not have the same contents");
        }

        public static void should_be_greater_than(this IComparable actual, IComparable expected)
        {
            Assert.That(actual, Is.GreaterThan(expected));
        }

        public static void should_be_greater_than_or_equal_to(this IComparable arg1, IComparable arg2)
        {
            Assert.That(arg1, Is.GreaterThanOrEqualTo(arg2));
        }

        public static void should_be_less_than(this IComparable arg1, IComparable arg2)
        {
            Assert.That(arg1, Is.LessThan(arg2));
        }

        public static void should_be_less_than_or_equal_to(this IComparable arg1, IComparable arg2)
        {
            Assert.That(arg1, Is.LessThanOrEqualTo(arg2));
        }

        public static void should_be_null(this object value)
        {
            Assert.That(value, Is.Null);
        }

        public static void should_not_be_null(this object value)
        {
            Assert.That(value, Is.Not.Null);
        }

        public static void should_be_empty(this string value)
        {
            Assert.That(value, Is.Empty);
        }

        public static void should_not_be_empty(this string value)
        {
            Assert.That(value, Is.Not.Empty);
        }

        public static void should_be_empty<T>(this IEnumerable<T> collection)
        {
            Assert.That(collection.Any(), Is.False);
        }

        public static void should_not_be_empty<T>(this IEnumerable<T> collection)
        {
            Assert.That(collection.Any(), Is.True);
        }

        public static void should_contain(this string actual, string expected)
        {
            Assert.That(actual.Contains(expected), Is.True, $"{expected} was not found");
        }

        public static void should_not_contain(this string actual, string expected)
        {
            Assert.That(actual.Contains(expected), Is.Not.True, $"{expected} was found");
        }

        public static void should_contain<T>(this IEnumerable<T> actual, T expected)
        {
            Assert.That(actual, Contains.Item(expected), $"{expected?.Serialize() ?? "null"}\r\n\r\nwas not found in\r\n\r\n{actual?.Serialize() ?? "null"}");
        }
        
        public static void should_not_contain<T>(this IEnumerable<T> actual, T expected)
        {
            Assert.That(actual.Contains(expected), Is.Not.True, $"{expected?.Serialize() ?? "null"}\r\n\r\nwas found in\r\n\r\n{actual?.Serialize() ?? "null"}");
        }
        
        public static void should_not_contain<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
        {
            Assert.That(actual.Any(expected.Contains), Is.Not.True, $"At least one of {expected?.Serialize() ?? "null"}\r\n\r\nwas found in\r\n\r\n{actual?.Serialize() ?? "null"}");
        }

        public static void should_all_be<T>(this IEnumerable<T> actual, T expected)
        {
            Assert.That(actual.All(i => i!.Equals(expected)), Is.True, $"Expected {expected?.Serialize() ?? "null"}\r\n\r\nwas not found in all elements of\r\n\r\n{actual?.Serialize() ?? "null"}");
        }

        public static void should_contain<T, U>(this IEnumerable<T> actual, U expected, Func<T, U, bool> predicate)
        {
            Assert.That(actual.Any(y => predicate(y, expected)), $"{expected?.Serialize() ?? "null"}\r\n\r\nwas not found in\r\n\r\n{actual?.Serialize() ?? "null"}");
        }

        public static void should_contain<T>(this IEnumerable<T> actual, Func<T, bool> predicate)
        {
            Assert.That(actual.Any(predicate.Invoke), Is.True);
        }

        public static void should_not_contain<T>(this IEnumerable<T> actual, Func<T, bool> predicate)
        {
            Assert.That(actual.Any(predicate.Invoke), Is.Not.True);
        }

        public static void should_contain_all<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
        {
            expected.ForEach(actual.should_contain);
        }

        public static void should_contain_all<T, U>(this IEnumerable<T> actual, IEnumerable<U> expected, Func<T, U, bool> predicate)
        {
            expected.ForEach(e => actual.should_contain(e, predicate));
        }

        public static void should_be_instance_of<T>(this object actual)
        {
            Assert.That(actual, Is.InstanceOf(typeof(T)));
        }
    }
}
