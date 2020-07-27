using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Sensemaking.Http
{
    public readonly struct Problem
    {
        public static readonly Problem Empty = new Problem(string.Empty);

        public Problem(string title, params string[] errors)
        {
            this.title = title;
            this.errors = errors;
        }

        private readonly string title;
        public string Title => title ?? string.Empty;

        private readonly string[] errors;
        public string[] Errors => errors ?? new string[0];

        #region Equality

        public static bool operator ==(Problem @this, Problem that)
        {
            return @this.Equals(that);
        }

        public static bool operator !=(Problem @this, Problem that)
        {
            return !@this.Equals(that);
        }

        public override bool Equals(object that)
        {
            return that is Problem problem && Equals(problem);
        }

        public bool Equals(Problem that)
        {
            return this.Title == that.Title && this.Errors.SequenceEqual(that.Errors);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Title, Errors);
        }

        #endregion
    }

    internal static class ProblemExtensions
    {
        internal static bool IsError(this HttpResponseMessage response)
        {
            return (int) response.StatusCode >= 400 && (int) response.StatusCode < 600;
        }

        internal static bool IsProblem(this HttpResponseMessage response)
        {
            return response.Content?.Headers.ContentType.MediaType == MediaType.JsonProblem;
        }
    }
}