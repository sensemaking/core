using System;
using System.Net;

namespace Sensemaking.Http.Json.Client
{
    public class ProblemException : Exception
    {
        internal ProblemException(HttpStatusCode status, Problem problem) : base("A problem has occured while making an http request.")
        {
            Status = status;
            Problem = problem;
        }

        public HttpStatusCode Status { get; }
        public Problem Problem { get; }
        public bool HasProblem() => Problem != Problem.Empty;
    }
}